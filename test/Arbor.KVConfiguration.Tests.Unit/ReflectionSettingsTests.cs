using System;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Machine.Specifications;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class ReflectionSettingsTests
    {
        [Fact]
        public void LoadAssemblySettings()
        {
            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(NoConfiguration.Empty).AddReflectionSettings(AppDomain.CurrentDomain.GetAssemblies())
                .Build();

            multiSourceKeyValueConfiguration.ShouldNotBeNull();
        }
    }
}