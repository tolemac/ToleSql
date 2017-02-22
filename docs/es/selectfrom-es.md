# SelectFrom y LinQ

Con `SelectFrom` puedes construir consultas a partir de un tipo concreto e incluyendo otros tipos haciendo uso del método `Join`. `SelectFrom` además acepta LinQ query syntax.

`SelectFrom` hace uso internamente de [`SelectBuilder`](./selectbuilder-es.md) así que viene bien que sepas como funciona.

### Select de una tabla

```` csharp
var customers = new SelectFrom<Customer>();
````

Una vez que tenemos este objeto podemos llamar a sus métodos para seleccionar columnas:

```` csharp
customers.Select(c => new { c.Name, c.Balance } );
````

Ordenar:

```` csharp
customers.OrderBy(c => c.Balance).ThenByDescending(c => c.Date);
````

Agrupar:

```` csharp
customers.GroupBy(c => c.ContactCustomerId);
````

Filtrar:
```` csharp
customers.Where(c => c.Balance > 1000);
````

### Añadiendo tipos a la consultas

Podemos añadir otras tablas haciendo `Join` con otros tipos:

```` csharp
var query = new SelectFrom<DeliveryNote>();
query
    .Select(dn => new { dn.TotalAmount, dn.SupplierId } );
    .Join<Supplier>((dn, s) => dn.SupplierId == s.Id);
````

Nuestro objeto query es del tipo `SelectFrom<TEntity>`, pero cuando hacemos un `Join` el resultado es un `SelectFrom<TEntity, TJoinedEntity>`. El segundo hereda del primero, así que una vez que hacemos el join podemos invocar a metodos `Select`, `OrderBy`, `GroupBy`, ... con dos parametros, tal que así:

```` csharp
var query = new SelectFrom<DeliveryNote>();
query
    .Select(dn => new { dn.TotalAmount, dn.SupplierId } );
    .Join<Supplier>((dn, s) => dn.SupplierId == s.Id)
    .Select((dn, s) => new { Supplier = dn.SupplierId + s.Name });    
````

Si volvemos a hacer `Join` el resultado es de nuevo `SelectFrom<TEntity, TJoinedEntity>` por lo tanto si hacemos un nuevo `Join` con `User` el resultado es un `SelectFrom<Supplier, User>`, se va encadenando la nueva tabla con la última que unió.

```` csharp
var query = new SelectFrom<DeliveryNote>();
query
    .Select(dn => new { dn.TotalAmount, dn.SupplierId } );
    .Join<Supplier>((dn, s) => dn.SupplierId == s.Id)
    .Join<User>((s, u) => s.CreatedBy == u.Id)
    .Select((s, u) => new { 
        UserName = u.Name, 
        SupplierName = s.Name 
    });
````

### Usando sintaxis LinQ

Si te gusta puedes usar la sintaxis LinQ, por ejemplo:

```` csharp
var b = from d in new SelectFrom<DeliveryNote>()
        select d;
````

Usar `where`:
```` csharp
var b = from d in new SelectFrom<DeliveryNote>()
        where d.TotalAmount > 100 && d.Year == "2017"
        select d;
````

Proyecciones con agrupaciones:

```` csharp
var b = from d in new SelectFrom<DeliveryNote>()
        group d by new { d.SupplierId, d.Year } into g
        select new { 
            Supplier = g.SupplierId, 
            Year = g.Year, 
            TotalAmount = DbFunctions.Sum(g.TotalAmount) 
        };
````

Enlazar tipos con `join`:

```` csharp
var b = from d in new SelectFrom<DeliveryNote>()
        join s in new SelectFrom<Supplier>() on d.SupplierId equals s.Id
        select new { d, s };
````

Todavía no se soportan los `join` con multiclave.

`SelectFrom` puede venir bien para consultas rápidas ya que es muy comodo enlazar llamadas a métodos (fluently).