# Change Guide

## Overview

This document describes how to upgrade, migrate, or modify the Stellaris Name List Generator project. It covers breaking changes, version compatibility, and migration steps.

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2025-01 | Initial release |

## Breaking Changes

### None (v1.0.0)

This is the initial release. No breaking changes are planned for v1.0.0.

## Migration Steps

### Upgrading from v0.x to v1.0.0

No migration required. The v1.0.0 release is a new project with no backward compatibility dependencies.

### Adding New Name Categories

When adding a new name category (e.g., `FleetNames`):

1. **Add model**: Create `Models/FleetNames.cs` with `List<NameGroup>` property
2. **Add builder**: Create `Service/NamesBuilders/FleetNamesBuilder.cs` implementing `IFleetNamesBuilder`
3. **Register builder**: Add to `Program.cs` composition root
4. **Update tests**: Add `OtherModelTests` entry and builder test if applicable
5. **Update docs**: Update `docs/data-model.md`, `docs/components/names-builders.md`, `docs/flows/name-list-generation.md`
6. **Update README**: Add new category to feature list

### Changing Output Format

If the output format changes (e.g., adding new fields):

1. Update `FileContentBuilder.cs` header generation
2. Update `FileContentBuilderTests.cs` to verify new format
3. Update `docs/flows/name-list-generation.md` if flow changes
4. Update `exampleNameList.xml` if example needs updating

### Updating Dependencies

When updating NuGet packages:

```bash
dotnet restore --force
dotnet build
dotnet test
```

Check for breaking changes in:
- NuciCLI 3.0.1 → 4.0.0 (if upgrading)
- NuciDAL 3.0.1 → 4.0.0 (if upgrading)
- NuciExtensions 5.3.1 → 6.0.0 (if upgrading)

Each major version may have API changes requiring code updates.

### Renaming Files

If renaming a file:

1. Rename in source control
2. Update all references (imports, usings, XML docs)
3. Update tests if they reference the old path
4. Update documentation references

## Compatibility Matrix

### .NET Framework

| Version | Status | Notes |
|---------|--------|-------|
| .NET 8.0 | Supported | LTS baseline |
| .NET 9.0 | Supported | Current LTS |
| .NET 10.0 | Supported | Target framework |

### Paradox Launcher

| Version | Status | Notes |
|---------|--------|-------|
| 1.7+ | Required | Minimum supported |
| 1.8+ | Recommended | Latest stable |
| 1.9+ | Latest | Current development |

The script format is compatible across all supported versions.

## Known Incompatibilities

### External Mods

The generated script may conflict with certain Stellaris mods that also generate name lists. Users should:

1. Check mod compatibility before running
2. Back up existing name list files
3. Merge manually if needed

### Unicode Normalization

The `ProcessName()` method applies Unicode normalization (NFKD). This could theoretically affect names from mods that use non-standard Unicode. However, this is unlikely to cause issues as most Stellaris names use standard Latin characters.

## Rollback Procedure

To rollback to previous version:

```bash
git checkout <previous-commit>
dotnet build
dotnet run --input input.xml --output output.txt
```

Restore previous name list files from backup if needed.

## Release Process

### Pre-Release Checklist

- [ ] All tests pass (`dotnet test`)
- [ ] Build succeeds (`dotnet build`)
- [ ] Documentation updated
- [ ] README updated
- [ ] Release notes written

### Release Commands

```bash
# Build release
dotnet publish -c Release -o bin/Release/net10.0

# Create GitHub Release
gh release create v1.0.0 --attach bin/Release/net10.0/StellarisNameListGenerator.dll
```

## Change Log Template

For future releases, add entries here:

```markdown
## [Version] - YYYY-MM-DD

### Added
- Description of new feature

### Changed
- Description of change

### Fixed
- Description of bug fix

### Removed
- Description of removed feature
```
