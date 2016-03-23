using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Behaviors]
    internal class two_unequal_urns
    {
        protected static Urns.Urn urn1;

        protected static Urns.Urn urn2;

        It should_not_be_equal_as_object = () => urn1.Equals((object)urn2).ShouldBeFalse();

        It should_not_be_equal_with_method = () => urn1.Equals(urn2).ShouldBeFalse();

        It should_not_be_equal_with_operator = () => (urn1 == urn2).ShouldBeFalse();

        It should_not_be_not_equal_with_not_operator = () => (urn1 != urn2).ShouldBeTrue();
    }
}