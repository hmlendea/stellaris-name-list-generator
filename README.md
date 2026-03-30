[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/stellaris-name-list-generator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/stellaris-name-list-generator/actions/workflows/dotnet.yml) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/stellaris-name-list-generator)](https://github.com/hmlendea/stellaris-name-list-generator/releases/latest)

# Stellaris Name List Generator

## About

Generate Stellaris-compatible name-list output from structured XML sources.

This project is useful when you want to keep names grouped and maintainable in XML, then compile them into the game-ready output format for use inside mods.

## Features

- Reads one or more name lists from XML.
- Merges multiple lists when present in the same input file.
- Generates a final Stellaris name-list text file.
- Sets name-list metadata such as display name and locked flag from CLI arguments.

## Requirements

- .NET SDK 10.0+
- Linux, macOS, or Windows

## Quick Start

1. Build the project:

```bash
dotnet build
```

2. Run with required arguments:

```bash
dotnet run -- \
	--input "exampleNameList.xml" \
	--output "generated_name_list.txt" \
	--name "My Name List"
```

3. Optional: mark the generated list as locked:

```bash
dotnet run -- \
	--input "exampleNameList.xml" \
	--output "generated_name_list.txt" \
	--name "My Name List" \
	--locked true
```

## CLI Arguments

### Canonical format

Use the long form with explicit values:

- `--input <path>` (required): XML source file.
- `--output <path>` (required): output file path.
- `--name <value>` (required): in-game name list display name.
- `--locked <bool>` (optional): `true` or `false` (default: `false`).

### Backward-compatible aliases

For compatibility with older usage, these aliases are also accepted:

- `-i` for `--input`
- `-o` for `--output`
- `-n` for `--name`
- `-l` for `--locked`

Examples:

```bash
dotnet run -- -i "exampleNameList.xml" -o "generated_name_list.txt" -n "My Name List"
dotnet run -- -i "exampleNameList.xml" -o "generated_name_list.txt" -n "My Name List" -l
```

## Input Format

Use [exampleNameList.xml](exampleNameList.xml) as the base template.

The root is an array of name lists (`ArrayOfNameList`), each with grouped sections such as:

- nationalities
- places
- great people
- warfare
- ships
- ship classes
- stations
- armies

Important notes:

- Keep each section populated with meaningful values.
- Empty collections may cause generation failures depending on the builder logic.

## Helper Scripts

- [extractor.sh](extractor.sh): extracts and normalizes names from an existing Stellaris text file region into XML string entries.
- [merge.sh](merge.sh): merges multiple XML fragments into a single `names.xml` file.
- [release.sh](release.sh): runs the release pipeline using an external deployment script.

## Development

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run -- --input "exampleNameList.xml" --output "generated_name_list.txt" --name "My Name List"
```

## Troubleshooting

- `ArgumentException` or `ArgumentNullException` on startup:
	verify all required arguments are present and each required option has a value.
- `NullReferenceException` during generation:
	check input XML groups for missing or empty data where random selection is expected.
- Output not generated:
	ensure the output path is writable and the parent directory exists.

## Target Framework

The current package targets `.NET 10.0`.

## License

This project is licensed under the `GNU General Public License v3.0` or later. See [LICENSE](./LICENSE) for details.
