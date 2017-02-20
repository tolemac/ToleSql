>Se aceptan issues en español.

# ToleSql

> ToleSql es generador de SQL para proyectos .NET

ToleSql está construido sobre dos capas, una base (SelectBuilder) que admite fragmentos de SQL en texto y otra (ExpressionSelectBuilder) que admite expressiones que son traducidas a SQL.

#### SelectBuilder
En este caso un framento de código dice más que mil palabras:

~~~~ csharp
[Fact]
public void SelectTableWithColumnsExplicitJoinWhereOrderBy()
{
    SqlConfiguration.SetDialect(new TestDialect());
    var b = new SelectBuilder();

    b.SetMainSourceSql("[WH].[INVOICE]");
    b.AddColumnSql("T0.DATE, T0.TOTAL");
    b.AddColumnSql("T1.NAME, T1.BALANCE");
    b.AddJoinSql("[CUS].[CUSTOMER]", "T1.CUSTOMERID = T2.ID");

    var spec = "SELECT T0.DATE, T0.TOTAL, T1.NAME, T1.BALANCE FROM [WH].[INVOICE] AS [T0] INNER JOIN [CUS].[CUSTOMER] AS [T1] ON T1.CUSTOMERID = T2.ID";
    var gen = b.GetSqlText();
    Assert.Equal(spec, gen);
}
~~~~

#### ExpressionSelectBuilder

Aquí va un test:
~~~~ csharp
[Fact]
public void SelectColumnsJoinsAndWhere()
{
    SqlConfiguration.SetDialect(new TestDialect());
    SetModeling();
    var b = new ExpressionSelectBuilder();
    b.SetMainTable<DeliveryNote>();
    b.AddJoin<DeliveryNote, DeliveryNoteDetail>((i, id) => i.Id == id.DeliveryNoteId);
    b.AddJoin<DeliveryNote, Supplier>((i, c) => i.SupplierId == c.Id);
    b.Select<DeliveryNote, Supplier>((i, c) => i.Number, (i, c) => c.Name, (i, c) => i.Date, (i, c) => i.TotalAmount);
    b.Select<DeliveryNoteDetail>((id) => id.Size);
    b.Where<Supplier>(c => c.IsDeleted);
    b.Where<DeliveryNote, DeliveryNoteDetail>((i, id) => i.Year == "B" && id.IsDeleted == false);
    b.Where<DeliveryNote, DeliveryNoteDetail>(WhereOperator.Or, (i, id) => i.Year == "Z");

    var gen = b.GetSqlText();
    var spec = "SELECT [T0].[Number], [T2].[Name], [T0].[Date], [T0].[TotalAmount], [T1].[Size] FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[DeliveryNoteDetail] AS [T1] ON ([T0].[Id] = [T1].[DeliveryNote_Id]) INNER JOIN [WH].[Supplier] AS [T2] ON ([T0].[SupplierId] = [T2].[Id]) WHERE ([T2].[IsDeleted] = @SqlParam0) AND (([T0].[Year] = @SqlParam1) AND ([T1].[IsDeleted] = @SqlParam2)) OR ([T0].[Year] = @SqlParam3)";

    Assert.Equal(spec, gen);
    Assert.Equal(b.Parameters["SqlParam0"], true);
    Assert.Equal(b.Parameters["SqlParam1"], "B");
    Assert.Equal(b.Parameters["SqlParam2"], false);
    Assert.Equal(b.Parameters["SqlParam3"], "Z");
}
~~~~

Se parte de la idea de que una consulta no tiene por que tener una tabla principal, por eso la clase principal no usa generics. Sin embargo al añadir cosas si que habrá que especificar los tipos de los objetos que intervienen. Por ejemplo cuando añado una columna:

~~~~ csharp
    b.Select<DeliveryNoteDetail>((id) => id.Size);
~~~~

Especifico el tipo `DeliveryNoteDetail` para indicar que quiero la columna correspondiente a la propiedad `Size`.
En el caso de querer indicar columnas de varias tablas, debo indicar los distintos tipos:

