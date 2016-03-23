using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Behaviors]
    public class a_one_level_urn
    {
        protected static Urns.Urn urn;

        It should_have_original_value = () => urn.OriginalValue.ShouldNotBeEmpty();

        It should_not_have_a_parent = () => Catch.Exception(() => urn.Parent).ShouldNotBeNull();
    }
}
