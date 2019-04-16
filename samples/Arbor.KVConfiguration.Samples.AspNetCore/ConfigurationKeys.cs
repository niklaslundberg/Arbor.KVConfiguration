using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Samples.AspNetCore
{
    public static class ConfigurationKeys
    {
        public const string DummyKey = "urn:vnd:my-sample-key";

        [Metadata("int", defaultValue: "12")]
        public const string SingleValue = "urn:vnc:single-int-value";
    }
}
