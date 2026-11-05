# Repository Overview

## Purpose

Stellaris Name List Generator is a command-line tool that generates [Stellaris](https://www.stellaris.com/) name list files from XML input sources. It reads one or more XML name list definitions, merges them, and produces a single Stellaris-compatible name list file in the game's Paradox script format.

## Problem Domain

Stellaris is a grand strategy game that uses name lists to name ships, fleets, planets, characters, armies, and other entities for each playable species. Creating a complete name list by hand requires:

- Hundreds of names across dozens of categories
- Correct Paradox script syntax with proper nesting and indentation
- Consistent formatting (quoting, line length, deduplication)
- Unicode normalization for game compatibility

This tool automates that process. Users provide XML files describing name groups (either explicit name lists or URLs to download names from), and the tool generates a complete, correctly formatted name list file.

## Key Capabilities

- **XML input**: Reads name lists from XML files via `NuciDAL` repository abstraction
- **Merging**: Combines multiple input name lists into one output
- **URL-based names**: Downloads name lists from URLs with local caching (8-hour TTL)
- **Category composition**: Automatically derives ship, fleet, planet, and army names from related categories (deities, places, warfare terms, mythological creatures)
- **Unicode normalization**: Extensive character mapping to ensure game-compatible output
- **Deduplication**: Removes duplicate names (diacritic-insensitive) across groups
- **Formatting**: Correct Paradox script output with 4-space indentation, 150-character line wrapping, and quoting of multi-word names

## Scope

- **In scope**: Generating Stellaris name list files from XML input
- **Out of scope**: Editing existing Stellaris name lists, managing game installations, providing a GUI

## Target Users

- Stellaris modders who want to create custom name lists for their species
- Contributors extending the tool with new name categories or mappings

## Technology Stack

| Technology | Role |
|------------|------|
| C# / .NET 10.0 | Application language and runtime |
| NuciCLI | Command-line argument processing |
| NuciDAL | XML repository access |
| NuciExtensions | Collection and text utilities |
| NUnit | Unit testing framework |
| Moq | Mocking for unit tests |

## Distribution

The tool is distributed as a .NET console application via [GitHub Releases](https://github.com/hmlendea/stellaris-name-list-generator/releases). It runs on any platform supported by .NET 10.
