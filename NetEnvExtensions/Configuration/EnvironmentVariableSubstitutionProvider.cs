using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    public class EnvironmentVariableSubstitutionProvider : ConfigurationProvider
    {
        private readonly IConfigurationRoot _root;

        public EnvironmentVariableSubstitutionProvider(IConfigurationRoot root)
        {
            _root = root;
        }

        public static IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root.")
            );
        }

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
