# NetEnvExtensions

NetEnvExtensions — это расширение для .NET, позволяющее автоматически подставлять значения переменных окружения в конфигурацию приложения (например, из appsettings.json или других источников), используя синтаксис `${VAR_NAME}` или `${VAR_NAME:default}`.

## Возможности
- Поддержка подстановки переменных окружения в любые значения конфигурации.
- Возможность указывать значения по умолчанию: `${VAR_NAME:default}`.
- Простая интеграция с Microsoft.Extensions.Configuration.

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

Для автоматической загрузки переменных из файла `.env` используйте:

```csharp
NetEnvExtensions.EnvironmentLoader.LoadEnvironmentVariables();
```

## Лицензия

Проект распространяется под лицензией MIT. См. файл [LICENSE.txt](LICENSE.txt).