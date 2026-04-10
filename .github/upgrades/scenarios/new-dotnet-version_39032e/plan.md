# .NET 10 Upgrade Migration Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [XpoSapHana.csproj](#xposaphanacsproj)
  - [BasicSample.csproj](#basicsamplecsproj)
  - [XpoSapHana.Tests.csproj](#xposaphanatestscsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade the **XPO SAP HANA Provider** solution from **.NET 6** to **.NET 10.0 (LTS)**.

### Scope
**3 projects** across the solution require framework upgrade:

| Project | Type | Current | Target | LOC |
|---------|------|---------|--------|-----|
| src\XpoSapHana\XpoSapHana.csproj | Class Library | net6.0 | net10.0 | 442 |
| samples\BasicSample\BasicSample.csproj | Console App | net6.0 | net10.0 | 149 |
| tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj | Test Project | net6.0 | net10.0 | 207 |

**Total codebase:** 798 lines of code across 6 files.

### Target State
All projects targeting **net10.0** with:
- All builds passing without errors or warnings
- All existing tests passing
- No package compatibility issues
- No API compatibility issues

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale:**
- ✅ Small solution (3 projects)
- ✅ Simple dependency structure (single dependency layer)
- ✅ All currently on .NET 6.0
- ✅ All 6 NuGet packages confirmed compatible with .NET 10
- ✅ No API compatibility issues detected (804 APIs analyzed, 0 incompatible)
- ✅ No security vulnerabilities
- ✅ Low complexity rating for all projects
- ✅ Good test coverage (dedicated test project)

### Complexity Assessment

**Discovered Metrics:**
- Projects: 3
- Dependency depth: 1 (single layer)
- Total packages: 6 (all compatible)
- Total LOC: 798
- API issues: 0 (100% compatible)
- Security vulnerabilities: 0
- Breaking changes: None detected

**Classification: Simple Solution**

This is an ideal candidate for All-at-Once upgrade:
- Minimal complexity
- Clean dependency structure
- No compatibility blockers
- No special migration requirements

### Critical Issues
**None identified.** This is a straightforward framework version upgrade with no blocking issues.

### Recommended Approach
Single atomic operation upgrading all project files simultaneously, followed by build verification and test execution.

### Iteration Strategy
Fast batch approach (3-4 iterations total):
1. ✅ Discovery & Classification (complete)
2. Foundation sections (dependency analysis, strategy, stubs)
3. Detailed project specifications (all projects batched)
4. Final sections (success criteria, source control)

---

## Migration Strategy

### Approach Selection: All-At-Once

**Selected Strategy:** All projects upgraded simultaneously in a single coordinated operation.

**Justification:**

✅ **Solution Size:** 3 projects (well below the 30-project threshold for All-at-Once)

✅ **Framework Homogeneity:** All projects currently on .NET 6.0, upgrading to same target (net10.0)

✅ **Dependency Simplicity:** Single-layer dependency structure with no circular dependencies

✅ **Package Compatibility:** All 6 packages confirmed compatible with .NET 10:
- DevExpress.Xpo 23.2.4 ✅
- Sap.Data.Hana.Core.v2.1 2.17.22 ✅
- Microsoft.NET.Test.Sdk 17.8.0 ✅
- xunit 2.6.2 ✅
- xunit.runner.visualstudio 2.5.4 ✅
- Moq 4.20.70 ✅

✅ **API Compatibility:** 804 APIs analyzed, 0 incompatible (100% compatibility)

✅ **Test Coverage:** Dedicated test project enables immediate validation

✅ **Low Risk:** No security vulnerabilities, no breaking changes, low complexity

### All-at-Once Strategy Rationale

This solution exhibits all the characteristics that make All-at-Once the optimal approach:

**Speed:** Single atomic operation completes upgrade in minimal time

**Simplicity:** No multi-targeting complexity or intermediate states

**Clean Testing:** All projects upgraded together, tested together, deployed together

**Low Coordination Cost:** Small team can review and validate entire changeset at once

**No Staggered Deployment:** All components move to .NET 10 simultaneously

### Dependency-Based Ordering

While All-at-Once updates all projects simultaneously, we acknowledge the dependency structure for context:

**Dependency Order (informational):**
1. **Layer 0 (Leaf):** XpoSapHana.csproj - Core library with no project dependencies
2. **Layer 1 (Consumers):** BasicSample.csproj, XpoSapHana.Tests.csproj - Depend on core library

**Execution Approach:** All projects updated in single pass, not sequentially by layer.

### Parallel vs Sequential Execution

**Parallel Execution (Recommended):**
- All 3 project files updated simultaneously
- All package references remain unchanged (already compatible)
- Single build operation validates all projects together
- Single test run validates entire solution

**Why Not Sequential:**
Sequential (layer-by-layer) would add unnecessary complexity:
- No intermediate compatibility requirements
- No gradual rollout needed
- Additional builds and test runs provide no value
- Extends timeline without reducing risk

### Implementation Timeline

#### Phase 0: Preparation (Validation Only)
**Operations:**
- Verify .NET 10 SDK installed
- Verify current branch (upgrade-to-NET10)
- No global.json present (no updates needed)

**Duration:** < 5 minutes  
**Deliverables:** Environment validated, ready to proceed

#### Phase 1: Atomic Upgrade
**Operations** (performed as single coordinated batch):
- Update all 3 project files: `<TargetFramework>net6.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`
- Restore dependencies (`dotnet restore`)
- Build entire solution
- Fix any compilation errors (none expected based on assessment)
- Rebuild to verify 0 errors

**Deliverables:** 
- All projects targeting net10.0
- Solution builds with 0 errors
- Solution builds with 0 warnings

#### Phase 2: Test Validation
**Operations:**
- Execute all tests in XpoSapHana.Tests.csproj
- Verify all tests pass
- Address any test failures (none expected)

**Deliverables:**
- All tests passing
- No behavioral regressions detected

#### Phase 3: Final Validation & Commit
**Operations:**
- Final build verification
- Review changes
- Commit to upgrade-to-NET10 branch
- Create pull request to main

**Deliverables:**
- Complete, validated upgrade
- Changes ready for review and merge

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a simple, clean dependency structure with a single dependency layer:

```
XpoSapHana.csproj (leaf - no dependencies)
    ↑
    ├── BasicSample.csproj (depends on XpoSapHana)
    └── XpoSapHana.Tests.csproj (depends on XpoSapHana)
```

**Dependency Characteristics:**
- **Depth:** 1 (single layer)
- **Circular dependencies:** None
- **External dependencies:** 6 NuGet packages (all compatible)
- **Leaf node:** XpoSapHana.csproj (0 project dependencies)
- **Root nodes:** BasicSample.csproj, XpoSapHana.Tests.csproj (no dependents)

### Project Groupings by Migration Phase

#### Single Atomic Phase: All Projects Simultaneously

Since this is an All-at-Once strategy with a simple solution, all projects are upgraded together:

**Group 1 (All Projects):**
- src\XpoSapHana\XpoSapHana.csproj - Core library
- samples\BasicSample\BasicSample.csproj - Sample application
- tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj - Test project

**Rationale for simultaneous upgrade:**
- All projects currently on same framework (net6.0)
- No intermediate compatibility requirements
- All packages support net10.0
- Clean dependency graph enables atomic update
- Test project ensures validation immediately after upgrade

### Critical Path Identification

**Primary Path:** XpoSapHana.csproj → (BasicSample.csproj + XpoSapHana.Tests.csproj)

The core library (XpoSapHana.csproj) forms the foundation, but in an All-at-Once approach, we update all projects simultaneously rather than sequentially. The dependency relationship informs our understanding but doesn't create sequential constraints.

### Circular Dependencies

**None detected.** The dependency graph is acyclic and straightforward.

---

## Project-by-Project Plans

All three projects follow the same simple upgrade pattern. Details for each project are provided below.

---

### XpoSapHana.csproj

**Location:** `src\XpoSapHana\XpoSapHana.csproj`

#### Current State
- **Target Framework:** net6.0
- **Project Type:** ClassLibrary (SDK-style)
- **Dependencies:** 2 NuGet packages
  - DevExpress.Xpo 23.2.4
  - Sap.Data.Hana.Core.v2.1 2.17.22
- **Dependents:** 2 projects (BasicSample, XpoSapHana.Tests)
- **Lines of Code:** 442
- **Files:** 4
- **Risk Level:** 🟢 Low

#### Target State
- **Target Framework:** net10.0
- **Package Versions:** No changes required (all compatible)
- **Expected Outcome:** Builds without errors or warnings

#### Migration Steps

**1. Prerequisites**
- ✅ .NET 10 SDK installed
- ✅ DevExpress.Xpo 23.2.4 compatible with net10.0
- ✅ Sap.Data.Hana.Core.v2.1 2.17.22 compatible with net10.0

**2. Framework Update**
Update project file:
```xml
<!-- BEFORE -->
<TargetFramework>net6.0</TargetFramework>

<!-- AFTER -->
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**
**No package updates required.** All packages remain at current versions:

| Package | Current Version | Target Version | Status |
|---------|-----------------|----------------|--------|
| DevExpress.Xpo | 23.2.4 | 23.2.4 | ✅ Compatible |
| Sap.Data.Hana.Core.v2.1 | 2.17.22 | 2.17.22 | ✅ Compatible |

**4. Expected Breaking Changes**
**None.** Assessment analyzed 477 APIs:
- 🔴 Binary incompatible: 0
- 🟡 Source incompatible: 0
- 🔵 Behavioral changes: 0
- ✅ Compatible: 477 (100%)

**5. Code Modifications**
**No code changes expected.** This is a pure framework upgrade.

**Areas to monitor during build:**
- DevExpress.Xpo API usage (no issues expected)
- SAP HANA driver integration (no issues expected)
- ConnectionProviderSql inheritance (no issues expected)

**6. Testing Strategy**
**Unit Testing:**
- Tests located in XpoSapHana.Tests.csproj
- All tests must pass after upgrade
- No test modifications expected

**Integration Testing:**
- BasicSample.csproj serves as integration test
- Must build and run successfully
- Verify SAP HANA provider registration works

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] `dotnet restore` completes successfully
- [ ] Project builds without errors: `dotnet build src\XpoSapHana\XpoSapHana.csproj`
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] XpoSapHana.Tests.csproj builds successfully (depends on this project)
- [ ] BasicSample.csproj builds successfully (depends on this project)

---

### BasicSample.csproj

**Location:** `samples\BasicSample\BasicSample.csproj`

#### Current State
- **Target Framework:** net6.0
- **Project Type:** Console Application (SDK-style)
- **Dependencies:** 
  - 1 NuGet package: DevExpress.Xpo 23.2.4
  - 1 project reference: XpoSapHana.csproj
- **Dependents:** None (leaf application)
- **Lines of Code:** 149
- **Files:** 1 (Program.cs)
- **Risk Level:** 🟢 Low

#### Target State
- **Target Framework:** net10.0
- **Package Versions:** No changes required
- **Expected Outcome:** Builds and runs without errors

#### Migration Steps

**1. Prerequisites**
- ✅ .NET 10 SDK installed
- ✅ XpoSapHana.csproj already upgraded to net10.0 (atomic operation)
- ✅ DevExpress.Xpo 23.2.4 compatible with net10.0

**2. Framework Update**
Update project file:
```xml
<!-- BEFORE -->
<TargetFramework>net6.0</TargetFramework>

<!-- AFTER -->
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**
**No package updates required.**

| Package | Current Version | Target Version | Status |
|---------|-----------------|----------------|--------|
| DevExpress.Xpo | 23.2.4 | 23.2.4 | ✅ Compatible |

**Project reference:** XpoSapHana.csproj (no version - automatically uses net10.0 after upgrade)

**4. Expected Breaking Changes**
**None.** Assessment analyzed 114 APIs:
- 🔴 Binary incompatible: 0
- 🟡 Source incompatible: 0
- 🔵 Behavioral changes: 0
- ✅ Compatible: 114 (100%)

**5. Code Modifications**
**No code changes expected.**

**Program.cs** demonstrates basic XPO with SAP HANA:
- Creates connection string
- Registers HanaConnectionProvider
- Performs CRUD operations
- No .NET 6-specific patterns identified

**6. Testing Strategy**
**Manual Testing:**
- Build the sample: `dotnet build samples\BasicSample\BasicSample.csproj`
- Run the sample: `dotnet run --project samples\BasicSample\BasicSample.csproj`
- Verify output shows successful CRUD operations

**Note:** This sample requires SAP HANA connection. May skip runtime test if database unavailable, build verification is sufficient.

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] `dotnet restore` completes successfully
- [ ] Project builds without errors: `dotnet build samples\BasicSample\BasicSample.csproj`
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] Can run successfully (if SAP HANA available)

