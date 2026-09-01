[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/stellaris-name-list-generator)](https://github.com/hmlendea/stellaris-name-list-generator/releases/latest)
[![Build Status](https://github.com/hmlendea/stellaris-name-list-generator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/stellaris-name-list-generator/actions/workflows/dotnet.yml)
[![License](https://img.shields.io/github/license/hmlendea/stellaris-name-list-generator)](https://github.com/hmlendea/stellaris-name-list-generator/blob/master/LICENSE)

# Stellaris Name List Generator

Generate Stellaris-compatible name-list text from structured XML sources.

## 📑 Table of Contents

- [Table of Contents](#table-of-contents)
- [Capabilities](#capabilities)
- [Usage](#usage)
- [Command Reference](#command-reference)
- [System Requirements](#system-requirements)
- [Installation](#installation)
- [Configuration](#configuration)
  - [Configuration Files](#configuration-files)
  - [Command-Line Options](#command-line-options)
- [Compatibility](#compatibility)
- [Development](#development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Run](#run)
  - [Continuous Integration](#continuous-integration)
  - [Release](#release)
  - [Dependencies](#dependencies)
- [Project Structure](#project-structure)
  - [Directories](#directories)
- [Architecture](#architecture)
- [Contributing](#contributing)
- [Project Engagement](#project-engagement)
- [License](#license)

## ✨ Capabilities

- Converts one or more XML name lists into a Stellaris name-list file.
- Merges name-list records from the supplied XML document.
- Generates ship, ship-class, fleet, army, planet, and character name categories.
- Accepts explicit XML values and optional URL-backed name groups.
- Sets the output identifier, display name, and lock state from command-line arguments.

## 🚀 Usage

```bash
dotnet run -- --input "exampleNameList.xml" --output "generated_name_list.txt" --name "My Name List"
```

The command writes a Stellaris text file to the selected output path. Use `--locked true` to generate a non-randomised name list.

## ⌨️ Command Reference

| Command | Description |
|---------|-------------|
| `--input <path>` | Required XML source file. The `-i` alias is also accepted. |
| `--output <path>` | Required generated file path. The `-o` alias is also accepted. |
| `--name <value>` | Required in-game display name. The `-n` alias is also accepted. |
| `--locked <true\|false>` | Optional lock state; defaults to `false`. The `-l` alias is accepted, and a bare `-l` means `true`. |

## 🖥️ System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| .NET SDK | 10.0 | N/A |
| Operating system | A platform supported by .NET 10 | N/A |

## 📦 Installation

[![Obtain it from GitHub](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/github.png)](https://github.com/hmlendea/stellaris-name-list-generator/releases)

Obtain the current release from [GitHub Releases](https://github.com/hmlendea/stellaris-name-list-generator/releases).

## ⚙️ Configuration

The generator receives its invocation configuration through command-line options and reads name data from an XML document. [exampleNameList.xml](exampleNameList.xml) provides the supported input structure.

### Configuration Files

| File | Scope | Purpose |
|------|-------|---------|
| `exampleNameList.xml` | Generation input | Template for one or more `NameList` records and their name groups. |

### Command-Line Options

| Option | Value | Default | Description |
|--------|-------|---------|-------------|
| `--input` | XML path | — | Required source document. |
| `--output` | File path | — | Required destination for generated Stellaris text. |
| `--name` | Text | — | Required display name for the generated list. |
| `--locked` | `true` or `false` | `false` | Whether the generated list is locked. |

## 🧩 Compatibility

| Component | Supported Versions | Notes |
|-----------|--------------------|-------|
| .NET | 10.0 | The executable targets `net10.0`. |
| Stellaris name-list format | Current project output | The generator produces a Stellaris-compatible text file for mod content. |
| XML input | `ArrayOfNameList` | The input may contain one or more `NameList` records. |

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

```bash
git clone https://github.com/hmlendea/stellaris-name-list-generator.git
cd stellaris-name-list-generator

dotnet restore
```

### Build

```bash
dotnet build --no-restore
```

### Run

```bash
dotnet run --no-build -- --input "exampleNameList.xml" --output "generated_name_list.txt" --name "My Name List"
```

### Continuous Integration

The GitHub Actions workflow restores dependencies, builds the project, and executes `dotnet test --no-build --verbosity normal` for pushes and pull requests targeting `master`.

### Release

The repository includes `release.sh`, which delegates to the upstream deployment script used by the project maintainer.

```bash
bash ./release.sh 3.4.3
```

This script downloads and executes an external release helper from `https://raw.githubusercontent.com/hmlendea/deployment-scripts/master/release/dotnet/10.0.sh`.

**Note:** Piping into `bash` is an intensely controversial topic. Please review any external scripts before running them in your environment!

### Dependencies

| Package | Version | Scope | Purpose |
|---------|---------|-------|---------|
| `NuciCLI` | 3.0.1 | Runtime | Command-line processing. |
| `NuciCLI.Arguments` | 1.0.1 | Runtime | Command argument parsing. |
| `NuciDAL` | 3.0.1 | Runtime | XML repository access. |
| `NuciExtensions` | 5.3.1 | Runtime | Collection and text utilities. |

## 🗂️ Project Structure

The repository contains one .NET console project with domain models and generation services.

### Directories

| Directory | Purpose |
|-----------|---------|
| `Models/` | XML-serialised name-list model types. |
| `Service/` | Generation, content construction, download, and cache services. |
| `Service/NamesBuilders/` | Category-specific Stellaris name-format builders. |
| `.github/workflows/` | Continuous-integration workflow definitions. |

## 🏗️ Architecture

See the [architecture documentation](ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so:
- Maintain cross-platform compatibility
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/stellaris-name-list-generator/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for further information.
