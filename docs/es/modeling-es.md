# Modelado

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