---

### XpoSapHana.Tests.csproj

**Location:** `tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj`

#### Current State
- **Target Framework:** net6.0
- **Project Type:** Test Project (SDK-style)
- **Dependencies:**
  - 4 NuGet packages:
    - Microsoft.NET.Test.Sdk 17.8.0
    - xunit 2.6.2
    - xunit.runner.visualstudio 2.5.4
    - Moq 4.20.70
  - 1 project reference: XpoSapHana.csproj
- **Dependents:** None (test project)
- **Lines of Code:** 207
- **Files:** 3
- **Risk Level:** 🟢 Low

#### Target State
- **Target Framework:** net10.0
- **Package Versions:** No changes required (all test packages compatible)
- **Expected Outcome:** All tests pass

#### Migration Steps

**1. Prerequisites**
- ✅ .NET 10 SDK installed
- ✅ XpoSapHana.csproj already upgraded to net10.0 (atomic operation)
- ✅ All test framework packages compatible with net10.0

**2. Framework Update**
Update project file:
```xml
<!-- BEFORE -->
<TargetFramework>net6.0</TargetFramework>

<!-- AFTER -->
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**
**No package updates required.** All test packages are compatible:

| Package | Current Version | Target Version | Status |
|---------|-----------------|----------------|--------|
| Microsoft.NET.Test.Sdk | 17.8.0 | 17.8.0 | ✅ Compatible |
| xunit | 2.6.2 | 2.6.2 | ✅ Compatible |
| xunit.runner.visualstudio | 2.5.4 | 2.5.4 | ✅ Compatible |
| Moq | 4.20.70 | 4.20.70 | ✅ Compatible |

**4. Expected Breaking Changes**
**None.** Assessment analyzed 213 APIs:
- 🔴 Binary incompatible: 0
- 🟡 Source incompatible: 0
- 🔵 Behavioral changes: 0
- ✅ Compatible: 213 (100%)

**5. Code Modifications**
**No code changes expected.**

Test suite validates:
- HanaConnectionProvider functionality
- SQL generation
- Schema operations
- No .NET 6-specific test patterns identified

**6. Testing Strategy**
**Automated Testing:**
- Run all tests: `dotnet test tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj`
- Verify 100% test pass rate
- Review test output for warnings

**Test execution validation:**
- All existing tests should pass
- No new failures introduced by framework upgrade
- Test discovery works correctly

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] `dotnet restore` completes successfully
- [ ] Project builds without errors: `dotnet build tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj`
- [ ] Project builds without warnings
- [ ] No package dependency conflicts
- [ ] All tests discovered: `dotnet test --list-tests`
- [ ] All tests pass: `dotnet test`
- [ ] No test execution warnings

---

## Package Update Reference

### Summary

**Excellent news:** All 6 NuGet packages in the solution are already compatible with .NET 10. No package version updates are required for this upgrade.

### Package Compatibility Matrix

| Package | Current Version | Target Version | Projects Affected | Update Reason |
|---------|-----------------|----------------|-------------------|---------------|
| DevExpress.Xpo | 23.2.4 | 23.2.4 (no change) | XpoSapHana.csproj, BasicSample.csproj | ✅ Already compatible with net10.0 |
| Sap.Data.Hana.Core.v2.1 | 2.17.22 | 2.17.22 (no change) | XpoSapHana.csproj | ✅ Already compatible with net10.0 |
| Microsoft.NET.Test.Sdk | 17.8.0 | 17.8.0 (no change) | XpoSapHana.Tests.csproj | ✅ Already compatible with net10.0 |
| xunit | 2.6.2 | 2.6.2 (no change) | XpoSapHana.Tests.csproj | ✅ Already compatible with net10.0 |
| xunit.runner.visualstudio | 2.5.4 | 2.5.4 (no change) | XpoSapHana.Tests.csproj | ✅ Already compatible with net10.0 |
| Moq | 4.20.70 | 4.20.70 (no change) | XpoSapHana.Tests.csproj | ✅ Already compatible with net10.0 |

### Package Categories

#### Core Functionality Packages (2 packages)
**DevExpress.Xpo 23.2.4**
- **Projects:** XpoSapHana.csproj, BasicSample.csproj
- **Purpose:** ORM framework (core dependency)
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

**Sap.Data.Hana.Core.v2.1 2.17.22**
- **Projects:** XpoSapHana.csproj
- **Purpose:** SAP HANA ADO.NET driver
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

#### Test Framework Packages (4 packages)
**Microsoft.NET.Test.Sdk 17.8.0**
- **Projects:** XpoSapHana.Tests.csproj
- **Purpose:** Test platform infrastructure
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

**xunit 2.6.2**
- **Projects:** XpoSapHana.Tests.csproj
- **Purpose:** Unit testing framework
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

**xunit.runner.visualstudio 2.5.4**
- **Projects:** XpoSapHana.Tests.csproj
- **Purpose:** Visual Studio test adapter
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

**Moq 4.20.70**
- **Projects:** XpoSapHana.Tests.csproj
- **Purpose:** Mocking framework
- **Compatibility:** ✅ Full compatibility with .NET 10
- **Action:** None required

### Verification Steps

After framework upgrade, verify package compatibility:

1. **Restore packages:** `dotnet restore`
2. **Check for warnings:** Review restore output for any compatibility warnings
3. **Verify no downgrades:** Ensure no packages were automatically downgraded
4. **Test package functionality:** Run test suite to validate all packages work correctly

### Optional Future Updates

While not required for .NET 10 compatibility, these packages have newer versions available:

⚠️ **Note:** These updates are OPTIONAL and outside the scope of this framework upgrade. Consider them in a separate package update initiative.

- Microsoft.NET.Test.Sdk: 17.8.0 → 18.x (newer versions available)
- xunit.runner.visualstudio: 2.5.4 → 3.x (newer major version available)

**Recommendation:** Complete framework upgrade first, then consider package updates separately.

---

## Breaking Changes Catalog

### Executive Summary

**Great news:** Assessment found **ZERO breaking changes** for this upgrade.

- ✅ 804 total APIs analyzed across all projects
- ✅ 0 binary incompatible changes
- ✅ 0 source incompatible changes
- ✅ 0 behavioral changes requiring code modifications
- ✅ 100% API compatibility

### .NET 6 → .NET 10 Framework Breaking Changes

While .NET 6 to .NET 10 does include some breaking changes in the framework, **none affect this solution** based on the comprehensive assessment analysis.

**Common .NET 6→10 breaking changes that do NOT apply here:**
- ❌ No System.Text.Json changes (not used in codebase)
- ❌ No ASP.NET Core breaking changes (not a web application)
- ❌ No Blazor changes (not used)
- ❌ No LINQ provider changes affecting EF Core (not used)
- ❌ No TLS/SSL changes (SAP driver handles this)
- ❌ No globalization changes (no culture-specific code)

### Package-Specific Breaking Changes

#### DevExpress.Xpo 23.2.4
**Status:** ✅ No breaking changes

The DevExpress.Xpo 23.2.4 package is explicitly designed for multi-framework compatibility and has been verified to work with .NET 10 without changes.

**Verified compatibility:**
- ConnectionProviderSql base class: ✅ Compatible
- XPO metadata system: ✅ Compatible
- Session management: ✅ Compatible
- LINQ provider: ✅ Compatible

#### Sap.Data.Hana.Core.v2.1 2.17.22
**Status:** ✅ No breaking changes

SAP's ADO.NET driver maintains backward compatibility across .NET versions.

**Verified compatibility:**
- HanaConnection API: ✅ Compatible
- HanaCommand API: ✅ Compatible
- HanaDataReader API: ✅ Compatible
- Connection string handling: ✅ Compatible

#### Test Framework Packages
**Status:** ✅ No breaking changes

All test packages (xunit 2.6.2, Moq 4.20.70, Microsoft.NET.Test.Sdk 17.8.0) are compatible with .NET 10.

### API Compatibility by Project

#### XpoSapHana.csproj (477 APIs analyzed)
- 🔴 Binary incompatible: **0**
- 🟡 Source incompatible: **0**
- 🔵 Behavioral changes: **0**
- ✅ Compatible: **477 (100%)**

**Key APIs verified:**
- DevExpress.Xpo.DB.ConnectionProviderSql: ✅ Compatible
- DevExpress.Xpo.DB.DataStoreBase: ✅ Compatible
- Sap.Data.Hana APIs: ✅ Compatible

#### BasicSample.csproj (114 APIs analyzed)
- 🔴 Binary incompatible: **0**
- 🟡 Source incompatible: **0**
- 🔵 Behavioral changes: **0**
- ✅ Compatible: **114 (100%)**

**Key APIs verified:**
- DevExpress.Xpo.UnitOfWork: ✅ Compatible
- DevExpress.Xpo.XPObject: ✅ Compatible
- Connection string APIs: ✅ Compatible

#### XpoSapHana.Tests.csproj (213 APIs analyzed)
- 🔴 Binary incompatible: **0**
- 🟡 Source incompatible: **0**
- 🔵 Behavioral changes: **0**
- ✅ Compatible: **213 (100%)**

**Key APIs verified:**
- Xunit.Fact/Theory attributes: ✅ Compatible
- Moq.Mock<T> APIs: ✅ Compatible
- Assert methods: ✅ Compatible

### Expected Compilation Behavior

**After TargetFramework update to net10.0:**

✅ **Expected:** Clean build with 0 errors, 0 warnings  
❌ **Not Expected:** Any compilation errors or warnings

**If warnings appear:**
- Review for potential performance improvements (new analyzers in .NET 10)
- Assess if they require immediate action (most are informational)

**If errors appear (highly unlikely):**
- Verify .NET 10 SDK is properly installed
- Check for typos in TargetFramework element
- Review error message for framework compatibility issues
- Consult this breaking changes documentation: https://learn.microsoft.com/en-us/dotnet/core/compatibility/

### Runtime Behavioral Changes

**None expected.** 

The assessment's behavioral analysis found no runtime changes affecting this codebase.

**Areas pre-validated:**
- Database connection handling: No changes
- SQL generation: No changes
- XPO provider registration: No changes
- Test execution: No changes

### Migration Action Items

**Summary:** NO code changes required for breaking changes.

**Recommended monitoring:**
1. Review build output carefully for any unexpected warnings
2. Run full test suite to validate runtime behavior
3. Spot-check SAP HANA connectivity in BasicSample (if database available)

**If you discover breaking changes not covered here:**
1. Document the specific API and error message
2. Consult official .NET breaking changes documentation
3. Consider filing an assessment issue (this would be unusual given the comprehensive analysis)

---

## Risk Management

### High-Level Risk Assessment

**Overall Risk Level: 🟢 LOW**

This upgrade presents minimal risk due to:
- ✅ Small codebase (798 LOC total)
- ✅ All packages compatible
- ✅ No API breaking changes
- ✅ No security vulnerabilities
- ✅ Clean dependency structure
- ✅ Good test coverage

### Risk Register

| Risk | Likelihood | Impact | Mitigation | Status |
|------|------------|--------|------------|--------|
| Build failures after framework update | Low | Medium | Comprehensive assessment found no incompatibilities; test build before committing | Mitigated |
| Test failures due to behavioral changes | Low | Medium | .NET 6→10 has excellent backward compatibility; run full test suite | Mitigated |
| DevExpress.Xpo compatibility issues | Very Low | High | Package confirmed compatible with net10.0 in assessment | Mitigated |
| SAP HANA driver compatibility | Very Low | High | Sap.Data.Hana.Core.v2.1 confirmed compatible with net10.0 | Mitigated |
| Deployment environment lacks .NET 10 runtime | Low | Medium | Document runtime requirement; verify deployment targets | Accepted |

### Security Vulnerabilities

**None identified.** 

Assessment found 0 security vulnerabilities in current packages. All packages are up-to-date and compatible with .NET 10.

### Contingency Plans

#### If Build Fails
1. Review build errors carefully
2. Check for multi-targeting syntax if needed (unlikely)
3. Verify SDK version: `dotnet --list-sdks`
4. Consult .NET 6→10 breaking changes: https://learn.microsoft.com/en-us/dotnet/core/compatibility/

#### If Tests Fail
1. Isolate failing tests
2. Check for timing-related issues (less common in .NET 10)
3. Review xunit/Moq compatibility with .NET 10
4. Compare behavior between net6.0 and net10.0 if necessary

#### If Package Restore Fails
1. Clear NuGet cache: `dotnet nuget locals all --clear`
2. Restore with verbose logging: `dotnet restore -v detailed`
3. Verify package source accessibility
4. Check for transitive dependency conflicts

### Rollback Plan

**Simple rollback capability:**
- All changes in isolated branch (upgrade-to-NET10)
- Can revert commit or switch back to main branch
- No database migrations or external dependencies
- Rollback time: < 5 minutes

**Rollback triggers:**
- Unresolvable build errors
- Critical test failures
- Blocker discovered in DevExpress.Xpo or SAP driver
- Business decision to postpone

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

This upgrade uses a three-tier validation strategy aligned with the All-at-Once approach:

1. **Build Verification** - Ensure all projects compile successfully
2. **Automated Test Execution** - Run full test suite
3. **Manual Validation** - Spot-check sample application (optional)

### Phase 1: Build Verification

**Objective:** Confirm all projects build cleanly with .NET 10

**Scope:** All 3 projects simultaneously

**Validation Steps:**

```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Build entire solution
dotnet build --configuration Release

