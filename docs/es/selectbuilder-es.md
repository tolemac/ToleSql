# SelectBuilder

Usando `SelectBuilder` puedes construir consultas indicando los tipos que intervienen en la llamada a cada método. `SelectBuilder` no depende de ningún tipo, los tipos se indican en las llamadas a los metodos.

Creamos un `SelectBuilder` así:

```` csharp
var builder = new SelectBuilder();
````

### Añadiendo tablas

Una vez que tenemos el builder podemos indicarle el tipo que representa la tabla principal:

```` csharp
builder.From<DeliveryNote>();
````

Y podemos añadir un `JOIN` indicando la expresión de unión entre los tipos, por ejemplo podemos añadir la tabla de proveedores a nuestra tabla de albaranes:
```` csharp
builder.Join<DeliveryNote, Supplier>((dn, s) => dn.SupplierId == s.Id && !s.IsDeleted);
````

Podemos seguir uniendo tablas, por ejemplo ahora añadimos las lineas de albarán:
```` csharp
builder.Join<DeliveryNote, DeliveryNoteDetail>((dn, dnd) => dn.Id == dnd.DeliveryNoteId);
````

En cualquier momento podemos añadir una tabla más con `Join` indicando como primer argumento genérico, una tabla que ya existe en la consulta y como segundo el que queremos añadir, ahora podría añadir por ejemplo un join con el usuario que ha creado el albarán:

```` csharp
builder.Join<DeliveryNote, User>((dn, u) => dn.CreatedBy == u.Id);
````

En casos excepcionales podríamos quere añadir una tabla que requiera una condición con más de una tabla, por ejemplo unir albaranes con lineas de albarán y productos por el usuario de creación, en este caso podemos hacer:

```` csharp
builder.Join<DeliveryNote, DeliveryNoteDetail, Product>((t1, t2, t3) 
    => t1.CreatedBy == t3.CreatedBy 
    && t2.CreatedBy == t3.CreatedBy);
````

Cuando hacemos un `Join` el último argumento genérico usado es la tabla que vamos a unir y los anteriores son tablas que ya deben estar en la consulta o bien como principales o bien unidas con `Join`.

### Seleccionando columnas

Podemos seleccionar columnas con el método `Select`, indicando con generics los tipos de las propiedades que vamos a usar. Los tipos que indicamos como argumentos genéricos deben haber sido añadidos a la consulta con `From` o `Join`. Podríamos añadir campos de albaranes así:

```` csharp
builder.Select<DeliveryNote>(dn => dn.Number, dn => dn.TotalAmount);
````

También podemos usar una proyección para dar nombres a las columnas resultado:

```` csharp
builder.Select<DeliveryNote>(dn => 
    new {
         DeliveryNoteNumber = dn.Number, 
         Total = dn.TotalAmount
    });
````

O usar un tipo existente:

```` csharp
builder.Select<DeliveryNote>(dn => new DeliveryNoteDto 
    {
        DeliveryNoteNumber = dn.Number, 
        Total = dn.TotalAmount
    });
````

También podemos añadir columnas usando varios tipos:

```` csharp
builder.Select<DeliveryNote, Supplier>((dn, s) => new  
    {
        DeliveryNoteNumber = dn.Number, 
        Total = dn.TotalAmount,
        SupplierName = s.Name
    });
````

En las columnas podemos usar funciones ([Funciones SQL](./functions-es.md)):

```` csharp
builder.Select<Supplier>(s => new { SupplierName == s.Name.SubString(0, 10)});
builder.Select<DeliveryNoteDetail>(s => new { TotalLineas == DbFunctions.Count(s.Id)});
````

> DbFunctions es una clase estatica con funciones de base de datos, se puede extender el número de funciones ([Extensibilidad](./extensibility-es.md)).

### Filtrando por campos

Al igual que con `Select`, con `Where` debemos de pasar como argumentos genéricos los tipos de los objetos que manejamos en las expresiones, y estos tipos deben haber sido añadidos a la consulta con `From` o `Join`

```` csharp
builder.Where<Supplier>(s => s.Name.Contains("CocaCola"));
builder.Where<Supplier>(s => s.Name.StartsWith("Coca"));
builder.Where<Supplier>(s => s.Type == "Corporation");
````

Los parametros constantes como `"CocaCola"`, `"Coca"` y `"Corporation"` son añadidos a la consulta como parametros, el dialecto en uso marcará como se nombran dichos parametros, en SqlServer se usa `@SqlParamN` donde `N` es el número de parametro en el orden en que se han introducido.

Estos parametros también pueden ser variables, ToleSql los resolverá y los añadirá como parametros:

```` csharp
var supplierType = "Corporation";
builder.Where<Supplier>(s => s.Type == supplierType);

Assert.Equal(builder.Parameter[0], supplierType);
````

Por supuesto también podemos añadir condicionantes usando distintos tipos:

```` csharp
builder.Where<Supplier, DeliveryNote>((s, dn) => 
    s.Type == "Corporation" 
    && dn.IsDeleted 
    && dn.Year == "2015");
````

## Ordenando y agrupando.

Una vez conocida la dinámica de ToleSql con los `Join`, `Select` y `Where`, es facil ordenar y agrupar:

```` csharp
builder.OrderBy<DeliveryNote>(dn => dn.date);
builder.OrderBy<Supplier>(s => s.Balance);

builder.GroupBy<DeliveryNote>(dn => dn.SupplierId);
builder.Having<DeliveryNote>(dn => 
    DbFunctions.Max(dn.TotalAmount) > 1000);
````

## Añadiendo SQL extremo

Es posible necesitar en algún momento añadir algo que no podemos generar con expresiones, ya sea por que ToleSql no lo soporta o por que no se puede implementar facilmente.

Con ToleSql puedes añadir SQL en texto en cualquier momento, como columna, como origen de datos (FROM), como JOIN o wheres ...
Imagina que queremos meter un `CASE WHEN` en una columna, podríamos hacer esto:

```` csharp
builder.From<Invoice>()
  .Where<Invoice>(i => !i.IsDelted)
  .SelectSql(@"case when serie = 'a' then 'Factura legal' 
case serie = 'b' then 'Factura en negro' 
end as tipofactura");
````

Y quedarnos tan anchos.