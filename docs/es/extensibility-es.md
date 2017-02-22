# Extensibilidad de funciones SQL

ToleSql se puede extender para que admita nuevas funciones, tiene un sistema `sencillo` para poder incorporar nuevas funciones SQL y generar SQL según métodos usados en las expresiones.

ToleSql usa un [ExpressionVisitor](https://msdn.microsoft.com/en-us/library/bb882521(v=vs.90).aspx) para recorrer las expresiones pasadas, cuando detecta la llamada a una función llama a una lista de *interceptores* para intentar procesar dicha llamada, si no es interceptada por ningún interceptor intenta resolverla evaluando la expresión y generando una constante, por lo que si en la llamada se usa un miembro de un parametro el proceso peta.

La siguiente expresión usada en la llamada al método `Select` provoca la llamada a los interceptores:

```` csharp
builder.Select<Invoice>(i => new { 
    Supplier = i.SupplierId, 
    MaxAmount = DbFunctions.Max(i.TotalAmount) 
});
````

Y de eso se deberá generar algo como:

```` sql
SELECT [T0].[SupplierId] as Supplier, MAX([T0].[TotalAmount]) AS MaxAmount From [Invoice]
````

En definitiva es traducir `DbFunctions.Max(i.TotalAmount)` en `MAX([T0].[TotalAmount])`

A continuación mostramos el interceptor usado para la función `MAX` de SQL. Cuando ToleSql se encuentra con la llamada al metodo `DbFunctions.Max` va llamando a los interceptores hasta que alguno devuelve distinto de nulo.

```` csharp
public class DbFunctionsMax : MethodCallInterceptorBase
{
    public override bool Intercept(MethodCallExpression m, 
        StringBuilder sql, Func<Expression, Expression> visit) // (1) parametros de entrada
    {
        if (m.Method.DeclaringType == typeof(DbFunctions) 
            && m.Method.Name == nameof(DbFunctions.Max)) // (2) checkeo del método.
        {
            var identifier = m.Arguments[0]; // (3) Aceso al identificador
            sql.Append($"{Dialect.Keyword(SqlKeyword.Max)}"); // (4) Escritura del literal 'MAX'
            sql.Append($"{Dialect.Symbol(SqlSymbols.StartGroup)}"); // (4a)
            visit(identifier); // (5) Procesado del identificador.
            sql.Append($"{Dialect.Symbol(SqlSymbols.EndGroup)}"); // (4b)
            return true; // (6) Indicamos que hemos capturado la expresión.
        }
        // Si no pasa el chequeo devolvemos false para indicar que no hemos hecho nada.
        return false; 

    }
}
````

1. Parametros de entrada del interceptor.
* Recibe la expresión de llamada al metodo. Esta expresión contiene el metodo al que se llama y los argumentos que se le pasarán. De aquí podemos acceder al tipo donde está declarado el método y el nombre del mismo.
* El string builder donde se va añadiendo la consulta SQL. Cuando tengamos que escribir SQL lo haremos con `sql.Append()`.
* Un método para procesar expresiones, los argumentos usados en la expresión de llamada al metodo pueden ser constantes, parametros, llamadas a funciones, ... 
2. Chequeo del método.  
Lo primero que hacemos es comprobar que se está llamando al método que `DbFunctions.Max()`, para eso comprobamos el tipo donde está declarado el método `Method.DeclaringType` y el nombre del mismo `Method.Name`, si coinciden con `DbFunctions` y `Max` entonces procesamos, si no devolvemos false para indicar a ToleSql que siga buscando interceptores para esta expresión.
3. Acceso al identificador.  
Sabemos que nuestra función `Max` solo acepta un parametro, así que accedemos a él mediante la expresión. Al ser una `MethodCallExpression` tiene una propiedad llamada `Arguments` que es la lista de argumentos de la función, en este caso cogemos el primero y único.
4. Escritura de literales.  
Ahora lo que hacemos es escribir en SQL el literal `MAX`, en este caso lo que se hace es pedirlo al dialecto activo, usamos `Dialect.Keyword(SqlKeyword.Max)`, `Keyword` es un método del dialecto que dada una plabra clave devuelve un literal, en este caso devolverá `MAX` a no ser que usemos un dialecto para una base de datos donde no se llame así.  
Acto seguido escribimos una apertura de parentesis `(`, luego pondremos el identificador y por último (4c) cerraremos el parentesis. La apertura de parentesis se realiza también mediante el dialecto, en este caso le solicitamos el simbolo `StartGroup` o `EndGroup` para cerrar: `Dialect.Symbol(SqlSymbols.StartGroup)` o `Dialect.Symbol(SqlSymbols.EndGroup)`.
5. Procesado del identificador.  
El identificador puede ser otra expresión compleja, si estuvieramos seguros de que es un miembro de un parametro podríamos coger su nombre, pero como puede ser cualquier cosa la procesamos con el visitor. En este ejemplo la expresión es `DbFunctions.Max(i.TotalAmount)`, pero podría ser `DbFunctions.Max(i.Count * i.Price * 0.20)`, como ToleSql ya sabe procesar expresiones mejor lo invocamos con `visit(identifier)`.
6. Devolvemos `true`, de esa forma indicamos a ToleSql que no siga llamando interceptores para esta expresión.

En [/ToleSql/Expressions/Visitors/Interceptors](../../src/ToleSql/Expressions/Visitors/Interceptors) podemos ver la lista de interceptores disponibles.

Una vez que hemos construido nuestro interceptor debemos añadirlo a la configuración de ToleSql para que lo tenga en cuenta:

```` csharp
Configuration.RegisterInterceptor(new DbFunctionsMax());
````

Actualmente se añaden los interceptores en el constructor estático de la clase [`Configuration`](../../src/ToleSql/Configuration.cs).