# Expected output: Build succeeded. 0 Warning(s). 0 Error(s).
```

**Success Criteria:**
- [ ] ✅ All 3 projects restore successfully
- [ ] ✅ XpoSapHana.csproj builds without errors
- [ ] ✅ BasicSample.csproj builds without errors
- [ ] ✅ XpoSapHana.Tests.csproj builds without errors
- [ ] ✅ 0 build warnings
- [ ] ✅ 0 build errors
- [ ] ✅ No package restore warnings

**If Build Fails:**
1. Review error messages for framework compatibility issues
2. Verify TargetFramework is exactly `net10.0` in all 3 project files
3. Check .NET 10 SDK installation: `dotnet --list-sdks`
4. Clear build artifacts: `dotnet clean && rm -rf bin/ obj/`
5. Consult Breaking Changes Catalog in this plan

---

### Phase 2: Automated Test Execution

**Objective:** Validate functionality with existing test suite

**Scope:** XpoSapHana.Tests.csproj

**Test Execution Steps:**

```bash
# Discover all tests
dotnet test tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj --list-tests

# Run all tests with detailed output
dotnet test tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj --logger "console;verbosity=detailed"

# Run tests with coverage (optional)
dotnet test tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj --collect:"XPlat Code Coverage"
```

**Success Criteria:**
- [ ] ✅ All tests discovered successfully
- [ ] ✅ 100% test pass rate
- [ ] ✅ 0 test failures
- [ ] ✅ 0 test execution errors
- [ ] ✅ No test runner warnings
- [ ] ✅ Test coverage maintained (if measured previously)

**Expected Test Categories:**
- HanaConnectionProvider functionality tests
- SQL generation tests  
- Schema provider tests
- Integration tests (may require SAP HANA connection)

**If Tests Fail:**
1. Isolate failing test(s)
2. Run individually: `dotnet test --filter "FullyQualifiedName~TestName"`
3. Check for timing issues (less common in .NET 10)
4. Review test output for specific error messages
5. Compare behavior with .NET 6 if necessary (switch branches)
6. Consult xunit/Moq .NET 10 compatibility documentation

**Handling Tests Requiring SAP HANA:**
- Some tests may require live SAP HANA connection
- If unavailable, these tests should skip gracefully
- Verify test framework properly handles connection failures

---

### Phase 3: Manual Validation (Optional)

**Objective:** Spot-check end-to-end functionality

**Scope:** BasicSample.csproj

**Validation Steps:**

```bash
# Build sample
dotnet build samples\BasicSample\BasicSample.csproj

