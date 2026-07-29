# LogicBuilder.App.Utils

[![CI](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/ci.yml/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/ci.yml)
[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Utils/graph/badge.svg?token=IQCZ1TKERD)](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Utils)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.App.Utils&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.App.Utils)
[![NuGet](https://img.shields.io/nuget/v/LogicBuilder.App.Utils.svg)](https://www.nuget.org/packages/LogicBuilder.App.Utils)

## Overview

LogicBuilder.App.Utils is part of the [Logic Builder](https://github.com/BpsLogicBuilder/LogicBuilder) ecosystem and is a utility library that provides essential classes and helper methods used by business applications to perform routine operations. It maintains reusable functions which the logic builder workflow depends on.

## Purpose

This library simplifies common business application tasks by providing:
- **Reusable Components**: Common functionality needed across business applications
- **Parameter-to-Operator Conversion**: Transforms UI-generated parameters from `LogicBuilder.Forms.Parameters` into operator classes
- **LINQ Expression Generation**: Leverages `LogicBuilder.Expressions.Utils` to build type-safe LINQ expressions
- **Object Mapping**: Uses AutoMapper for efficient object-to-object mapping during the transformation process

## Features

- ✅ Targets .NET Standard 2.0 for broad compatibility
- ✅ Converts Logic Builder form parameters into executable operators
- ✅ Generates strongly-typed LINQ expressions for querying and filtering
- ✅ Built-in support for owned entity expansion via `OwnedEntityAttribute`
- ✅ Integrates seamlessly with the Logic Builder ecosystem

## Installation

Install via NuGet Package Manager:
- dotnet add package LogicBuilder.App.Utils

Or via Package Manager Console:
- Install-Package LogicBuilder.App.Utils

### Basic Setup

First, use the `AddAppUtilsServices()` to register dependencies:
```c#
services.AddAppUtilsServices(); 
```

## Dependencies

- **AutoMapper**
- **LogicBuilder.Attributes**
- **LogicBuilder.Expressions.Utils**
- **LogicBuilder.Forms.Parameters**
- **Microsoft.Extensions.Logging.Abstractions**
- **System.Reflection.Emit**
- **System.Text.Json**

## Use Cases

This library is designed for business applications that need to:

- Dynamically construct queries based on runtime criteria
- Generate CRUD operations without writing repetitive boilerplate code
- Build complex filtering, sorting, and projection expressions
- Maintain consistent data access patterns across application layers

### HttpClientHelper

A simplified wrapper around `HttpClient` that provides strongly-typed HTTP operations with built-in JSON serialization/deserialization.

**Key Benefits:**
- Integrates with `IHttpClientFactory` for proper HttpClient lifecycle management
- Automatic JSON serialization and deserialization
- Strongly-typed request and response handling
- Supports custom `JsonSerializerOptions` for flexible JSON configuration
- Throws `InvalidOperationException` on deserialization failures for robust error handling

## Related Projects

This library is part of the [LogicBuilder](https://github.com/BpsLogicBuilder/LogicBuilder) ecosystem.

## License

Copyright © BPS 2026

Licensed under the [MIT License](LICENSE).

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests to the [GitHub repository](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils).

## Support

For questions, issues, or feature requests, please visit the [Issues](https://github.com/BpsLogicBuilder/LogicBuilder.App.Utils/issues) page.