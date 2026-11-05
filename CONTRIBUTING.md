# Contributing to Stellaris Name List Generator

This document covers the guidelines and processes for contributing to this project, including how to report issues, suggest enhancements, submit code changes, and follow project standards.

## 📑 Table of Contents

- [Contributing](#contributing)
- [Code of Conduct](#code-of-conduct)
- [Security](#security)
- [License](#license)
- [Getting Assistance](#getting-assistance)

## 🤝 Contributing

### Reporting Issues

- Search existing issues first.
- Use the issue templates if available.
- Provide clear reproduction steps.
- Include environment details.

### Suggesting Enhancements

- Check the existing discussions.
- Explain the use case and expected conduct.
- Consider the implementation complexity.

### Code Contributions

#### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

#### Development Setup

```bash
# Clone the repository
git clone https://github.com/hmlendea/stellaris-name-list-generator.git
cd stellaris-name-list-generator

# Install dependencies
dotnet restore
```

#### Making Changes

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes.
4. Run tests: `dotnet test --no-build --verbosity normal`
5. Run linters: `dotnet build --no-restore`
6. Commit with clear and descriptive messages.
7. Push to your fork.
8. Open a Pull Request.

#### Pull Request Guidelines

- Target the `master` branch.
- Keep PRs focused and atomic.
- Revise documentation if applicable.
- Include tests for any new functionality.
- Ensure the CI checks pass.

### Code Style

Follow the project's coding standards:
- Run `dotnet build --no-restore` before committing.

### Testing

```bash
# Run all tests
dotnet test --no-build --verbosity normal
```

### Documentation

- Revise relevant docs for changes.
- Follow the documentation style guide.
- Preview changes locally if possible.

## 📜 Code of Conduct

This project follows the [Code of Conduct](./CODE_OF_CONDUCT.md).

## 🔒 Security

Report security vulnerabilities per the [Security Policy](./SECURITY.md).

See the [Security Policy](./SECURITY.md) for details on supported versions, reporting procedures, scope, disclosure policy, safe harbour, and recognition.

## 📄 License

By contributing, you agree that your contributions will be licensed under the GNU General Public License v3.0 or later.

## ❓ Getting Assistance

- Check existing discussions and documentation first.