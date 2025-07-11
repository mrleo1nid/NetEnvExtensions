# NetEnvExtensions

[![Build status](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml/badge.svg )](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml )
![Tests](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/test.yml/badge.svg)
[![NuGet version](https://img.shields.io/nuget/v/NetEnvExtensions )](https://www.nuget.org/packages/NetEnvExtensions )

> 🇷🇺 Russian version: [README.ru.md](README.ru.md)

NetEnvExtensions is an extension for .NET that automatically substitutes environment variable values into your application configuration (e.g., from appsettings.json or other sources) using the syntax `${VAR_NAME}` or `${VAR_NAME:default}`.

## Features
- Supports environment variable substitution in any configuration value.
- Ability to specify default values: `${VAR_NAME:default}`.
- Simple integration with Microsoft.Extensions.Configuration.
- Supports loading environment variables from a `.env` file via [DotNetEnv](https://github.com/tonerdo/dotnet-env).

## Installation

Add the package to your project (example for NuGet):

```
dotnet add package NetEnvExtensions
```

## Usage

1. Import the namespace:

```csharp
using NetEnvExtensions;
```

2. Add the extension to your `IConfigurationBuilder`:

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();

var configuration = builder.Build();
```

3. Use environment variables in your appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_HOST:localhost};Port=${DB_PORT:5432};User Id=${DB_USER};Password=${DB_PASS}"
  }
}
```

If an environment variable is not defined, the default value will be used (if specified).

## Loading .env files

To automatically load variables from a `.env` file, use the `path` parameter in the extension:

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution(path: ".env");

var configuration = builder.Build();
```

> **Note:** The `EnvironmentLoader` class has been removed. Use the `path` parameter in `AddEnvironmentVariableSubstitution` to load `.env` files.

### Using LoadOptions.TraversePath()

If your project structure requires searching for a `.env` file in parent directories, you can use the `LoadOptions.TraversePath()` option from DotNetEnv:

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution(path: ".env", options: LoadOptions.TraversePath());

var configuration = builder.Build();
```

This will make DotNetEnv search for the `.env` file in the specified directory and all parent directories until it is found.

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.