~~~~ csharp
    b.Select<DeliveryNote, Supplier>((i, c) => i.Number, (i, c) => c.Name, (i, c) => i.Date, (i, c) => i.TotalAmount);
~~~~

Incluso puedo seleccionar columnas que son combinación de otras:

~~~~ csharp
    b.Select<DeliveryNote, Supplier>((i, c) => i.Number + "-" + s.Name);
~~~~

Esto es igual para el `Where`. También se acepta `OrderBy` y pronto `GroupBy` y `Having`, cuando usamos éstos los tipos que intervienen han debido ser incluidos como tablas en la consulta, ya sea como tabla principal o como un `join`. Si no es así falla y te obliga a añadir dicho tipo a la consulta.

También podemos proyectar el resultado a un objeto de forma que se asignan aliases a las columnas resultantes:

```` csharp
[Fact]
public void SelectProjection()
{
    SqlConfiguration.SetDialect(new TestDialect());
    SetModeling();
    var b = new ExpressionSelectBuilder();
    b.SetMainTable<DeliveryNote>();
    b.AddJoin<DeliveryNote, Supplier>((dn, s) => dn.SupplierId == s.Id);
    b.Select<DeliveryNote, Supplier>((dn, s) => new DeliveryNoteDto
    {
        Number = dn.Number,
        TotalAmount = dn.TotalAmount,
        Date = dn.Date,
        SupplierName = s.Name
    });

    var gen = b.GetSqlText();
    var spec = "SELECT [T0].[Number] AS Number, [T0].[TotalAmount] AS TotalAmount, [T0].[Date] AS Date, [T1].[Name] AS SupplierName FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id])";

    Assert.Equal(spec, gen);
}
````



A la hora de generar el SQL, ToleSql puede generar nombres de tablas y campos distintos a los nombres de tipos y propiedades. Como no me gustan los `attributes` por ser invasivos, se usa una especie de configuración que llamo `Modeling`:

~~~~ csharp
private void SetModeling()
{
    Modeling.ResetModeling();
    Modeling.Model<Supplier>().SetSchema("WH");
    Modeling.Model<DeliveryNote>().SetSchema("WH");
    Modeling.Model<DeliveryNoteDetail>().SetSchema("WH");
    Modeling.Model<Product>().SetSchema("WH");
    Modeling.Model<EquivalentProduct>().SetSchema("WH");
    Modeling.Model<DeliveryNoteDetail>().SetColumnName(dnd => dnd.DeliveryNoteId, "DeliveryNote_Id");
    Modeling.Model<User>()
        .SetSchema("LoB")
        .SetTable("SecurityProfile")
        .SetColumnName(u => u.CreatedBy, "CreatedBy_Id");
}
~~~~

Con `Modeling` se puede especificar el nombre de cada tabla y el schema de cada una de ellas, también podemos especificar nombres de columnas. De momento esto se hace de forma global, pero además de esto también me gustaría añadir la posibilidad de `Modelar` por consulta, de forma que cada consulta miraría en su `Modeling` y luego en el global.

Por otra parte también se ha creado teniendo en cuenta que se pueda generar SQL para distintas bases de datos, hay una clase base para crear dialectos (DialectBase), donde se establecen las palabras clave de SQL, simbolos y posibles formas de generar los nombres de nuestras tablas y columnas. De momento incorpora un dialecto para SQLServer que es con el que se están haciendo las pruebas. En un futuro si se crea otro dialecto habrá que ver si la forma escogida para hacerlo es la correcta.

De momento está en fase de construcción, está desarrollado en .NET Core (.Net Standard 1.6) y también se puede usar desde .NET Framework.

Actualmente se está intentando que pueda tragar cualquier cosa, e incluso se pueda extender de forma fácil. La siguiente fase será hacerla más fácil de usar con clases que admitan métodos LinQ que al final usaran `ExpressionSelectBuilder` para ir montando las consultas.

## Instalación via NuGet

Está disponible [en NuGet](https://www.nuget.org/packages/ToleSql/)

````
Install-Package ToleSql
````
