using System;
using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    public class EnvironmentVariableSubstitutionSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentVariableSubstitutionProvider(
                builder.Build()
                    ?? throw new InvalidOperationException("Failed to build configuration root.")
            );
        }
    }
}
