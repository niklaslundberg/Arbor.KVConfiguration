using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.Tests.Unit.Schema
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class DummyConfigurationConstants
    {
        [Metadata(isRequired: true)] public const string
            ADummyConfigurationKeyFieldName = "urn:a:dummy:key:field:constant:urn-value";
    }
}
