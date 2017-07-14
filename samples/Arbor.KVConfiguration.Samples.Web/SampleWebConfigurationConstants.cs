using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Samples.Web
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class SampleWebConfigurationConstants
    {
        [Metadata("urn")]
        public const string
            NoDefaultValue = "urn:test:key";

        [Metadata("urn", defaultValue: "ThisIsAnotherDefaultSourceValue")]
        public const string
            ATestKey = "urn:test:key:with:another:default:value";

        [Metadata("urn", defaultValue: "ThisIsTheDefaultSourceValue")]
        public const string
            AKeyWithDefaultValue = "urn:test:key:with:default:value";
    }
}
