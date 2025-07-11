using System;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfigurationBuilder"/> to add support for environment variable substitution in configuration values.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds support for environment variable substitution in configuration values to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="regexTimeout">Optional timeout for the variable substitution regex. Default is 10 seconds.</param>
        /// <returns>The configuration builder with environment variable substitution support.</returns>
        public static IConfigurationBuilder AddEnvironmentVariableSubstitution(
            this IConfigurationBuilder builder,
            TimeSpan? regexTimeout = null
        )
        {
            return builder.Add(new EnvironmentVariableSubstitutionSource(regexTimeout));
        }
    }
}
