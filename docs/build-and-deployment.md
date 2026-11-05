# Build and Deployment

## Overview

The project uses standard .NET CLI commands for building, publishing, and releasing. GitHub Actions handles CI/CD with automated testing, building, and GitHub Releases.

## Build Commands

### Local Development

```bash
# Restore dependencies
dotnet restore

# Build (Debug)
dotnet build

# Build (Release)
dotnet build -c Release

# Run with arguments
dotnet run -- -i input.xml -o output.txt -n "Test"

# Watch mode (auto-rebuild on changes)
dotnet watch run -- -i input.xml -o output.txt -n "Test"
```

### VS Code Tasks

Predefined tasks in `.vscode/tasks.json` (via `tasks.json` generation):

| Task | Command | Purpose |
|------|---------|---------|
| `build` | `dotnet build` | Debug build |
| `publish` | `dotnet publish` | Release publish |
| `watch` | `dotnet watch run` | Hot reload development |

## Project Structure

```
StellarisNameListGenerator.slnx
├── StellarisNameListGenerator/
│   ├── StellarisNameListGenerator.csproj
│   ├── Program.cs
│   ├── Models/
│   ├── Service/
│   └── Service/NamesBuilders/
└── StellarisNameListGenerator.UnitTests/
    ├── StellarisNameListGenerator.UnitTests.csproj
    ├── Models/
    └── Service/
```

### Solution File (`.slnx`)

```xml
<!-- StellarisNameListGenerator.slnx -->
<Solution>
  <Project Name="StellarisNameListGenerator" Path="StellarisNameListGenerator/StellarisNameListGenerator.csproj" />
  <Project Name="StellarisNameListGenerator.UnitTests" Path="StellarisNameListGenerator.UnitTests/StellarisNameListGenerator.UnitTests.csproj" />
</Solution>
```

### Main Project (`.csproj`)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>false</SelfContained>
    <TrimMode>partial</TrimMode>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="NuciCLI" Version="3.0.1" />
    <PackageReference Include="NuciCLI.Arguments" Version="1.0.1" />
    <PackageReference Include="NuciDAL" Version="3.0.1" />
    <PackageReference Include="NuciExtensions" Version="5.3.1" />
  </ItemGroup>
</Project>
```

**Key Settings**:
- `OutputType>Exe` - Console application
- `TargetFramework>net10.0` - .NET 10.0
- `PublishSingleFile>true` - Single-file executable
- `SelfContained>false` - Framework-dependent (requires .NET runtime)
- `TrimMode>partial` - Trimming for size reduction

### Test Project (`.csproj`)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="NUnit" Version="3.14.0" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="NuciCLI" Version="3.0.1" />
    <PackageReference Include="NuciDAL" Version="3.0.1" />
    <PackageReference Include="NuciExtensions" Version="5.3.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\StellarisNameListGenerator\StellarisNameListGenerator.csproj" />
  </ItemGroup>
</Project>
```

## Publishing

### Framework-Dependent (Default)

```bash
dotnet publish -c Release
```

**Output**: `bin/Release/net10.0/publish/`
- `StellarisNameListGenerator` (executable, ~few MB)
- `.dll` files for dependencies
- Requires .NET 10.0 runtime on target machine

### Self-Contained (Single File)

```bash
# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained true

# Windows x64
dotnet publish -c Release -r win-x64 --self-contained true

# macOS x64
dotnet publish -c Release -r osx-x64 --self-contained true

# macOS ARM64
dotnet publish -c Release -r osx-arm64 --self-contained true
```

**Output**: Single executable (~60-80 MB) with embedded runtime.

### Trimmed Publishing

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishTrimmed=true
```

Reduces size by removing unused assemblies. May require `TrimmerRootAssembly` for reflection-heavy code (NuciDAL uses XML serialization).

## GitHub Actions CI/CD

### Workflow: `.github/workflows/dotnet.yml`

The repository uses a standard .NET workflow for CI. The workflow runs on push/PR to main branch, restores dependencies, builds, and runs tests.

### Workflow: `.github/workflows/github-release.yml`

The repository uses a GitHub Release workflow triggered on version tags (v*). It builds self-contained executables for Linux, Windows, and macOS, then creates a GitHub Release with all three platform binaries.

## Release Process

### Versioning

No automated versioning. Version managed via:
- Git tags: `v1.0.0`, `v1.1.0`, etc.
- Assembly version: Not set (uses default 1.0.0.0)
- Package version: N/A (not a NuGet package)

### Creating a Release

```bash
# 1. Ensure main is up to date
git checkout main
git pull

