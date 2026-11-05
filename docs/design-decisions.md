# Design Decisions

## Overview

This document records key architectural and implementation decisions made during development, including trade-offs and rationale.

## Decision Records

### ADR-001: Clean Architecture with Separation of Concerns

**Date**: 2025-01
**Status**: Accepted

**Context**: Need to organize code for maintainability and testability.

**Decision**: Use clean architecture with four layers:
- `Models/` - Domain entities (NameList, NameGroup, category models)
- `Service/` - Application services (generation, building, downloading)
- `Service/NamesBuilders/` - Category-specific formatters
- `Program.cs` - Composition root

**Consequences**:
- ✅ Clear separation of domain logic from infrastructure
- ✅ Easy to test each layer independently
- ✅ New name categories follow consistent pattern
- ❌ More files than monolithic approach

### ADR-002: XML Repository Pattern for Input Data

**Date**: 2025-01
**Status**: Accepted

**Context**: Need to read Stellaris name list XML files.

**Decision**: Use `NuciDAL.XmlRepository<T>` for deserialization.

**Consequences**:
- ✅ Declarative XML mapping via attributes
- ✅ Handles nested collections automatically
- ✅ Consistent with other Nuci projects
- ❌ Requires NuciDAL dependency
- ❌ Less control than manual parsing

### ADR-003: URL-Based Name Downloads with Caching

**Date**: 2025-01
**Status**: Accepted

**Context**: Some name categories reference external URLs for additional names.

**Decision**:
- `NameGroup.Url` property for source URL
- `FileDownloader` with `CacheManager` (8-hour TTL)
- `NameGroup.UrlValues` downloads and splits on newlines

**Consequences**:
- ✅ Reduces repository size
- ✅ Allows community-maintained name lists
- ✅ Cache prevents repeated downloads
- ❌ Network dependency at build time
- ❌ External URLs may become unavailable

### ADR-004: Unicode Normalization (NFKD) for Name Processing

**Date**: 2025-01
**Status**: Accepted

**Context**: Stellaris expects ASCII-compatible names; input may contain diacritics.

**Decision**: `ProcessName()` applies NFKD normalization + regex removal of combining marks.

**Consequences**:
- ✅ Ensures ASCII output compatibility
- ✅ Handles common diacritics (é, ñ, ü, etc.)
- ✅ Consistent with Paradox script expectations
- ❌ May lose semantic meaning (café → cafe)
- ❌ Complex regex chain hard to maintain

### ADR-005: Builder Pattern for Category-Specific Formatting

**Date**: 2025-01
**Status**: Accepted

**Context**: Each Stellaris name category has unique output format.

**Decision**: Abstract `NamesBuilder` base class with category-specific derived classes.

**Consequences**:
- ✅ Each category encapsulates its format logic
- ✅ Shared utilities in base class (dedup, formatting)
- ✅ Easy to add new categories
- ❌ Some duplication across builders
- ❌ Cross-category references create coupling

### ADR-006: CLI-First Design with NuciCLI

**Date**: 2025-01
**Status**: Accepted

**Context**: Tool must be usable from command line for modders.

**Decision**: Use `NuciCLI` for argument parsing, single executable output.

**Consequences**:
- ✅ Standard CLI experience
- ✅ Self-contained deployment possible
- ✅ Integrates with build scripts
- ❌ No GUI option
- ❌ NuciCLI learning curve

### ADR-007: Merge Strategy for Multiple Input Files

**Date**: 2025-01
**Status**: Accepted

**Context**: Users may provide multiple name list XML files.

**Decision**: `NameList.AddRange()` concatenates all categories; single file uses filename as ID.

**Consequences**:
- ✅ Simple, predictable merge behavior
- ✅ Preserves all names from all inputs
- ✅ ID derivation is intuitive
- ❌ No deduplication across files
- ❌ No conflict resolution for duplicate names

### ADR-008: Target .NET 10.0

**Date**: 2025-01
**Status**: Accepted

**Context**: Choose target framework for new project.

**Decision**: Target `net10.0` (latest at project creation).

**Consequences**:
- ✅ Latest performance improvements
- ✅ C# 13 language features
- ✅ Long-term support timeline
- ❌ Requires .NET 10 runtime
- ❌ May not be available on all CI systems initially

### ADR-009: File-System Cache in `.cache/` Directory

**Date**: 2025-01
**Status**: Accepted

**Context**: Need to cache downloaded name lists.

**Decision**: `CacheManager` uses `.cache/` in working directory with 8-hour expiry.

**Consequences**:
- ✅ No external dependencies (Redis, etc.)
- ✅ Works in CI/CD without setup
- ✅ Automatic cleanup via TTL
- ❌ Cache not shared across machines
- ❌ `.cache/` may be committed if not gitignored

### ADR-010: NUnit + Moq for Testing

**Date**: 2025-01
**Status**: Accepted

**Context**: Choose testing framework.

**Decision**: NUnit 3.14.0 with Moq 4.20.72.

**Consequences**:
- ✅ Mature, well-documented
- ✅ Good VS Code integration
- ✅ Parameterized tests via TestCase
- ❌ xUnit more popular in .NET ecosystem
- ❌ Moq has some limitations with non-virtual methods

## Trade-off Summary

| Decision | Benefit | Cost |
|----------|---------|------|
| Clean Architecture | Maintainability, testability | More files |
| XML Repository | Declarative mapping | NuciDAL dependency |
| URL Downloads | Community extensibility | Network dependency |
| Unicode Normalization | ASCII compatibility | Potential data loss |
| Builder Pattern | Category isolation | Some duplication |
| CLI-First | Automation friendly | No GUI |
| Merge Strategy | Simple, predictable | No dedup/conflict resolution |
| .NET 10.0 | Modern features | Runtime requirement |
| File Cache | Zero config | Not distributed |
| NUnit + Moq | Familiar tooling | Not xUnit |

## Future Considerations

### Potential Changes

1. **Add deduplication option** - Configurable merge strategy
2. **Support JSON input** - Alternative to XML
3. **Add GUI** - Optional Avalonia/MAUI frontend
4. **Distributed cache** - Redis for CI/CD sharing
5. **Source generators** - Reduce boilerplate in builders
6. **AOT compilation** - Native AOT for faster startup

### Deferred Decisions

- Localization support (currently English-only output)
- Parallel download optimization
- Streaming XML for large files
- Plugin system for custom builders