using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.Tests.Unit.Schema
{
    public static class DummyConfigurationConstants
    {
        [Metadata(isRequired: true)] public const string
            ADummyConfigurationKeyFieldName = "urn:a:dummy:key:field:constant:urn-value";
    }
}