# 2. Create and push tag
git tag v1.0.0
git push origin v1.0.0

# 3. GitHub Actions builds and creates release automatically
```

### Release Artifacts

| Platform | Artifact Name | Size (approx) |
|----------|---------------|---------------|
| Linux x64 | `StellarisNameListGenerator` | 65 MB |
| Windows x64 | `StellarisNameListGenerator.exe` | 68 MB |
| macOS x64 | `StellarisNameListGenerator` | 65 MB |

## Installation (End User)

### Linux

```bash
# Download from GitHub Releases
wget https://github.com/owner/repo/releases/download/v1.0.0/StellarisNameListGenerator

# Make executable
chmod +x StellarisNameListGenerator

# Run
./StellarisNameListGenerator -i input.xml -o output.txt -n "My Names"
```

### Windows

```powershell
# Download from GitHub Releases
# Run directly
.\StellarisNameListGenerator.exe -i input.xml -o output.txt -n "My Names"
```

### macOS

```bash
# Download from GitHub Releases
# Remove quarantine attribute (if needed)
xattr -d com.apple.quarantine StellarisNameListGenerator

# Run
./StellarisNameListGenerator -i input.xml -o output.txt -n "My Names"
```

## Docker (Optional)

### Dockerfile

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["StellarisNameListGenerator/StellarisNameListGenerator.csproj", "StellarisNameListGenerator/"]
RUN dotnet restore "StellarisNameListGenerator/StellarisNameListGenerator.csproj"
COPY . .
WORKDIR "/src/StellarisNameListGenerator"
RUN dotnet publish -c Release -r linux-x64 --self-contained true -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["./StellarisNameListGenerator"]
```

### Build Image

```bash
docker build -t stellaris-namelist-generator .
```

### Run Container

```bash
docker run --rm -v $(pwd):/data stellaris-namelist-generator \
  -i /data/input.xml -o /data/output.txt -n "My Names"
```

## Platform Support

| Platform | Build Support | Runtime Support | Notes |
|----------|---------------|-----------------|-------|
| Linux x64 | ✅ | ✅ | Primary CI platform |
| Windows x64 | ✅ | ✅ | Self-contained .exe |
| macOS x64 | ✅ | ✅ | Intel Macs |
| macOS ARM64 | ✅ | ✅ | Apple Silicon |
| Linux ARM64 | ⚠️ | ⚠️ | Not in CI, but buildable |

## Build Output Locations

| Command | Output Directory |
|---------|------------------|
| `dotnet build` | `bin/Debug/net10.0/` or `bin/Release/net10.0/` |
| `dotnet publish` | `bin/Release/net10.0/publish/` |
| `dotnet publish -r linux-x64` | `bin/Release/net10.0/linux-x64/publish/` |
| GitHub Actions | `./publish/{linux-x64,win-x64,osx-x64}/` |

## Troubleshooting Build Issues

### NuGet Restore Fails

```bash
dotnet nuget locals all --clear
dotnet restore
```

### Build Fails with "Could not execute"

- Check .NET 10.0 SDK installed: `dotnet --list-sdks`
- Install if missing: `dotnet-install.sh` or package manager

### Publish Trimming Errors

Add to `.csproj` if reflection types are trimmed:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="NuciDAL" />
  <TrimmerRootAssembly Include="System.Xml" />
</ItemGroup>
```

### Test Discovery Fails

Ensure test project has:
- `<IsTestProject>true</IsTestProject>`
- NUnit package reference
- `dotnet test` discovers via `Microsoft.NET.Test.Sdk` (implicit)

## Performance

| Operation | Time (approx) |
|-----------|---------------|
| `dotnet restore` | 5-15s |
| `dotnet build` | 3-8s |
| `dotnet test` | 2-5s |
| `dotnet publish` (framework-dependent) | 5-10s |
| `dotnet publish` (self-contained, 3 platforms) | 30-60s |

## Artifacts Retention

- GitHub Actions: 90 days default (configurable)
- GitHub Releases: Permanent
- Local `bin/obj`: Manual cleanup (`dotnet clean`)