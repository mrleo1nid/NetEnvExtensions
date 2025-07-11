using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetEnvExtensions
{
    /// <summary>
    /// A configuration provider that performs environment variable substitution in configuration values.
    /// </summary>
    public class EnvironmentVariableSubstitutionProvider : ConfigurationProvider
    {
        private readonly IConfigurationRoot _root;
        private readonly Regex _variablePattern;
        private readonly ILogger? _logger;

        /// <summary>
        /// Creates a new instance of <see cref="EnvironmentVariableSubstitutionProvider"/>.
        /// </summary>
        /// <param name="root">The configuration root to search for variables.</param>
        /// <param name="regexTimeout">Optional timeout for the variable substitution regex. Default is 10 seconds.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        public EnvironmentVariableSubstitutionProvider(
            IConfigurationRoot root,
            TimeSpan? regexTimeout = null,
            ILogger? logger = null
        )
        {
            _root = root;
            _variablePattern = new Regex(
                @"\$\{([^:}]+)(?::([^}]*))?\}",
                RegexOptions.Compiled,
                regexTimeout ?? TimeSpan.FromSeconds(10)
            );
            _logger = logger;
        }

        /// <summary>
        /// Builds an environment variable substitution provider for the specified configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>A configuration provider with environment variable substitution support.</returns>
        public static IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            // Try to get ILogger<EnvironmentVariableSubstitutionProvider> from builder.Services if available
            ILogger? logger = null;
#if NET6_0_OR_GREATER
            var services =
                (builder as IConfigurationBuilder)
                    ?.GetType()
                    .GetProperty("Services")
                    ?.GetValue(builder) as IServiceProvider;
            if (services != null)
            {
                var loggerType = typeof(ILogger<>).MakeGenericType(
                    typeof(EnvironmentVariableSubstitutionProvider)
                );
                logger = services.GetService(loggerType) as ILogger;
            }
#endif
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root."),
                null,
                logger
            );
        }

        /// <summary>
        /// Loads the configuration values with environment variable substitution applied.
        /// </summary>
        public override void Load()
        {
            var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            foreach (var child in _root.AsEnumerable())
            {
                if (child.Value != null && child.Value.Contains("${"))
                {
                    data[child.Key] = SubstituteVariables(child.Value);
                }
                else
                {
                    data[child.Key] = child.Value;
                }
            }

            Data = data;
        }

        private string SubstituteVariables(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.Contains("${"))
                return value;

            return _variablePattern.Replace(
                value,
                match =>
                {
                    var variableName = match.Groups[1].Value;
                    var defaultValue = match.Groups[2].Success
                        ? match.Groups[2].Value
                        : string.Empty;

                    var envValue = Environment.GetEnvironmentVariable(variableName);
                    if (envValue == null && string.IsNullOrEmpty(defaultValue))
                    {
                        _logger?.LogWarning(
                            "Environment variable '{VariableName}' not found and no default value provided.",
                            variableName
                        );
                    }
                    return envValue ?? defaultValue;
                }
            );
        }
    }
}
