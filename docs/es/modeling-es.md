# Modelado

### Modelado global

Algunas veces las tablas de la base de datos no se llaman como las clases de nuestros objetos, igual con los nombres de columnas e incluso podemos usar distintos esquemas donde clasificamos nuestras tablas, para ello tenemos la clase `Modeling`.

Para ello podemos hacer esto:

```` csharp
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
````

Esto establecería los valores de forma global. Siempre que se vaya a resolver el tipo `Supplier` el esquema será `WH`, siempre que se resuelva el tipo `User` se usará el esquema `LoB` y el nombre de tabla `SecurityProfile`.

También podemos establecer un esquema por defecto para todas las tablas que no tengan asociado un esquema:

```` csharp
Modeling.DefaultSchema = "dbo";
````

### Modelado por builder

Alternativamente podemos establecer configuración de modelado a nivel del propio builder.   
En primer lugar un tipo se traduce en tabla usando el nombre del tipo y sin esquema, el siguiente paso es establecerle el esquema por defecto si se ha definido. Entonces se busca el tipo en el `Modeling` global y por último se buscan en el `Modeling` del builder. Por lo tanto prevalece la configuración más concreta.

Aquí unos tests que lo demuestran.

Esquema por defecto:
```` csharp
Modeling.DefaultSchema = "KKK";
var b = new SelectBuilder();
b.From<Supplier>();

var gen = b.GetSqlText();
var spec = "SELECT * FROM [KKK].[Supplier] AS [T0]";
Assert.Equal(spec, gen);
````

Esquema por defecto + modelado global:
```` csharp
Modeling.DefaultSchema = "KKK";
Modeling.Model<Supplier>().SetSchema("CUS").SetTable("CUSTOMER");
var b = new SelectBuilder();
b.From<Supplier>();

var gen = b.GetSqlText();
var spec = "SELECT * FROM [CUS].[CUSTOMER] AS [T0]";
Assert.Equal(spec, gen);
````

Esquema por defecto + modelado global + modelado por builder:
```` csharp
Modeling.ResetModeling();
Modeling.DefaultSchema = "KKK";
Modeling.Model<Supplier>().SetSchema("CUS").SetTable("CUSTOMER");

var b = new SelectBuilder();
b.Modeling.Model<Supplier>().SetSchema("dbo").SetTable("MyTable");
b.From<Supplier>();

var gen = b.GetSqlText();
var spec = "SELECT * FROM [dbo].[MyTable] AS [T0]";

Assert.Equal(spec, gen);
````

En definitiva tenemos distintos modos establecer los nombres de nuestras tablas a nuestros tipos.