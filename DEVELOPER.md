Building and Testing ToleSql

## Prerequisites.

You need at least .Net Core 1.1.0.

## Getting the sources

[Fork](http://help.github.com/forking) and clone [ToleSql](https://github.com/tolemac/ToleSql) repository.

```` shell
# Clone your GitHub repository:
git clone https://github.com/tolemac/ToleSql.git

# Go to the ToleSql directory:
cd ToleSql
````

## Restore dependencies

```` shell
dotnet restore
````

## Building

```` shell
./build
````

## Running tests

```` shell
./test
````