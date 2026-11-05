# Ambiguities and Open Questions

## Overview

This document records known uncertainties, unresolved questions, and areas requiring clarification in the Stellaris Name List Generator project.

## Open Questions

### AMB-001: Stellaris Name List Format Completeness

**Question**: Does the current output format cover all Stellaris name list categories and options?

**Context**: The generator produces 6 main categories (ship_names, ship_class_names, fleet_names, army_names, planet_names, character_names). Stellaris may have additional categories or sub-options not yet implemented.

**Impact**: Missing categories would require new models, builders, and tests.

**Status**: Under investigation. Need to verify against latest Stellaris wiki and game files.

### AMB-002: Unicode Normalization Edge Cases

**Question**: Does NFKD normalization handle all diacritics and special characters correctly for Stellaris?

**Context**: `ProcessName()` uses NFKD + regex to strip combining marks. Some characters may not decompose as expected (e.g., ł, ø, å, ß).

**Impact**: Names with unsupported characters may appear garbled or be rejected by game.

**Status**: Known limitation. Testing with diverse name sets needed.

### AMB-003: URL Download Reliability

**Question**: How should the generator handle permanently unavailable URLs?

**Context**: `NameGroup.UrlValues` downloads from external URLs. If a URL 404s or times out, it returns empty list silently.

**Impact**: Missing names from unavailable sources; no user notification.

**Options**:
- Fail build with error
- Log warning and continue
- Fallback to cached version (if exists)
- Configurable behavior

**Status**: No decision. Current behavior: silent empty list.

### AMB-004: Cache Invalidation Strategy

**Question**: Is 8-hour TTL appropriate for all use cases?

**Context**: `CacheManager` uses fixed 8-hour expiry. CI/CD runs may need fresh data; local development may prefer longer cache.

**Impact**: Stale data in CI; unnecessary downloads locally.

**Options**:
- Configurable TTL via CLI/environment
- Force-refresh flag
- Cache versioning with content hash
- Per-URL TTL

**Status**: No decision. Fixed 8-hour TTL.

### AMB-005: Merge Conflict Resolution

**Question**: How should duplicate names across input files be handled?

**Context**: `NameList.AddRange()` concatenates all names. Duplicates are not removed.

**Impact**: Duplicate entries in output; game may handle gracefully or log warnings.

**Options**:
- Deduplicate by value (case-insensitive)
- Deduplicate by normalized value
- Preserve all (current)
- Configurable strategy

**Status**: No decision. Current: preserve all.

### AMB-006: Character Name Gender Handling

**Question**: Are the current gender categories (Male/Female/NonBinary) sufficient for Stellaris?

**Context**: `CharacterNames` model has separate lists for MaleFirstNames, FemaleFirstNames, NonBinaryFirstNames, etc. Stellaris may have different gender system.

**Impact**: Incorrect gender assignment in generated names.

**Status**: Need verification against Stellaris documentation.

### AMB-007: Weight/Ordering in Character Names

**Question**: How does the `Weight` property affect in-game name selection?

**Context**: `CharacterNamesBuilder` orders by Weight descending. Unclear if Stellaris uses weight for probability or just ordering.

**Impact**: Incorrect weighting may skew name distribution.

**Status**: Need verification against Stellaris scripting docs.

### AMB-008: Fleet Name Template Completeness

**Question**: Are the 13 fleet name templates exhaustive?

**Context**: `FleetNamesBuilder` uses 13 format strings combining sequential names with Weapons, MilitaryUnitTypes, ShipTypes, MythologicalCreatures.

**Impact**: Missing templates reduce variety.

**Status**: Based on observed patterns. May not cover all Stellaris fleet name formats.

### AMB-009: Army Name Special Cases

**Question**: Are PsionicArmy and XenomorphArmy generation rules correct?

**Context**: `ArmyNamesBuilder` has special logic for PsionicArmy (unified from MythologicalCreatures) and XenomorphArmy (first/second word combinations).

**Impact**: Incorrect special army names.

**Status**: Based on reverse-engineering. Not verified against game code.

### AMB-010: Planet Class Coverage

**Question**: Does `PlanetNamesBuilder` cover all Stellaris planet classes?

**Context**: Builder handles 16 planet classes. Stellaris may have more (e.g., Ring World, Habitat, Shattered Ring variants).

**Impact**: Missing planet classes = no names for those types.

**Status**: Need verification against current Stellaris version.

## Technical Uncertainties

### AMB-011: NuciDAL XML Mapping Completeness

**Question**: Does `XmlRepository<NameList>` correctly map all XML structures?

**Context**: NuciDAL uses attribute-based mapping. Complex nested structures may not map correctly.

**Impact**: Silent data loss or incorrect deserialization.

**Status**: Works for known examples. Edge cases untested.

### AMB-012: Large File Handling

**Question**: How does the generator perform with very large input files (10k+ names)?

**Context**: Current implementation loads all into memory. No streaming.

**Impact**: OOM errors or slow performance.

**Status**: Not tested. Assumed small inputs.

### AMB-013: Concurrent Download Safety

