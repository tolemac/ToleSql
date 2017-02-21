#!/usr/bin/env bash
dotnet restore && dotnet build src/**/project.json -f "netstandard1.6" && dotnet build test/**/project.json && dotnet test test/ToleSql.Tests