# Run sample (requires SAP HANA connection)
dotnet run --project samples\BasicSample\BasicSample.csproj
```

**What to Validate:**
- [ ] Application starts without runtime errors
- [ ] HanaConnectionProvider registers correctly
- [ ] SAP HANA connection establishes (if database available)
- [ ] CRUD operations execute successfully
- [ ] Console output shows expected results
- [ ] Application exits cleanly

**Expected Console Output:**
- Connection string creation
- Provider registration confirmation
- CRUD operation success messages
- No exceptions or error messages

**If SAP HANA Unavailable:**
- ✅ Build success is sufficient validation
- Sample app will fail at runtime (expected without database)
- This doesn't indicate .NET 10 compatibility issues

---

### Smoke Testing Checklist

**Quick validation checklist after upgrade:**

**Environment:**
- [ ] .NET 10 SDK installed and active
- [ ] Branch: upgrade-to-NET10
- [ ] All changes committed (or ready to commit)

**Build Verification:**
- [ ] `dotnet restore` → Success
- [ ] `dotnet build` → Success, 0 errors, 0 warnings
- [ ] All 3 projects build successfully

**Test Verification:**
- [ ] `dotnet test` → All tests pass
- [ ] Test discovery works correctly
- [ ] No test execution warnings

**Package Verification:**
- [ ] No package restore warnings
- [ ] No version conflicts
- [ ] All packages at expected versions

**Project File Verification:**
- [ ] XpoSapHana.csproj: `<TargetFramework>net10.0</TargetFramework>`
- [ ] BasicSample.csproj: `<TargetFramework>net10.0</TargetFramework>`
- [ ] XpoSapHana.Tests.csproj: `<TargetFramework>net10.0</TargetFramework>`

---

### Regression Testing Considerations

**No behavioral changes expected** based on assessment, but monitor:

**XpoSapHana.csproj:**
- SQL generation consistency (compare output before/after)
- Schema reading accuracy
- Connection provider registration
- Type mapping correctness

**BasicSample.csproj:**
- CRUD operations produce same results
- Connection string handling unchanged
- Provider auto-registration works

**XpoSapHana.Tests.csproj:**
- All assertions still valid
- Mock behavior unchanged
- Test isolation maintained

---

### Performance Validation (Optional)

**Not required for this upgrade**, but .NET 10 may show performance improvements:

**If you want to measure:**
```bash
# Benchmark test execution time
dotnet test --logger "console;verbosity=minimal" -- RunConfiguration.DisableParallelization=false
```

**Expected:** Similar or better performance than .NET 6

---

### Comprehensive Validation Report

After all testing phases, document:

**Build Results:**
- Solution builds: ✅ / ❌
- Warnings count: [number]
- Errors count: [number]

**Test Results:**
- Total tests: [number]
- Passed: [number]
- Failed: [number]
- Skipped: [number]
- Pass rate: [percentage]

**Manual Validation:**
- BasicSample runs: ✅ / ❌ / ⏭️ (skipped)
- Observations: [notes]

**Overall Status:**
- Upgrade successful: ✅ / ❌
- Ready to merge: ✅ / ❌
- Issues found: [list or "None"]

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Risk | Rationale |
|---------|------------|--------------|------|-----------|
| XpoSapHana.csproj | 🟢 Low | 2 packages | Low | Core library, 442 LOC, no breaking changes, leaf node in dependency graph |
| BasicSample.csproj | 🟢 Low | 1 package + 1 project | Low | Simple console app, 149 LOC, depends on XpoSapHana |
| XpoSapHana.Tests.csproj | 🟢 Low | 4 packages + 1 project | Low | Test project, 207 LOC, standard xunit setup |

### Phase Complexity Assessment

#### Phase 0: Preparation
- **Complexity:** 🟢 Trivial
- **Operations:** SDK verification only
- **Dependencies:** None

#### Phase 1: Atomic Upgrade
- **Complexity:** 🟢 Low
- **Operations:** Update 3 TargetFramework properties, build, verify
- **Dependencies:** .NET 10 SDK
- **Key factors:**
  - All packages pre-validated as compatible
  - No code changes expected
  - Straightforward project file edits

#### Phase 2: Test Validation
- **Complexity:** 🟢 Low
- **Operations:** Run xunit tests, verify pass
- **Dependencies:** Phase 1 complete
- **Key factors:**
  - Existing test suite
  - No behavioral changes expected

### Overall Solution Complexity

**Rating: 🟢 LOW**

**Factors supporting low complexity:**
- ✅ Small solution (3 projects, 798 LOC)
- ✅ All SDK-style projects (modern, simplified format)
- ✅ Single dependency layer (no deep hierarchies)
- ✅ No circular dependencies
- ✅ All packages compatible (no version conflicts)
- ✅ No API breaking changes (100% compatibility)
- ✅ No platform-specific code
- ✅ No legacy framework dependencies
- ✅ Good test coverage

**Why this is low complexity:**
- Framework upgrade only (no architectural changes)
- Assessment found zero compatibility issues
- Clean, modern codebase structure
- Well-maintained dependencies

### Resource Requirements

**Skill Level Required:**
- Junior to mid-level .NET developer
- Familiarity with .NET project files
- Understanding of NuGet package management
- Basic Git workflow knowledge

**Parallel Work Capacity:**
- All projects updated in single atomic operation
- No parallelization needed (too small to benefit)
- Single developer can execute entire upgrade

**Estimated Relative Effort:**
- **Low:** This is one of the simplest .NET upgrade scenarios
- Direct framework version update
- No refactoring required
- No package updates required
- Minimal validation needed

**Note:** Time estimates are deliberately excluded per planning guidelines. Complexity is expressed relatively (Low/Medium/High) to indicate effort level without predicting specific duration.

---

## Source Control Strategy

### Branching Strategy

**Main Branch:** `main` (source branch)  
**Upgrade Branch:** `upgrade-to-NET10` (target branch for all changes)  
**Merge Strategy:** Pull Request with review before merging to main

**Current Status:**
- ✅ Branch `upgrade-to-NET10` created from `main`
- ✅ Working directory clean (no pending changes)
- ✅ Ready to begin upgrade work

---

### Commit Strategy

**Recommended Approach: Single Atomic Commit**

Given the simplicity of this All-at-Once upgrade, prefer a **single comprehensive commit** containing all changes:

```bash
# After all project files updated and validated
git add .
git commit -m "Upgrade solution from .NET 6 to .NET 10

