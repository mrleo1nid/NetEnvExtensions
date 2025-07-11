using System;
using System.Linq;
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
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            // Создаём копию builder без EnvironmentVariableSubstitutionSource
            var filteredSources = builder
                .Sources.Where(s => !(s is EnvironmentVariableSubstitutionSource))
                .ToList();
            var tempBuilder = new ConfigurationBuilder();
            foreach (var src in filteredSources)
                tempBuilder.Add(src);
            var root = tempBuilder.Build();
            return new EnvironmentVariableSubstitutionProvider(root);
        }
    }
}
