using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEnvironmentVariableSubstitution(
            this IConfigurationBuilder builder
        )
        {
            return builder.Add(new EnvironmentVariableSubstitutionSource());
        }
    }
}
