using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public static class ClassWithNestedClasses
    {
        [Metadata] public const string PlainKey = "urn:nested:plain";

        public static class NestedA
        {
            [Metadata] public const string NestedKeyA = "urn:nested:a";
        }

        public static class NestedB
        {
            [Metadata] public const string NestedKeyB = "urn:nested:b";
        }
    }
}