- Update XpoSapHana.csproj to net10.0
- Update BasicSample.csproj to net10.0
- Update XpoSapHana.Tests.csproj to net10.0

All packages remain compatible (no version changes).
All tests pass. All builds succeed with 0 warnings.

Refs: #[issue-number] (if applicable)"
```

**Why single commit?**
- ✅ All 3 projects interdependent (atomic operation)
- ✅ Cannot meaningfully separate changes
- ✅ Easier to revert if needed (single commit)
- ✅ Clean git history
- ✅ Matches All-at-Once strategy philosophy

**Alternative: Per-Phase Commits (if preferred)**

If your team prefers granular commits:

```bash
# After updating all project files
git add src/ samples/ tests/
git commit -m "Update TargetFramework to net10.0 for all projects"

# After successful build
git commit --allow-empty -m "Verify: All projects build successfully with .NET 10"

# After tests pass
git commit --allow-empty -m "Verify: All tests pass with .NET 10"
```

---

### Commit Message Format

**Template:**
```
<type>: <short summary>

<detailed description>

<validation results>

<references>
```

**Example:**
```
upgrade: Migrate solution from .NET 6 to .NET 10

Updated TargetFramework in all 3 project files:
- src\XpoSapHana\XpoSapHana.csproj
- samples\BasicSample\BasicSample.csproj  
- tests\XpoSapHana.Tests\XpoSapHana.Tests.csproj

