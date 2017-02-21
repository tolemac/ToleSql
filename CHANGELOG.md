# Version 1.5.0-beta2 (WIP)

* refactor: generic methods refactorized [fae094a](https://github.com/tolemac/ToleSql/commit/fae094a18af0f27c4e642b15c57bce0363d42c30)
* docs: update docs with new features [b1eb556](https://github.com/tolemac/ToleSql/commit/b1eb5563360abd37afedfee64c0ab7341a1b653f)
* test: add travis continuous integration [199fdb5](https://github.com/tolemac/ToleSql/commit/199fdb50ee31b7e7fa0b791be2128528d56c7192)
* feat: modeling by builder and default schema [746f59d](https://github.com/tolemac/ToleSql/commit/746f59d4bfd2908db8dc84da1acc660f120fcee2)
* refactor: interceptor returns a bool value. [6b8cb31](https://github.com/tolemac/ToleSql/commit/6b8cb31d2954e1276b8449e6cf5102e8e4dda609)
* fix: substring function is 1 based. [a2503b7](https://github.com/tolemac/ToleSql/commit/a2503b7dea8822e04ca44170e62c9a1efa1c3857)
* docs: add contributing and developer markdown files [ad04ee4](https://github.com/tolemac/ToleSql/commit/ad04ee41f20756a0d307f7bcc15ac8e406b8522a)
* docs: add issue template [fcdfde7](https://github.com/tolemac/ToleSql/commit/fcdfde7f457bf1ae29d67a3c55ea39f75bbe44df)
* docs: add initial spanish documentation [ea49042](https://github.com/tolemac/ToleSql/commit/ea490423e4fe7f740d6787c4931a61e44da263a3)
* refactor: remove `add` prefix from methods [4572ce4](https://github.com/tolemac/ToleSql/commit/4572ce4464c0f1e0154083e4d82f5299fbb84933)
* fix: Remove test project from `InternalsVisibleTo` [8eed20c](https://github.com/tolemac/ToleSql/commit/8eed20cb07775060e4390c7557a9a7e3129b64c8)
* build: Add test build to build.bat [63b57eb](https://github.com/tolemac/ToleSql/commit/63b57eb73f7ee0d4434c37b4d9b5cfc26cf69004)
* feat: add SelectFrom<TEntity, TJoinedEntity> and double generics methods [e8e4019](https://github.com/tolemac/ToleSql/commit/e8e4019fb449bd3ac1a481a55f238f034e1c09c3)
* feat: LinQ query syntax is enable with SelectForm [ed034de](https://github.com/tolemac/ToleSql/commit/ed034dee64342893e0cb39850ae918fe8e00bb3d)
* feat: Add groupby and having. Add sum, max, min and count group functions [ed3f57e](https://github.com/tolemac/ToleSql/commit/ed3f57e88627f8d4ecccc6a0d29eab47b12a791d)

## Breaking Changes

* Interceptors return a bool value instead expression.

# Version 1.5.0-beta1 (2017-02-20)

## Code refactoring
* core: unificar clases bajo el namespace `ToleSql` para no tener que hacer tantos `using` [#1](https://github.com/tolemac/ToleSql/issues/1)

## Breaking Changes
* core: las clases cambian de nombre y se reubican en el namespace `ToleSql`

# Version 1.0.2 (2017-02-19)
Versión inicial en desarrollo.
Era el primer paquete que subía a NuGet y la cagué con el número de versión, luego no pude solucionarlo ...
