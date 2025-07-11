using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    /// <summary>
    /// A configuration provider that performs environment variable substitution in configuration values.
    /// </summary>
    public class EnvironmentVariableSubstitutionProvider : ConfigurationProvider
    {
        private readonly IConfigurationRoot _root;

        /// <summary>
        /// Creates a new instance of <see cref="EnvironmentVariableSubstitutionProvider"/>.
        /// </summary>
        /// <param name="root">The configuration root to search for variables.</param>
        public EnvironmentVariableSubstitutionProvider(IConfigurationRoot root)
        {
            _root = root;
        }

        /// <summary>
        /// Builds an environment variable substitution provider for the specified configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>A configuration provider with environment variable substitution support.</returns>
        public static IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root.")
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

        private static string SubstituteVariables(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.Contains("${"))
                return value;

            return VariablePattern.Replace(
                value,
                match =>
                {
                    var variableName = match.Groups[1].Value;
                    var defaultValue = match.Groups[2].Success
                        ? match.Groups[2].Value
                        : string.Empty;

                    var envValue = Environment.GetEnvironmentVariable(variableName);
                    return envValue ?? defaultValue;
                }
            );
        }

        private static readonly Regex VariablePattern = new Regex(
            @"\$\{([^:}]+)(?::([^}]*))?\}",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(10)
        );
    }
}