No package updates required - all 6 packages compatible.
No code changes required - 100% API compatibility.

Validation:
✅ All projects build successfully (0 errors, 0 warnings)
✅ All tests pass (100% pass rate)
✅ No package conflicts

Closes #[issue-number]
```

---

### Review and Merge Process

**Pull Request Checklist:**

**Before Creating PR:**
- [ ] All commits pushed to `upgrade-to-NET10` branch
- [ ] Full solution builds successfully
- [ ] All tests pass
- [ ] No uncommitted changes
- [ ] Branch up-to-date with `main` (rebase if needed)

**PR Title:**
```
[Upgrade] Migrate solution from .NET 6 to .NET 10
```

**PR Description Template:**
```markdown
## Overview
Upgrade XPO SAP HANA Provider solution from .NET 6 to .NET 10 (LTS).

## Changes
- Updated `TargetFramework` to `net10.0` in all 3 projects
- No package version changes (all compatible)
- No code modifications required

## Validation
✅ **Build:** All projects build successfully (0 errors, 0 warnings)
✅ **Tests:** All tests pass (100% pass rate)
✅ **Packages:** All 6 packages compatible, no conflicts

## Assessment Results
- 804 APIs analyzed: 100% compatible
- 0 breaking changes detected
- 0 security vulnerabilities

