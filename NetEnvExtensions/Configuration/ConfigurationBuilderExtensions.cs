using DotNetEnv;
using DotNetEnv.Configuration;
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
        /// <param name="path">Optional path to the .env file to load environment variables from.</param>
        /// <param name="options">Optional load options for DotNetEnv.</param>
        /// <returns>The configuration builder with environment variable substitution support.</returns>
        public static IConfigurationBuilder AddEnvironmentVariableSubstitution(
            this IConfigurationBuilder builder,
            string? path = null,
            LoadOptions? options = null
        )
        {
            builder.AddDotNetEnv(path, options);
            return builder.Add(new EnvironmentVariableSubstitutionSource());
        }
    }
}
