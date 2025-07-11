using System;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    /// <summary>
    /// A configuration source that supports environment variable substitution in values.
    /// </summary>
    public class EnvironmentVariableSubstitutionSource : IConfigurationSource
    {
        /// <summary>
        /// Builds a configuration provider with environment variable substitution support.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>A configuration provider with environment variable substitution support.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root.")
            );
        }
    }
}
