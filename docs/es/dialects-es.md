# Dialectos

ToleSql soporta distintos dialectos. ToleSql no genera ningún literal ni palabra clave en las consultas SQL, todo literal lo extrae del dialecto activo.

ToleSql soporta sintaxis explicita o implicita para realizar los JOIN entre tablas, la forma explicita usa la palabra clave `JOIN` y `ON` y la implicita separa los origenes de datos por comas en la cláusula `FROM`.

ToleSql viene con un solo dialecto, para SqlServer y es el que se establece por defecto. El dialecto SqlServer usa `explicit joins`.

Se pueden construir nuevos dialectos mediante la implementación de la interface `ToleSql.Dialect.IDialect` o heredando de `ToleSql.Dialect.DialectBase`. Ya sea incorporandolo al proyecto mediante un pull request o de forma externa.

Como ejemplo puedes echar un vistazo al [dialecto de SqlServer](../../src/ToleSql/SqlServer/SqlServerDialect.cs)