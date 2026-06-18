# LogicBuilder.App.Utils

[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Utils/graph/badge.svg?token=IQCZ1TKERD)](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Utils)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.App.Utils&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.App.Utils)

## Overview

LogicBuilder.App.Utils is a utility library that provides essential classes and helper methods used by business applications to perform routine operations. This library focuses on creating CRUD-related LINQ queries dynamically from data, enabling flexible and efficient data access patterns.

## Purpose

This library simplifies common business application tasks by providing:
- **Reusable Components**: Common functionality needed across business applications

## Features

- ✅ Targets .NET Standard 2.0 for broad compatibility
- ✅ Strong-named assembly for enterprise environments
- ✅ Includes source link support for debugging
- ✅ Built-in support for owned entity expansion via `OwnedEntityAttribute`
- ✅ Nullable reference types enabled for improved type safety

## Installation

Install via NuGet Package Manager:
- dotnet add package LogicBuilder.App.Utils

Or via Package Manager Console:
- Install-Package LogicBuilder.App.Utils

## Dependencies

- **LogicBuilder.Attributes** (v2.0.6)
- **Microsoft.Extensions.Logging.Abstractions** (v10.0.9)
- **System.Reflection.Emit** (v4.7.0)
- **System.Text.Json** (v10.0.9)

## Use Cases

This library is designed for business applications that need to:

- Dynamically construct queries based on runtime criteria
- Generate CRUD operations without writing repetitive boilerplate code
- Build complex filtering, sorting, and projection expressions
- Maintain consistent data access patterns across application layers

## Related Projects

This library is part of the [LogicBuilder](https://github.com/BpsLogicBuilder/LogicBuilder) ecosystem.

## License

Copyright © BPS 2026

Licensed under the [MIT License](LICENSE).

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests to the [GitHub repository](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils).

## Support

For questions, issues, or feature requests, please visit the [Issues](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/issues) page.