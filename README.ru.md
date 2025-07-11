

# NetEnvExtensions

[![Build status](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml/badge.svg )](https://github.com/mrleo1nid/NetEnvExtensions/actions/workflows/nuget.yml )
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

2. Добавьте расширение в ваш `IConfigurationBuilder`:

```csharp
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

## Загрузка .env файлов

Для автоматической загрузки переменных из файла `.env` используйте параметр `path` в расширении:

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution(path: ".env");

var configuration = builder.Build();
```

> **Внимание:** Класс `EnvironmentLoader` был удалён. Для загрузки `.env` файлов используйте параметр `path` в методе `AddEnvironmentVariableSubstitution`.

### Использование LoadOptions.TraversePath()

Если структура вашего проекта требует поиска файла `.env` в родительских директориях, используйте опцию `LoadOptions.TraversePath()` из DotNetEnv:

```csharp
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariableSubstitution(path: ".env", options: LoadOptions.TraversePath());

var configuration = builder.Build();
```

Это позволит DotNetEnv искать файл `.env` в указанной директории и всех родительских директориях, пока файл не будет найден.

## Лицензия

Проект распространяется под лицензией MIT. См. файл [LICENSE.txt](LICENSE.txt). 