# Funciones especiales de SQL

* `string.Contains(parameter)` - `LIKE %parameter%`  
Para buscar por contenido: `builder.Where(p => p.Name.Contains("Javi"));` o al contrario `builder.Where(p => "Javier Ros".Contains(p.Name));`
* `string.StartsWith(parameter)` - `LIKE parameter%`  
Para buscar texto que empieza por algo: `builder.Where(p => p.Name.StartsWith("Javi"));` o al contrario `builder.Where(p => "Javier Ros".StartsWith(p.Name));`
* `string.EndsWith(parameter)` - `LIKE %parameter`  
Para buscar texto que termina con otro texto: `builder.Where(p => p.Name.EndsWith("Javi"));` o al contrario `builder.Where(p => "Javier Ros".EndsWith(p.Name));`
* `string.SubString(from, count)` - `SUBSTRING(parameter, from, count)`  
Para extraer una subcadena de longitud `count` desde el indice `from`.
`builder.Select(p => pName.SubString(0, 3));`
* `DbFunctions.Sum(param)` - `SUM(param)`  
Para mostrar sumas en las agrupaciones: `builder.Select(p => DbFunctions.Sum(p.Amount))`
* `DbFunctions.Sum(param)` - `COUNT(param)`
* `DbFunctions.Max(param)` - `MAX(param)`
* `DbFunctions.Min(param)` - `MIN(param)`


Para dar soporte a nuevas funciones mirar la sección [Extensibilidad](./extensibility-es.md)