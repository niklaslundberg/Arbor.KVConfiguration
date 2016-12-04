using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class SampleConfigurationConstants
    {
        [Metadata("urn")] public const string
            ATestKey = "urn:test:key";

        [Metadata("urn", defaultValue:"ThisIsDefault")] public const string
            AKeyWithDefaultValue = "urn:test:key:with:default:value";
    }
}
