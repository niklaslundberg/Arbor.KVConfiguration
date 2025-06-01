using System.Text.Json;
using Arbor.KVConfiguration.Core;
using static System.String;

namespace Arbor.KVConfiguration.Schema.SystemJson;

public static class JsonConfigurationSerializer
{
    public static ConfigurationItems Deserialize(string json)
    {
        if (IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(json));
        }

        return JsonSerializer.Deserialize<ConfigurationItems>(json) ??
               throw new InvalidOperationException(
                   $"Could not deserialize JSON to {nameof(ConfigurationItems)}, value is null");
    }

    public static string Serialize(ConfigurationItems configurationItems) => JsonSerializer.Serialize(
        configurationItems,
        new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        });
}