**Question**: Is `FileDownloader` thread-safe for concurrent URL downloads?

**Context**: Multiple `NameGroup` instances may download simultaneously.

**Impact**: Race conditions, corrupted cache, HTTP connection exhaustion.

**Status**: Not verified. `HttpClient` is thread-safe but cache writes may conflict.

### AMB-014: Cross-Platform Path Handling

**Question**: Does the tool work correctly on Windows (path separators, line endings)?

**Context**: Developed on Linux. Uses `Path.Combine` but cache directory is `.cache/`.

**Impact**: Cache path issues, line ending differences in output.

**Status**: Not tested on Windows.

### AMB-015: Self-Contained Deployment

**Question**: Does `dotnet publish` produce a working self-contained executable?

**Context**: CLI tool intended for modders who may not have .NET installed.

**Impact**: Distribution complexity if not self-contained.

**Status**: Not verified. Requires testing.

## Specification Gaps

### AMB-016: Output Format Validation

**Question**: Is there a formal schema for Stellaris name list output?

**Context**: Output is Paradox script format. No formal validation beyond game loading.

**Impact**: Syntax errors only caught at game load time.

**Options**:
- Add script syntax validator
- Integrate with Paradox CLI tools
- Unit test output format

**Status**: No validator. Manual testing only.

### AMB-017: Localization Support

**Question**: Should the generator support localized name lists?

**Context**: Stellaris supports multiple languages. Current generator produces English-only.

**Impact**: Non-English players get English names.

**Options**:
- Add language parameter
- Generate per-language files
- Use Stellaris localisation system

**Status**: Out of scope for v1.0.0.

### AMB-018: Mod Compatibility

**Question**: How does generated name list interact with other mods?

**Context**: Multiple mods may provide name lists. Load order determines precedence.

**Impact**: Names may be overridden or cause conflicts.

**Status**: Unknown. Requires modding community input.

## Process Uncertainties

### AMB-019: Release Cadence

**Question**: What should the release schedule be?

**Context**: Stellaris updates may require generator updates.

**Impact**: Users may need frequent updates.

**Options**:
- Per Stellaris major version
- Per Stellaris minor version
- On demand
- Automated on Stellaris release

**Status**: No decision.

### AMB-020: Community Name List Curation

**Question**: How to manage community-contributed name lists (URLs)?

**Context**: URLs in `NameGroup.Url` point to external sources. No curation process.

**Impact**: Quality, availability, and appropriateness uncontrolled.

**Options**:
- Curated allowlist
- Community voting
- Automated quality checks
- No curation (current)

**Status**: No decision.

## Resolved Ambiguities

### RES-001: Target Framework

**Question**: Which .NET version to target?

**Resolution**: .NET 10.0 (ADR-008)

### RES-002: Testing Framework

**Question**: NUnit vs xUnit?

**Resolution**: NUnit + Moq (ADR-010)

### RES-003: CLI Library

**Question**: Which CLI parser?

**Resolution**: NuciCLI (ADR-006)

### RES-004: Cache Backend

**Question**: File system vs Redis vs SQLite?

**Resolution**: File system (ADR-009)

## Tracking

| ID | Title | Priority | Status | Owner |
|----|-------|----------|--------|-------|
| AMB-001 | Format completeness | High | Open | - |
| AMB-002 | Unicode edge cases | High | Open | - |
| AMB-003 | URL reliability | Medium | Open | - |
| AMB-004 | Cache TTL | Medium | Open | - |
| AMB-005 | Merge conflicts | Medium | Open | - |
| AMB-006 | Gender handling | Medium | Open | - |
| AMB-007 | Weight semantics | Medium | Open | - |
| AMB-008 | Fleet templates | Low | Open | - |
| AMB-009 | Army special cases | Low | Open | - |
| AMB-010 | Planet classes | Medium | Open | - |
| AMB-011 | XML mapping | Medium | Open | - |
| AMB-012 | Large files | Low | Open | - |
| AMB-013 | Concurrent downloads | Medium | Open | - |
| AMB-014 | Windows support | Medium | Open | - |
| AMB-015 | Self-contained deploy | High | Open | - |
| AMB-016 | Output validation | Medium | Open | - |
| AMB-017 | Localization | Low | Deferred | - |
| AMB-018 | Mod compatibility | Low | Open | - |
| AMB-019 | Release cadence | Low | Open | - |
| AMB-020 | Community curation | Low | Open | - |

## Resolution Process

To resolve an ambiguity:

1. **Research**: Check Stellaris wiki, game files, community forums
2. **Propose**: Create GitHub issue with findings and options
3. **Decide**: Team discussion, document decision in ADR
4. **Implement**: Code change with tests
5. **Verify**: Test against game, update docs
6. **Close**: Move from Open to Resolved in this document

## Contributing

To add a new ambiguity:
1. Add entry to Open Questions or Technical Uncertainties
2. Assign ID (next sequential AMB-XXX)
3. Set priority (High/Medium/Low)
4. Set status to Open
5. Link related issues/PRs if any