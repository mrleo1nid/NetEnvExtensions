using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void AddEnvironmentVariableSubstitution_AddsProvider_AndSubstitutesVariables()
        {
            Environment.SetEnvironmentVariable("EXT_ENV", "ext_value");
            var dict = new Dictionary<string, string?> { { "Key", "${EXT_ENV}" } };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dict);
            builder.AddEnvironmentVariableSubstitution();
            var config = builder.Build();
            Assert.Equal("ext_value", config["Key"]);
        }
    }
}
