# Related Documentation

## Overview

This document lists complementary documentation, external resources, and references relevant to the Stellaris Name List Generator project.

## Internal Documentation

### Project Documentation

| Document | Description | Location |
|----------|-------------|----------|
| README.md | Project overview, usage, installation | `/README.md` |
| ARCHITECTURE.md | System architecture, patterns, decisions | `/ARCHITECTURE.md` |
| CONTRIBUTING.md | Contribution guidelines, workflow | `/CONTRIBUTING.md` |
| SECURITY.md | Security policy, vulnerability reporting | `/SECURITY.md` |
| PRIVACY.md | Data handling, privacy practices | `/PRIVACY.md` |
| CODE_OF_CONDUCT.md | Community standards, behavior | `/CODE_OF_CONDUCT.md` |
| ROADMAP.md | Planned features, milestones | `/ROADMAP.md` |
| LICENSE | MIT License | `/LICENSE` |

### Technical Documentation (`docs/`)

| Document | Description |
|----------|-------------|
| INDEX.md | Documentation index and navigation |
| repository-overview.md | High-level project summary |
| repository-structure.md | Directory and file organization |
| architecture.md | Architectural patterns and principles |
| data-model.md | Domain models and relationships |
| components/presentation.md | CLI layer and entry point |
| components/application-services.md | Core services (generator, builder, downloader) |
| components/names-builders.md | Category-specific name builders |
| flows/name-list-generation.md | End-to-end generation flow |
| flows/name-download.md | URL-based name download flow |
| configuration.md | Build, runtime, and environment configuration |
| dependencies.md | External dependencies and versions |
| build-and-deployment.md | Build, test, release, CI/CD |
| testing.md | Test strategy, coverage, patterns |
| change-guide.md | Migration, upgrade, compatibility |
| design-decisions.md | Architectural decisions and trade-offs |
| ambiguities.md | Known uncertainties and open questions |

## External References

### Stellaris Modding

| Resource | URL | Description |
|----------|-----|-------------|
| Stellaris Wiki - Modding | https://stellaris.paradoxwikis.com/Modding | Official modding documentation |
| Stellaris Wiki - Name Lists | https://stellaris.paradoxwikis.com/Name_list | Name list format reference |
| Paradox Modding Forum | https://forum.paradoxplaza.com/forum/forums/stellaris-modding.941/ | Community modding forum |
| Stellaris Modding Discord | https://discord.gg/stellaris-modding | Real-time modding help |

### Paradox Script Format

| Resource | URL | Description |
|----------|-----|-------------|
| Clausewitz Script Syntax | https://stellaris.paradoxwikis.com/Scripting | Scripting language reference |
| Localisation | https://stellaris.paradoxwikis.com/Localisation | Localisation file format |
| Scripted Effects/Triggers | https://stellaris.paradoxwikis.com/Scripted_effects_and_triggers | Advanced scripting |

### .NET / C# References

| Resource | URL | Description |
|----------|-----|-------------|
| .NET 10 Documentation | https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10 | Target framework docs |
| C# Language Reference | https://learn.microsoft.com/dotnet/csharp/language-reference/ | C# language spec |
| NUnit Documentation | https://docs.nunit.org/ | Testing framework |
| Moq Documentation | https://github.com/moq/moq4 | Mocking library |

### NuGet Packages Used

| Package | Version | Repository | Description |
|---------|---------|------------|-------------|
| NuciCLI | 3.0.1 | https://github.com/hmlendea/NuciCLI | CLI argument parsing |
| NuciCLI.Arguments | 1.0.1 | https://github.com/hmlendea/NuciCLI | Argument definitions |
| NuciDAL | 3.0.1 | https://github.com/hmlendea/NuciDAL | XML data access layer |
| NuciExtensions | 5.3.1 | https://github.com/hmlendea/NuciExtensions | Collection/text utilities |

### GitHub Actions

| Resource | URL | Description |
|----------|-----|-------------|
| GitHub Actions Docs | https://docs.github.com/actions | CI/CD workflow reference |
| dotnet.yml | `.github/workflows/dotnet.yml` | Build and test workflow |
| security.yml | `.github/workflows/security.yml` | Security scanning workflow |

## Standards and Conventions

### Code Style

| Standard | Reference |
|----------|-----------|
| C# Coding Conventions | https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions |
| .NET Naming Guidelines | https://learn.microsoft.com/dotnet/standard/design-guidelines/naming-guidelines |
| EditorConfig | `.editorconfig` (if present) |

### Documentation Style

| Standard | Reference |
|----------|-----------|
| Markdown Guide | https://www.markdownguide.org/ |
| GitHub Flavored Markdown | https://github.github.com/gfm/ |
| Mermaid Diagrams | https://mermaid.js.org/ |

## Community Resources

### Stellaris Modding Community

- **Reddit**: r/StellarisModding
- **Discord**: Stellaris Modding (official)
- **Steam Workshop**: Stellaris Mods

### Open Source

- **GitHub**: https://github.com/hmlendea/stellaris-name-list-generator
- **Issues**: Bug reports and feature requests
- **Discussions**: Questions and community help

## Learning Resources

### For Contributors

| Topic | Resource |
|-------|----------|
| C# for Beginners | https://learn.microsoft.com/dotnet/csharp/tour-of-csharp/ |
| .NET CLI | https://learn.microsoft.com/dotnet/core/tools/ |
| Git Workflow | https://guides.github.com/introduction/flow/ |
| Unit Testing | https://docs.nunit.org/articles/nunit/writing-tests/ |

### For Modders

| Topic | Resource |
|-------|----------|
| Stellaris Modding Tutorial | https://stellaris.paradoxwikis.com/Modding_tutorial |
| Creating Name Lists | https://stellaris.paradoxwikis.com/Name_list |
| Scripting Basics | https://stellaris.paradoxwikis.com/Scripting |

## Version-Specific Notes

### .NET 10.0

- Target framework: `net10.0`
- C# 13 features available
- Performance improvements over .NET 8/9

### Nuci Packages

All Nuci packages are from the same author (hmlendea) and designed to work together. Version alignment is maintained.

## Deprecated References

None currently. All references are current as of project creation.

## Contributing to Documentation

To add or update related documentation:

1. Edit this file (`docs/related-documentation.md`)
2. Update `docs/INDEX.md` if adding new internal docs
3. Update `README.md` if adding new external resources
4. Follow the existing table format
5. Keep descriptions concise (one line)
6. Verify URLs are accessible

## Maintenance

Review this document quarterly or when:
- New dependencies are added
- External resources change URLs
- New internal documentation is created
- Project standards are updated