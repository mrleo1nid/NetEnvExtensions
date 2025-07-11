using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using NetEnvExtensions;
using Xunit;

namespace NetEnvExtensions.Tests
{
    public class EnvironmentVariableSubstitutionSourceTests
    {
        [Fact]
        public void Build_ReturnsProvider_AndSubstitutesVariables()
        {
            Environment.SetEnvironmentVariable("SRC_ENV", "src_value");
            var dict = new Dictionary<string, string?> { { "Key", "${SRC_ENV}" } };
            var builder = new ConfigurationBuilder().AddInMemoryCollection(dict);
            var source = new EnvironmentVariableSubstitutionSource();
            var provider = source.Build(builder);
            provider.Load();
            Assert.True(provider.TryGet("Key", out var value));
            Assert.Equal("src_value", value);
        }

        [Fact]
        public void Build_ThrowsIfBuilderIsNull()
        {
            var source = new EnvironmentVariableSubstitutionSource();
            Assert.Throws<ArgumentNullException>(() => source.Build(null!));
        }
    }
}
