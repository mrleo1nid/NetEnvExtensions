using System;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    /// <summary>
    /// Источник конфигурации, поддерживающий подстановку переменных окружения в значения.
    /// </summary>
    public class EnvironmentVariableSubstitutionSource : IConfigurationSource
    {
        /// <summary>
        /// Строит провайдер конфигурации с поддержкой подстановки переменных окружения.
        /// </summary>
        /// <param name="builder">Билдер конфигурации.</param>
        /// <returns>Провайдер конфигурации с поддержкой подстановки переменных окружения.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root.")
            );
        }
    }
}
