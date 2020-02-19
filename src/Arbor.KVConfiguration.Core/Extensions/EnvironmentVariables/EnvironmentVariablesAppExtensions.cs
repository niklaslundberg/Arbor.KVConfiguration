using System.Collections.Generic;
using System.Collections.Specialized;

namespace Arbor.KVConfiguration.Core.Extensions.EnvironmentVariables
{
    public static class EnvironmentVariablesAppExtensions
    {
        public static AppSettingsBuilder AddEnvironmentVariables(
            this AppSettingsBuilder builder,
            IReadOnlyDictionary<string, string> environmentVariables)
        {
            if (environmentVariables is null)
            {
                return builder;
            }

            var nameValueCollection = new NameValueCollection();

            foreach (var environmentVariable in environmentVariables)
            {
                nameValueCollection.Add(environmentVariable.Key, environmentVariable.Value);
            }

            return builder.Add(new InMemoryKeyValueConfiguration(nameValueCollection));
        }
    }
}