## Testing Performed
- [x] Full solution build
- [x] All automated tests
- [ ] Manual BasicSample run (requires SAP HANA - optional)

## Migration Plan
Followed plan: `.github/upgrades/scenarios/new-dotnet-version_39032e/plan.md`

## Checklist
- [x] All project files updated
- [x] Solution builds successfully
- [x] All tests pass
- [x] No warnings introduced
- [x] Documentation updated (if needed)

## Reviewers
@[team-members]
```

**PR Review Criteria:**

Reviewers should verify:
1. **Project Files:** All 3 .csproj files correctly updated to `net10.0`
2. **No Unintended Changes:** Only TargetFramework modified (no package changes)
3. **Build Status:** CI/CD pipeline shows green build
4. **Test Status:** All tests passing in CI/CD
5. **No Code Changes:** Confirm no code modifications (pure framework upgrade)

**Merge Requirements:**
- ✅ At least 1 approval (team decision)
- ✅ CI/CD build passes
- ✅ All tests pass
- ✅ No merge conflicts with main
- ✅ Branch up-to-date with main

**Merge Method:** 
- **Recommended:** Squash and merge (creates single commit in main)
- **Alternative:** Merge commit (preserves all commits if using multi-commit approach)

---

### Post-Merge Actions

**After merge to main:**

1. **Tag Release (Optional):**
   ```bash
   git checkout main
   git pull
   git tag -a v1.0.0-net10 -m "Release: .NET 10 upgrade"
   git push origin v1.0.0-net10
   ```

2. **Update Documentation:**
   - README.md: Update .NET version requirement
   - Any developer setup guides

3. **Notify Team:**
   - Inform developers .NET 10 SDK now required
   - Share upgrade completion status

4. **Clean Up Branch:**
   ```bash
   # Delete remote upgrade branch
   git push origin --delete upgrade-to-NET10

   # Delete local upgrade branch
   git branch -d upgrade-to-NET10
   ```

5. **Update CI/CD (if needed):**
   - Verify CI/CD uses .NET 10 SDK
   - Update build agent requirements
   - Update deployment targets to .NET 10 runtime

---

### Rollback Procedure

**If issues discovered after merge:**

**Option 1: Revert Commit**
```bash
# On main branch
git revert <commit-hash>
git push origin main
```

**Option 2: Emergency Hotfix**
```bash
# Create hotfix branch from last stable commit
git checkout -b hotfix/revert-net10 <commit-before-upgrade>
git push origin hotfix/revert-net10
# Create PR to merge hotfix to main
```

**Option 3: Force Reset (use with caution)**
```bash
# Only if no one else has pulled the changes
git reset --hard <commit-before-upgrade>
git push origin main --force
```

**Recommended:** Option 1 (revert) - safest for shared repositories

---

### Git Configuration Recommendations

**For this upgrade:**

```bash
# Ensure line endings consistent
git config core.autocrlf true  # Windows
git config core.autocrlf input # macOS/Linux

# Ensure .gitignore includes build artifacts
# (should already be configured, but verify)
```

**Verify .gitignore includes:**
```
bin/
obj/
*.user
.vs/
```

---

### Change Tracking

**Files Modified:**
- `src/XpoSapHana/XpoSapHana.csproj` (1 line: TargetFramework)
- `samples/BasicSample/BasicSample.csproj` (1 line: TargetFramework)
- `tests/XpoSapHana.Tests/XpoSapHana.Tests.csproj` (1 line: TargetFramework)

**Files Added:** None

**Files Deleted:** None

**Total Changes:** 3 files, 3 lines modified

---

### Documentation Updates (Post-Merge)

**Update these files after successful merge:**

**README.md:**
```markdown
## Requirements

