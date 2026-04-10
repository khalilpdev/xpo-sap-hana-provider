# 📦 Publicando o XpoSapHana no NuGet

Este guia descreve o passo a passo completo para empacotar e publicar o pacote **XpoSapHana** no [NuGet.org](https://www.nuget.org/), tornando-o disponível para instalação via `dotnet add package` ou pelo gerenciador de pacotes do Visual Studio.

---

## ✅ Pré-requisitos

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download) instalado
- Conta criada em [nuget.org](https://www.nuget.org/)
- API Key gerada no NuGet.org
- CLI do NuGet ou `dotnet` CLI

---

## 🔧 Etapa 1 — Configurar os metadados do pacote no `.csproj`

Edite o arquivo `src/XpoSapHana/XpoSapHana.csproj` e adicione/complete as propriedades de metadados dentro do `<PropertyGroup>`:

```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
  <RootNamespace>XpoSapHana</RootNamespace>
  <AssemblyName>XpoSapHana</AssemblyName>

  <!-- Identidade do pacote -->
  <PackageId>XpoSapHana</PackageId>
  <Version>1.0.0</Version>
  <Authors>khalilpdev</Authors>
  <Company>khalilpdev</Company>
  <Product>XpoSapHana</Product>

  <!-- Descrição e metadados -->
  <Description>DevExpress XPO custom data store provider for SAP HANA</Description>
  <Summary>Provedor de dados SAP HANA para o ORM DevExpress XPO.</Summary>
  <PackageTags>devexpress;xpo;sap;hana;orm;provider</PackageTags>

  <!-- Repositório e licença -->
  <RepositoryUrl>https://github.com/khalilpdev/xpo-sap-hana-provider</RepositoryUrl>
  <RepositoryType>git</RepositoryType>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <PackageReadmeFile>README.md</PackageReadmeFile>
  <PackageProjectUrl>https://github.com/khalilpdev/xpo-sap-hana-provider</PackageProjectUrl>

  <!-- Ativar geração do pacote no build -->
  <GeneratePackageOnBuild>false</GeneratePackageOnBuild>
</PropertyGroup>

<!-- Incluir o README no pacote -->
<ItemGroup>
  <None Include="..\..\README.md" Pack="true" PackagePath="\" />
</ItemGroup>
```

> 💡 Ajuste o campo `<Version>` a cada nova publicação seguindo o padrão [SemVer](https://semver.org/lang/pt-BR/) (ex: `1.0.0`, `1.1.0`, `2.0.0`).

---

## 📦 Etapa 2 — Gerar o pacote `.nupkg`

Na raiz do repositório, execute:

```bash
dotnet pack src/XpoSapHana/XpoSapHana.csproj --configuration Release --output ./nupkgs
```

O arquivo `.nupkg` será gerado na pasta `./nupkgs`, por exemplo:
```
nupkgs/XpoSapHana.1.0.0.nupkg
```

---

## 🔑 Etapa 3 — Obter a API Key do NuGet.org

1. Acesse [https://www.nuget.org/account/apikeys](https://www.nuget.org/account/apikeys)
2. Clique em **Create**
3. Defina um nome para a chave (ex: `xpo-sap-hana-publish`)
4. Em **Glob Pattern**, coloque `XpoSapHana*` para restringir ao seu pacote
5. Marque a permissão **Push** (e **Push new packages and package versions**)
6. Clique em **Create** e **copie a chave gerada** — ela só é exibida uma vez

---

## 🚀 Etapa 4 — Publicar no NuGet.org

Execute o comando abaixo substituindo `<SUA_API_KEY>` pela chave obtida:

```bash
dotnet nuget push ./nupkgs/XpoSapHana.1.0.0.nupkg \
  --api-key <SUA_API_KEY> \
  --source https://api.nuget.org/v3/index.json
```

Ou usando a CLI do NuGet:

```bash
nuget push ./nupkgs/XpoSapHana.1.0.0.nupkg <SUA_API_KEY> -Source https://api.nuget.org/v3/index.json
```

Após o envio, o pacote ficará disponível em:
```
https://www.nuget.org/packages/XpoSapHana
```

> ⏱ A indexação pode levar alguns minutos.

---

## 🤖 Etapa 5 (Opcional) — Publicar automaticamente via GitHub Actions

Crie o arquivo `.github/workflows/publish-nuget.yml` para publicar automaticamente ao criar uma tag de versão:

```yaml
name: Publish NuGet Package

on:
  push:
    tags:
      - "v*.*.*"

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "6.0.x"

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release --no-restore

      - name: Pack
        run: dotnet pack src/XpoSapHana/XpoSapHana.csproj --configuration Release --no-build --output ./nupkgs

      - name: Push to NuGet.org
        run: dotnet nuget push ./nupkgs/*.nupkg --api-key ${{ secrets.NUGET_API_KEY }} --source https://api.nuget.org/v3/index.json --skip-duplicate
```

### Configurando o secret `NUGET_API_KEY`

1. Acesse **Settings** → **Secrets and variables** → **Actions** no seu repositório
2. Clique em **New repository secret**
3. Nome: `NUGET_API_KEY`
4. Valor: sua API Key do NuGet.org
5. Clique em **Add secret**

Para acionar a publicação, crie e envie uma tag:

```bash
git tag v1.0.0
git push origin v1.0.0
```

---

## 📥 Como instalar o pacote após publicação

Via .NET CLI:

```bash
dotnet add package XpoSapHana --version 1.0.0
```

Via Package Manager Console (Visual Studio):

```powershell
Install-Package XpoSapHana -Version 1.0.0
```

Via `PackageReference` no `.csproj`:

```xml
<PackageReference Include="XpoSapHana" Version="1.0.0" />
```

---

## 🔄 Publicando uma nova versão

1. Atualize o `<Version>` no `XpoSapHana.csproj`
2. Execute novamente os passos de **pack** e **push** (ou crie uma nova tag se usar o workflow de CI/CD)

---

## 📚 Referências

- [Documentação oficial do NuGet](https://docs.microsoft.com/nuget/)
- [Criando pacotes NuGet com a .NET CLI](https://docs.microsoft.com/nuget/create-packages/creating-a-package-dotnet-cli)
- [nuget.org — Publicação de pacotes](https://docs.microsoft.com/nuget/nuget-org/publish-a-package)
- [GitHub Actions para .NET](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)