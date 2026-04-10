# XPO SAP HANA Provider

> Provedor de armazenamento de dados personalizado para **DevExpress XPO** com suporte ao **SAP HANA**, construído sobre o driver ADO.NET oficial da SAP.

🇺🇸 [English version available here](README.md)

---

## Descrição e Motivação

O [DevExpress XPO](https://www.devexpress.com/Products/NET/ORM/) é um ORM .NET de alta performance que suporta nativamente diversos bancos de dados (SQL Server, Oracle, MySQL, PostgreSQL, SQLite, etc.), mas **não inclui suporte nativo ao SAP HANA**.

Este projeto implementa um **provedor customizado** utilizando a API de extensibilidade `ConnectionProviderSql` do XPO e o driver oficial ADO.NET da SAP (`Sap.Data.Hana.Core.v2.1`), permitindo utilizar o SAP HANA como backend de persistência em aplicações .NET que já utilizam (ou desejam utilizar) o XPO como ORM.

---

## Requisitos

| Componente | Versão mínima |
|---|---|
| .NET SDK | 6.0+ |
| DevExpress XPO | 23.2.4+ (requer licença) |
| SAP HANA Client (ADO.NET) | `Sap.Data.Hana.Core.v2.1` 2.17.22+ |
| SAP HANA Server | 2.0+ |

> ⚠️ **Licença DevExpress:** O pacote `DevExpress.Xpo` requer uma licença válida da DevExpress. Consulte [devexpress.com](https://www.devexpress.com) para mais informações.
>
> ⚠️ **Driver SAP HANA:** O pacote `Sap.Data.Hana.Core.v2.1` está disponível no NuGet oficial. Certifique-se de que o SAP HANA Client esteja instalado no ambiente de execução.

---

## Instalação

### 1. Adicionar pacotes NuGet ao seu projeto

```xml
<ItemGroup>
  <PackageReference Include="DevExpress.Xpo" Version="23.2.4" />
  <PackageReference Include="Sap.Data.Hana.Core.v2.1" Version="2.17.22" />
</ItemGroup>
```

### 2. Referenciar o projeto `XpoSapHana`

```xml
<ItemGroup>
  <ProjectReference Include="path/to/src/XpoSapHana/XpoSapHana.csproj" />
</ItemGroup>
```

---

## Início Rápido

```csharp
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using XpoSapHana;

// 1. Garante que o assembly seja carregado (o provider é registrado automaticamente)
_ = typeof(HanaConnectionProvider);

// 2. Cria a connection string
var connectionString = HanaDataStoreFactory.CreateConnectionString(
    host: "hana-server",
    port: 30015,
    user: "SYSTEM",
    password: "YourPassword123"
);

// 3. Cria o DataLayer
var dataStore = HanaDataStoreFactory.Create(connectionString, AutoCreateOption.DatabaseAndSchema);
XpoDefault.DataLayer = XpoDefault.GetDataLayer(dataStore, AutoCreateOption.DatabaseAndSchema);

// 4. CRUD com UnitOfWork
using var uow = new UnitOfWork();
var customer = new Customer(uow) { Name = "João Silva", Email = "joao@example.com" };
await uow.CommitChangesAsync();

// 5. Consulta
var customers = uow.Query<Customer>().ToList();
```

---

## Mapeamento de Tipos XPO → SAP HANA

| XPO `DBColumnType` | Tipo SAP HANA |
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
| `String` (>5000 ou sem tamanho) | `NCLOB` |
| `ByteArray` | `BLOB` |
| `Char` | `NVARCHAR(1)` |

---

## Como Registrar o Provider

O provider é registrado **automaticamente** quando o assembly `XpoSapHana` é carregado, graças ao construtor estático de `HanaConnectionProvider`:

```csharp
static HanaConnectionProvider()
{
    DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
}
```

Para garantir que o assembly seja carregado antes de usar o XPO, inclua uma referência explícita ao tipo:

```csharp
_ = typeof(HanaConnectionProvider); // força o carregamento do assembly
```

Após o registro, você também pode usar o token de provider string diretamente:

```csharp
// Formato: XpoProvider=SapHana;Server=host:port;UserName=user;Password=pass;
var cs = $"XpoProvider={HanaConnectionProvider.XpoProviderTypeString};{rawConnectionString}";
var dataStore = XpoDefault.GetConnectionProvider(cs, AutoCreateOption.DatabaseAndSchema);
```

---

## Estrutura do Projeto

```
xpo-sap-hana-provider/
├── src/
│   └── XpoSapHana/
│       ├── XpoSapHana.csproj
│       ├── HanaConnectionProvider.cs   ← provider principal (herda ConnectionProviderSql)
│       ├── HanaSqlGenerator.cs         ← gerador de DDL/DML HANA
│       ├── HanaSchemaProvider.cs       ← leitura de schema via SYS.TABLES
│       └── HanaDataStoreFactory.cs     ← factory de conveniência
├── samples/
│   └── BasicSample/
│       ├── BasicSample.csproj
│       └── Program.cs                  ← exemplo funcional com CRUD
├── tests/
│   └── XpoSapHana.Tests/
│       ├── XpoSapHana.Tests.csproj
│       └── HanaConnectionProviderTests.cs
├── xpo-sap-hana-provider.sln
└── README.md
```

---

## Contribuição

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do repositório
2. Crie uma branch para sua feature: `git checkout -b feature/minha-feature`
3. Faça commits atômicos e descritivos
4. Abra um Pull Request descrevendo as mudanças

### Diretrizes

- Mantenha a compatibilidade com .NET 6+
- Adicione testes unitários para novos comportamentos
- Use comentários XML (`///`) em todos os membros públicos
- Use identificadores com aspas duplas em todo SQL gerado (ex: `"NomeTabela"`)

---

## Licença

MIT License — veja [LICENSE](LICENSE) para detalhes.

```
MIT License

Copyright (c) 2024 khalilpdev

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
