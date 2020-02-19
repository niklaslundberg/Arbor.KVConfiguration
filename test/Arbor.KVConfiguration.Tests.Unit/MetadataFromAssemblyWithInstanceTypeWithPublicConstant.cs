using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class MetadataFromAssemblyWithInstanceTypeWithPublicConstant
    {
        [Metadata] public const string AbcConstantsInstance = "123";

        [Fact]
        public void Do()
        {
            ImmutableArray<ConfigurationMetadata> metadataFromAssemblyTypes =
                GetType().Assembly.GetMetadataFromAssemblyTypes();

            Assert.Contains(metadataFromAssemblyTypes,
                metadata => metadata.MemberName.Equals(nameof(AbcConstantsInstance)) && metadata.Key.Equals("123"));
        }
    }
}