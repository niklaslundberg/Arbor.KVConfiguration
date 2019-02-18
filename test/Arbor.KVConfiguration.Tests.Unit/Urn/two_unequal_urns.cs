using System.Diagnostics.CodeAnalysis;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
    [Behaviors]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class two_unequal_urns
    {
        protected static Arbor.Primitives.Urn urn1;

        protected static Arbor.Primitives.Urn urn2;

        private It should_not_be_equal_as_object = () => urn1.Equals((object)urn2).ShouldBeFalse();

        private It should_not_be_equal_with_method = () => urn1.Equals(urn2).ShouldBeFalse();

        private It should_not_be_equal_with_operator = () => (urn1 == urn2).ShouldBeFalse();

        private It should_not_be_not_equal_with_not_operator = () => (urn1 != urn2).ShouldBeTrue();
    }
}
