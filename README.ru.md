

# NetEnvExtensions

[![Build status](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml/badge.svg )](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml )
![Tests](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/test.yml/badge.svg)
[![NuGet version](https://img.shields.io/nuget/v/NetEnvExtensions )](https://www.nuget.org/packages/NetEnvExtensions )

> 🇬🇧 English version: [README.md](README.md)

NetEnvExtensions — это расширение для .NET, позволяющее автоматически подставлять значения переменных окружения в конфигурацию приложения (например, из appsettings.json или других источников), используя синтаксис `${VAR_NAME}` или `${VAR_NAME:default}`.

## Возможности
- Поддержка подстановки переменных окружения в любые значения конфигурации.
- Возможность указывать значения по умолчанию: `${VAR_NAME:default}`.
- Простая интеграция с Microsoft.Extensions.Configuration.
- Поддержка загрузки переменных окружения из файла `.env` через [DotNetEnv](https://github.com/tonerdo/dotnet-env).

## Установка

Добавьте пакет в ваш проект (пример для NuGet):

```
dotnet add package NetEnvExtensions
```

## Использование

1. Подключите пространство имён:

```csharp
using NetEnvExtensions;
```

2. Добавьте расширение в ваш IConfigurationBuilder:

```csharp
DotNetEnv.Env.Load(); // Загружает переменные из .env в окружение процесса

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();

var configuration = builder.Build();
```

3. Используйте переменные окружения в вашем appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_HOST:localhost};Port=${DB_PORT:5432};User Id=${DB_USER};Password=${DB_PASS}"
  }
}
```

Если переменная окружения не определена, будет использовано значение по умолчанию (если указано).

> **Примечание:** Если вам нужно подгружать переменные окружения из файла `.env`, используйте сторонние библиотеки (например, [DotNetEnv](https://github.com/tonerdo/dotnet-env)) до вызова AddEnvironmentVariableSubstitution. Этот пакет не включает загрузку .env по умолчанию.

## Загрузка .env файлов

Если вам нужно подгрузить переменные окружения из файла `.env`, используйте стороннюю библиотеку, например [DotNetEnv](https://github.com/tonerdo/dotnet-env), до вызова AddEnvironmentVariableSubstitution:

```csharp
DotNetEnv.Env.Load(); // Загружает переменные из .env в окружение процесса

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution();
```

## Лицензия

Проект распространяется под лицензией MIT. См. файл [LICENSE.txt](LICENSE.txt). 