| Component | Minimum version |
|---|---|
| .NET SDK | 10.0+ |  <!-- UPDATED FROM 6.0+ -->
| DevExpress XPO | 23.2.4+ (license required) |
| SAP HANA Client (ADO.NET) | `Sap.Data.Hana.Core.v2.1` 2.17.22+ |
| SAP HANA Server | 2.0+ |
```

**Any build instructions:**
- Update dotnet SDK version references
- Update deployment documentation
- Update developer onboarding guides

---

## Success Criteria

### Technical Criteria

The migration is considered **technically successful** when ALL of the following criteria are met:

#### Framework Upgrade Criteria
- [x] **All Projects Targeting .NET 10**
  - [ ] XpoSapHana.csproj: `<TargetFramework>net10.0</TargetFramework>`
  - [ ] BasicSample.csproj: `<TargetFramework>net10.0</TargetFramework>`
  - [ ] XpoSapHana.Tests.csproj: `<TargetFramework>net10.0</TargetFramework>`

#### Build Criteria
- [x] **Clean Build**
  - [ ] `dotnet restore` completes without errors
  - [ ] `dotnet build` succeeds for entire solution
  - [ ] 0 compilation errors
  - [ ] 0 compilation warnings
  - [ ] All 3 projects build successfully
  - [ ] Both Debug and Release configurations build

#### Package Criteria
- [x] **Package Compatibility**
  - [ ] All 6 packages remain at current versions (no forced updates)
  - [ ] No package restore errors
  - [ ] No package restore warnings
  - [ ] No dependency conflicts
  - [ ] No transitive dependency issues

#### Test Criteria
- [x] **Test Execution**
  - [ ] All tests discovered successfully
  - [ ] All tests execute without errors
  - [ ] 100% test pass rate (all tests that passed before still pass)
  - [ ] No new test failures introduced
  - [ ] Test runner works correctly with .NET 10

#### API Compatibility Criteria
- [x] **No Breaking Changes**
  - [ ] No API compatibility issues detected (validated by assessment)
  - [ ] All 804 analyzed APIs remain compatible
  - [ ] 0 binary incompatible changes
  - [ ] 0 source incompatible changes
  - [ ] 0 behavioral changes requiring code modifications

#### Security Criteria
- [x] **No New Vulnerabilities**
  - [ ] 0 security vulnerabilities (same as before upgrade)
  - [ ] No packages downgraded to vulnerable versions
  - [ ] No new security warnings from `dotnet list package --vulnerable`

---

### Quality Criteria

The migration maintains **quality standards** when:

#### Code Quality
- [x] **No Code Modifications Required**
  - [ ] Pure framework upgrade (only .csproj files modified)
  - [ ] No source code changes needed
  - [ ] No workarounds or hacks introduced

#### Test Coverage
- [x] **Test Coverage Maintained**
  - [ ] All existing tests still valid
  - [ ] Test coverage percentage unchanged (if measured)
  - [ ] No tests disabled or skipped due to upgrade

#### Performance
- [x] **Performance Acceptable**
  - [ ] Build time similar or better than .NET 6
  - [ ] Test execution time similar or better
  - [ ] No performance regressions observed

#### Documentation
- [x] **Documentation Updated**
  - [ ] README.md updated with .NET 10 requirement
  - [ ] Build instructions updated (if applicable)
  - [ ] Migration documented in git history

---

### Process Criteria

The migration follows **process standards** when:

#### All-at-Once Strategy Adherence
- [x] **Strategy Followed**
  - [ ] All 3 projects upgraded simultaneously (not incrementally)
  - [ ] Single atomic operation completed
  - [ ] No intermediate multi-targeting states
  - [ ] Clean, unified upgrade

#### Source Control
- [x] **Proper Git Workflow**
  - [ ] All changes in `upgrade-to-NET10` branch
  - [ ] Clear, descriptive commit message(s)
  - [ ] No unrelated changes included
  - [ ] Branch ready for PR to main

#### Validation
- [x] **Comprehensive Testing Performed**
  - [ ] Build verification completed
  - [ ] Automated test suite executed
  - [ ] Smoke testing performed (or explicitly skipped with reason)

---

### Acceptance Criteria

**DEFINITION OF DONE:**

The .NET 10 upgrade is **complete and ready to merge** when:

1. ✅ **All Technical Criteria Met** (see above)
2. ✅ **All Quality Criteria Met** (see above)
3. ✅ **All Process Criteria Met** (see above)
4. ✅ **Pull Request Created** with complete description
5. ✅ **CI/CD Pipeline Passes** (if configured)
6. ✅ **Code Review Approved** (per team policy)

---

### Verification Commands

**Quick verification checklist:**

```bash
# 1. Verify .NET 10 SDK installed
dotnet --list-sdks | grep "10.0"

# 2. Verify current branch
git branch --show-current  # Should show: upgrade-to-NET10

# 3. Clean build verification
dotnet clean
dotnet restore
dotnet build --configuration Release

# 4. Test verification
dotnet test --configuration Release

# 5. Verify project files
grep -r "<TargetFramework>net10.0</TargetFramework>" *.csproj

# 6. Check for vulnerabilities
dotnet list package --vulnerable

# 7. Verify no uncommitted changes (after final commit)
git status
```

**Expected output:**
- Build: `Build succeeded. 0 Warning(s). 0 Error(s).`
- Tests: `Passed! - ... tests ... 0 Failed`
- grep: Shows all 3 project files
- git status: `nothing to commit, working tree clean`

---

### Success Metrics

**Quantifiable success indicators:**

| Metric | Before (NET 6) | After (NET 10) | Status |
|--------|----------------|----------------|--------|
| Projects Upgraded | 0/3 | 3/3 | ✅ |
| Build Errors | 0 | 0 | ✅ |
| Build Warnings | 0 | 0 | ✅ |
| Test Pass Rate | 100% | 100% | ✅ |
| Package Compatibility | 100% | 100% | ✅ |
| API Compatibility | 100% | 100% | ✅ |
| Security Vulnerabilities | 0 | 0 | ✅ |
| Code Changes Required | N/A | 0 | ✅ |

---

### Sign-Off Checklist

**Before marking upgrade as complete:**

**Technical Lead Sign-Off:**
- [ ] All projects upgraded correctly
- [ ] Build verification passed
- [ ] Test suite passed
- [ ] No technical issues identified

**QA Sign-Off (if applicable):**
- [ ] Automated tests passed
- [ ] Manual testing completed (or explicitly skipped)
- [ ] No regressions detected

**DevOps Sign-Off (if applicable):**
- [ ] CI/CD pipeline configured for .NET 10
- [ ] Deployment targets support .NET 10 runtime
- [ ] No infrastructure blockers

**Product Owner Sign-Off:**
- [ ] Upgrade aligned with project roadmap
- [ ] No business impact concerns
- [ ] Ready to merge to main

---

### Post-Upgrade Success Indicators

**After merge to main, confirm:**

1. **Developer Adoption:**
   - [ ] Team informed of .NET 10 requirement
   - [ ] Local development environments updated
   - [ ] No developer blockers reported

2. **CI/CD Success:**
   - [ ] First build on main branch succeeds
   - [ ] Automated tests pass in pipeline
   - [ ] Deployment pipeline works (if applicable)

3. **Production Readiness:**
   - [ ] Deployment targets have .NET 10 runtime
   - [ ] No production compatibility issues
   - [ ] Monitoring shows no anomalies

---

### Failure Criteria

**The upgrade is considered FAILED if:**

❌ Any project fails to build after TargetFramework update  
❌ Any previously passing test now fails  
❌ Any package compatibility issues arise  
❌ Any breaking changes discovered that block compilation  
❌ Security vulnerabilities introduced  
❌ Unresolvable errors prevent completion  

**If failure occurs:**
1. Document the specific failure
2. Attempt resolution using this plan's guidance
3. If unresolvable, rollback and reassess
4. Update plan with findings for future attempt

---

### Final Validation Statement

**The .NET 10 upgrade is SUCCESSFUL when:**

> "All 3 projects in the XPO SAP HANA Provider solution target net10.0, build cleanly with 0 errors and 0 warnings, all tests pass at 100% rate, all 6 packages remain compatible, and the solution is functionally equivalent to the .NET 6 version with no regressions."

**Approval:**

- **Date:** __________
- **Approved By:** __________
- **Notes:** __________
