using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    /// <summary>
    /// Предоставляет методы расширения для <see cref="IConfigurationBuilder"/>, добавляющие поддержку подстановки переменных окружения.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Добавляет в билдер конфигурации поддержку подстановки переменных окружения в значения.
        /// </summary>
        /// <param name="builder">Билдер конфигурации.</param>
        /// <returns>Билдер конфигурации с поддержкой подстановки переменных окружения.</returns>
        public static IConfigurationBuilder AddEnvironmentVariableSubstitution(
            this IConfigurationBuilder builder
        )
        {
            return builder.Add(new EnvironmentVariableSubstitutionSource());
        }
    }
}
