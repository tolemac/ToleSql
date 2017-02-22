> Se aceptan issues en español.

# ToleSql

[![Build Status](https://travis-ci.org/tolemac/ToleSql.svg)](https://travis-ci.org/tolemac/ToleSql)
![#](https://img.shields.io/nuget/vpre/tolesql.svg?style=flat)
![#](https://img.shields.io/nuget/v/tolesql.svg?style=flat)

ToleSql es generador de SQL para proyectos .NET.
Construye consultas type-safe y refactor friendly para usarlas con ADO.NET o Dapper.

## ¿Como se usa?
Acceda a la [Documentación](./docs/es/home-es.md)

## Instalación via NuGet

Está disponible [en NuGet](https://www.nuget.org/packages/ToleSql/)

````
Install-Package ToleSql
````

## Uso

Añadir using:
```` csharp
using ToleSql;
````

Crear el primer select:

```` csharp
var b = new SelectFrom<DeliveryNote>().Where(d => d.Year == "2017");
var gen = b.GetSqlText();

var spec = "SELECT * FROM [DeliveryNote] AS [T0] WHERE [T0].[Year] = @SqlParam0";
Assert.Equal(spec, gen);
Assert.Equal(b.Builder.Parameters[0], "2017");
````
