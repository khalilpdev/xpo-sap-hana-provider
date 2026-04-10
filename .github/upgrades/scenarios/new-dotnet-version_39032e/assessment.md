# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [samples\BasicSample\BasicSample.csproj](#samplesbasicsamplebasicsamplecsproj)
  - [src\XpoSapHana\XpoSapHana.csproj](#srcxposaphanaxposaphanacsproj)
  - [tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 3 | All require upgrade |
| Total NuGet Packages | 6 | All compatible |
| Total Code Files | 6 |  |
| Total Code Files with Incidents | 3 |  |
| Total Lines of Code | 798 |  |
| Total Number of Issues | 3 |  |
| Estimated LOC to modify | 0+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [samples\BasicSample\BasicSample.csproj](#samplesbasicsamplebasicsamplecsproj) | net6.0 | 🟢 Low | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [src\XpoSapHana\XpoSapHana.csproj](#srcxposaphanaxposaphanacsproj) | net6.0 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj) | net6.0 | 🟢 Low | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 6 | 100.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 0 | 0.0% |
| ***Total NuGet Packages*** | ***6*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 804 |  |
| ***Total APIs Analyzed*** | ***804*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| DevExpress.Xpo | 23.2.4 |  | [BasicSample.csproj](#samplesbasicsamplebasicsamplecsproj)<br/>[XpoSapHana.csproj](#srcxposaphanaxposaphanacsproj) | ✅Compatible |
| Microsoft.NET.Test.Sdk | 17.8.0 |  | [XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj) | ✅Compatible |
| Moq | 4.20.70 |  | [XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj) | ✅Compatible |
| Sap.Data.Hana.Core.v2.1 | 2.17.22 |  | [XpoSapHana.csproj](#srcxposaphanaxposaphanacsproj) | ✅Compatible |
| xunit | 2.6.2 |  | [XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj) | ✅Compatible |
| xunit.runner.visualstudio | 2.5.4 |  | [XpoSapHana.Tests.csproj](#testsxposaphanatestsxposaphanatestscsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;XpoSapHana.csproj</b><br/><small>net6.0</small>"]
    P2["<b>📦&nbsp;BasicSample.csproj</b><br/><small>net6.0</small>"]
    P3["<b>📦&nbsp;XpoSapHana.Tests.csproj</b><br/><small>net6.0</small>"]
    P2 --> P1
    P3 --> P1
    click P1 "#srcxposaphanaxposaphanacsproj"
    click P2 "#samplesbasicsamplebasicsamplecsproj"
    click P3 "#testsxposaphanatestsxposaphanatestscsproj"

```

## Project Details

<a id="samplesbasicsamplebasicsamplecsproj"></a>
### samples\BasicSample\BasicSample.csproj

#### Project Info

- **Current Target Framework:** net6.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 1
- **Number of Files with Incidents**: 1
- **Lines of Code**: 149
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["BasicSample.csproj"]
        MAIN["<b>📦&nbsp;BasicSample.csproj</b><br/><small>net6.0</small>"]
        click MAIN "#samplesbasicsamplebasicsamplecsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;XpoSapHana.csproj</b><br/><small>net6.0</small>"]
        click P1 "#srcxposaphanaxposaphanacsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 114 |  |
| ***Total APIs Analyzed*** | ***114*** |  |

<a id="srcxposaphanaxposaphanacsproj"></a>
### src\XpoSapHana\XpoSapHana.csproj

#### Project Info

- **Current Target Framework:** net6.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 2
- **Number of Files**: 4
- **Number of Files with Incidents**: 1
- **Lines of Code**: 442
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P2["<b>📦&nbsp;BasicSample.csproj</b><br/><small>net6.0</small>"]
        P3["<b>📦&nbsp;XpoSapHana.Tests.csproj</b><br/><small>net6.0</small>"]
        click P2 "#samplesbasicsamplebasicsamplecsproj"
        click P3 "#testsxposaphanatestsxposaphanatestscsproj"
    end
    subgraph current["XpoSapHana.csproj"]
        MAIN["<b>📦&nbsp;XpoSapHana.csproj</b><br/><small>net6.0</small>"]
        click MAIN "#srcxposaphanaxposaphanacsproj"
    end
    P2 --> MAIN
    P3 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 477 |  |
| ***Total APIs Analyzed*** | ***477*** |  |

<a id="testsxposaphanatestsxposaphanatestscsproj"></a>
### tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj

#### Project Info

- **Current Target Framework:** net6.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 3
- **Number of Files with Incidents**: 1
- **Lines of Code**: 207
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["XpoSapHana.Tests.csproj"]
        MAIN["<b>📦&nbsp;XpoSapHana.Tests.csproj</b><br/><small>net6.0</small>"]
        click MAIN "#testsxposaphanatestsxposaphanatestscsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;XpoSapHana.csproj</b><br/><small>net6.0</small>"]
        click P1 "#srcxposaphanaxposaphanacsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 213 |  |
| ***Total APIs Analyzed*** | ***213*** |  |

