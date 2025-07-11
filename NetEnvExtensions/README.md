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
DotNetEnv.Env.Load(); // Loads variables from .env into the process environment

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();

var configuration = builder.Build();
```

**Optional: Pass a logger for diagnostics**

If you want to log warnings about missing environment variables, you can pass an `ILogger` instance:

```csharp
using Microsoft.Extensions.Logging;

ILogger logger = ...; // Get or create an ILogger instance

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution(null, logger); // Pass logger as the second argument
```

This will log warnings if a variable is not found and no default is provided.

3. Use environment variables in your appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_HOST:localhost};Port=${DB_PORT:5432};User Id=${DB_USER};Password=${DB_PASS}"
  }
}
```

If an environment variable is not defined, the default value will be used (if specified).

> **Note:** If you want to load environment variables from a `.env` file, you can use a third-party library (e.g., [DotNetEnv](https://github.com/tonerdo/dotnet-env)) before building the configuration. This package does not include .env loading by default.

### Typical scenarios

- **Local development:** Use `.env` file and call `DotNetEnv.Env.Load()` before building the configuration.
- **Docker/Production:** Set environment variables via Docker or system, and do not call `DotNetEnv.Env.Load()`.

## Loading .env files

If you want to load environment variables from a `.env` file, you can use a third-party library such as [DotNetEnv](https://github.com/tonerdo/dotnet-env) before building the configuration:

```csharp
DotNetEnv.Env.Load(); // Loads variables from .env into the process environment

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();
```

## Known limitations

- **No recursion**: Variable substitution does not support nested or recursive variables (e.g., `${VAR_${NESTED}}` will not be resolved).
- **Flat variables only**: Only flat (non-hierarchical) environment variables are supported.
- **No .env loading by default**: You must use a third-party library (e.g., DotNetEnv) to load .env files.
- **Logging is optional**: Warnings about missing variables are logged only if an ILogger is provided explicitly or available in the configuration builder's services.
