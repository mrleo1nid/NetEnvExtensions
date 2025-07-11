using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NetEnvExtensions
{
    /// <summary>
    /// A configuration source that supports environment variable substitution in values.
    /// </summary>
    public class EnvironmentVariableSubstitutionSource : IConfigurationSource
    {
        private readonly TimeSpan? _regexTimeout;
        private readonly ILogger? _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="EnvironmentVariableSubstitutionSource"/>.
        /// </summary>
        /// <param name="regexTimeout">Optional timeout for the variable substitution regex. Default is 10 seconds.</param>
        public EnvironmentVariableSubstitutionSource(TimeSpan? regexTimeout = null)
        {
            _regexTimeout = regexTimeout;
            _logger = null;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EnvironmentVariableSubstitutionSource"/> with logger.
        /// </summary>
        /// <param name="regexTimeout">Optional timeout for the variable substitution regex. Default is 10 seconds.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        public EnvironmentVariableSubstitutionSource(TimeSpan? regexTimeout, ILogger? logger)
        {
            _regexTimeout = regexTimeout;
            _logger = logger;
        }

        /// <summary>
        /// Builds a configuration provider with environment variable substitution support.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>A configuration provider with environment variable substitution support.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            // Create a copy of the builder without EnvironmentVariableSubstitutionSource
            var filteredSources = builder
                .Sources.Where(s => !(s is EnvironmentVariableSubstitutionSource))
                .ToList();
            var tempBuilder = new ConfigurationBuilder();
            foreach (var src in filteredSources)
                tempBuilder.Add(src);
            var root = tempBuilder.Build();
            return new EnvironmentVariableSubstitutionProvider(root, _regexTimeout, _logger);
        }
    }
}
