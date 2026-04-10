# XPO SAP HANA Provider

> A custom data store provider for **DevExpress XPO** with **SAP HANA** support, built on top of SAP's official ADO.NET driver.

🇧🇷 [Versão em português disponível aqui](README-ptBR.md)

---

## Description and Motivation

[DevExpress XPO](https://www.devexpress.com/Products/NET/ORM/) is a high-performance .NET ORM that natively supports several databases (SQL Server, Oracle, MySQL, PostgreSQL, SQLite, etc.), but **does not include native SAP HANA support**.

This project implements a **custom provider** using XPO's `ConnectionProviderSql` extensibility API and SAP's official ADO.NET driver (`Sap.Data.Hana.Core.v2.1`), enabling SAP HANA to be used as a persistence backend in .NET applications that already use (or want to use) XPO as their ORM.

---

## Requirements

| Component | Minimum version |
|---|---|
| .NET SDK | 6.0+ |
| DevExpress XPO | 23.2.4+ (license required) |
| SAP HANA Client (ADO.NET) | `Sap.Data.Hana.Core.v2.1` 2.17.22+ |
| SAP HANA Server | 2.0+ |

> ⚠️ **DevExpress License:** The `DevExpress.Xpo` package requires a valid DevExpress license. See [devexpress.com](https://www.devexpress.com) for more information.
>
> ⚠️ **SAP HANA Driver:** The `Sap.Data.Hana.Core.v2.1` package is available on the official NuGet feed. Make sure the SAP HANA Client is installed in your runtime environment.

---

## Installation

### 1. Add NuGet packages to your project

```xml
<ItemGroup>
  <PackageReference Include="DevExpress.Xpo" Version="23.2.4" />
  <PackageReference Include="Sap.Data.Hana.Core.v2.1" Version="2.17.22" />
</ItemGroup>
```

### 2. Reference the `XpoSapHana` project

```xml
<ItemGroup>
  <ProjectReference Include="path/to/src/XpoSapHana/XpoSapHana.csproj" />
</ItemGroup>
```

---

## Quick Start

```csharp
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using XpoSapHana;

// 1. Ensure the assembly is loaded (the provider registers itself automatically)
_ = typeof(HanaConnectionProvider);

// 2. Build the connection string
var connectionString = HanaDataStoreFactory.CreateConnectionString(
    host: "hana-server",
    port: 30015,
    user: "SYSTEM",
    password: "YourPassword123"
);

// 3. Create the DataLayer
var dataStore = HanaDataStoreFactory.Create(connectionString, AutoCreateOption.DatabaseAndSchema);
XpoDefault.DataLayer = XpoDefault.GetDataLayer(dataStore, AutoCreateOption.DatabaseAndSchema);

// 4. CRUD with UnitOfWork
using var uow = new UnitOfWork();
var customer = new Customer(uow) { Name = "John Smith", Email = "john@example.com" };
await uow.CommitChangesAsync();

// 5. Query
var customers = uow.Query<Customer>().ToList();
```

---

## XPO → SAP HANA Type Mapping

| XPO `DBColumnType` | SAP HANA Type |
|---|---|
| `Boolean` | `BOOLEAN` |
| `Byte` | `TINYINT` |
| `SByte` | `SMALLINT` |
| `Short` | `SMALLINT` |
| `UShort` | `INTEGER` |
| `Int32` | `INTEGER` |
| `UInt32` | `BIGINT` |
| `Int64` | `BIGINT` |
| `UInt64` | `DECIMAL(20,0)` |
| `Single` | `FLOAT` |
| `Double` | `DOUBLE` |
| `Decimal` | `DECIMAL(28,4)` |
| `DateTime` | `TIMESTAMP` |
| `Guid` | `NVARCHAR(36)` |
| `String` (≤5000) | `NVARCHAR(N)` |
| `String` (>5000 or no size) | `NCLOB` |
| `ByteArray` | `BLOB` |
| `Char` | `NVARCHAR(1)` |

---

## Registering the Provider

The provider registers itself **automatically** when the `XpoSapHana` assembly is loaded, thanks to the static constructor of `HanaConnectionProvider`:

```csharp
static HanaConnectionProvider()
{
    DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
}
```

To ensure the assembly is loaded before using XPO, include an explicit type reference:

```csharp
_ = typeof(HanaConnectionProvider); // forces the assembly to load
```

After registration you can also use the provider string token directly:

```csharp
// Format: XpoProvider=SapHana;Server=host:port;UserName=user;Password=pass;
var cs = $"XpoProvider={HanaConnectionProvider.XpoProviderTypeString};{rawConnectionString}";
var dataStore = XpoDefault.GetConnectionProvider(cs, AutoCreateOption.DatabaseAndSchema);
```

---

## Project Structure

```
xpo-sap-hana-provider/
├── src/
│   └── XpoSapHana/
│       ├── XpoSapHana.csproj
│       ├── HanaConnectionProvider.cs   ← main provider (inherits ConnectionProviderSql)
│       ├── HanaSqlGenerator.cs         ← HANA DDL/DML generator
│       ├── HanaSchemaProvider.cs       ← schema reader via SYS.TABLES
│       └── HanaDataStoreFactory.cs     ← convenience factory
├── samples/
│   └── BasicSample/
│       ├── BasicSample.csproj
│       └── Program.cs                  ← working CRUD example
├── tests/
│   └── XpoSapHana.Tests/
│       ├── XpoSapHana.Tests.csproj
│       └── HanaConnectionProviderTests.cs
├── xpo-sap-hana-provider.sln
└── README.md
```

---

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a branch for your feature: `git checkout -b feature/my-feature`
3. Make atomic and descriptive commits
4. Open a Pull Request describing the changes

### Guidelines

- Maintain compatibility with .NET 6+
- Add unit tests for new behaviors
- Use XML comments (`///`) on all public members
- Use double-quoted identifiers in all generated SQL (e.g. `"TableName"`)

---

## License

MIT License — see [LICENSE](LICENSE) for details.

```
MIT License

Copyright (c) 2026 khalilpdev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
