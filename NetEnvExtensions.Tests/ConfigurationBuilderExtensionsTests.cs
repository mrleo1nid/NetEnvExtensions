using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NetEnvExtensions;
using Xunit;

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

        [Fact]
        public void AddEnvironmentVariableSubstitution_AddsDotNetEnvAndCustomSource()
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariableSubstitution();
            var sources = builder.Sources.ToList();
            Assert.Contains(sources, s => s.GetType().Name.Contains("EnvConfigurationSource"));
            Assert.Contains(sources, s => s is EnvironmentVariableSubstitutionSource);
        }

        [Fact]
        public void AddEnvironmentVariableSubstitution_LoadsVariablesFromDotEnvFile()
        {
            // Arrange: создаём временный .env-файл
            var tempEnvPath = Path.GetTempFileName();
            File.WriteAllText(tempEnvPath, "DOTENV_TEST_VAR=dotenv_value");
            try
            {
                var dict = new Dictionary<string, string?> { { "Key", "${DOTENV_TEST_VAR}" } };
                var builder = new ConfigurationBuilder().AddInMemoryCollection(dict);
                builder.AddEnvironmentVariableSubstitution(path: tempEnvPath);
                var config = builder.Build();
                Assert.Equal("dotenv_value", config["Key"]);
            }
            finally
            {
                File.Delete(tempEnvPath);
            }
        }
    }
}
