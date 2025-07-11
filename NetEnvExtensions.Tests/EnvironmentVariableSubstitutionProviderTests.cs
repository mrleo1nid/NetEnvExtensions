using Microsoft.Extensions.Configuration;

namespace NetEnvExtensions.Tests
{
    public class EnvironmentVariableSubstitutionProviderTests
    {
        [Fact]
        public void SubstituteVariables_ReplacesEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("TEST_ENV", "value1");
            var dict = new Dictionary<string, string?> { { "Key1", "${TEST_ENV}" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key1", out var value));
            Assert.Equal("value1", value);
        }

        [Fact]
        public void SubstituteVariables_UsesDefaultValue_WhenEnvNotSet()
        {
            Environment.SetEnvironmentVariable("TEST_ENV2", null);
            var dict = new Dictionary<string, string?> { { "Key2", "${TEST_ENV2:default}" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key2", out var value));
            Assert.Equal("default", value);
        }

        [Fact]
        public void SubstituteVariables_LeavesValueUnchanged_IfNoPattern()
        {
            var dict = new Dictionary<string, string?> { { "Key3", "plainvalue" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key3", out var value));
            Assert.Equal("plainvalue", value);
        }

        [Fact]
        public void SubstituteVariables_EmptyString_ReturnsEmpty()
        {
            var dict = new Dictionary<string, string?> { { "Key4", "" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key4", out var value));
            Assert.Equal("", value);
        }

        [Fact]
        public void SubstituteVariables_MultipleVariablesInOneValue()
        {
            Environment.SetEnvironmentVariable("ENV1", "foo");
            Environment.SetEnvironmentVariable("ENV2", "bar");
            var dict = new Dictionary<string, string?> { { "Key5", "${ENV1}-${ENV2}" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key5", out var value));
            Assert.Equal("foo-bar", value);
        }

        [Fact]
        public void SubstituteVariables_UnknownVariableWithoutDefault_ReturnsEmpty()
        {
            Environment.SetEnvironmentVariable("NOT_SET", null);
            var dict = new Dictionary<string, string?> { { "Key6", "${NOT_SET}" } };
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("Key6", out var value));
            Assert.Equal("", value);
        }

        [Fact]
        public void SubstituteVariables_LongStringWithManyVariables()
        {
            // Arrange
            var dict = new Dictionary<string, string?>();
            var expected = "";
            var value = "";
            for (int i = 0; i < 1000; i++)
            {
                var varName = $"STRESS_{i}";
                Environment.SetEnvironmentVariable(varName, $"val{i}");
                value += $"${{{varName}}}-";
                expected += $"val{i}-";
            }
            dict["StressKey"] = value;
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            Assert.True(provider.TryGet("StressKey", out var result));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SubstituteVariables_LargeNumberOfConfigEntries()
        {
            // Arrange
            var dict = new Dictionary<string, string?>();
            for (int i = 0; i < 5000; i++)
            {
                var varName = $"BULK_{i}";
                Environment.SetEnvironmentVariable(varName, $"bulk{i}");
                dict[$"Key{i}"] = $"${{{varName}}}";
            }
            var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            var provider = new EnvironmentVariableSubstitutionProvider(config);
            provider.Load();
            for (int i = 0; i < 5000; i++)
            {
                Assert.True(provider.TryGet($"Key{i}", out var result));
                Assert.Equal($"bulk{i}", result);
            }
        }
    }
}
