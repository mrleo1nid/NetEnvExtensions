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

- **Local development:** Use `.env` file and set `useDotNetEnvOnlyDevelopment: true` (default).
- **Docker/Production:** Set environment variables via Docker or system, and use `useDotNetEnvOnlyDevelopment: false` to skip loading `.env`.

## Loading .env files

If you want to load environment variables from a `.env` file, you can use a third-party library such as [DotNetEnv](https://github.com/tonerdo/dotnet-env) before building the configuration:

```csharp
DotNetEnv.Env.Load(); // Loads variables from .env into the process environment

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();
```
