using System.Diagnostics.CodeAnalysis;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Behaviors]
    public class a_one_level_urn
    {
        protected static Primitives.Urn urn;

        private It should_have_original_value = () => urn.OriginalValue.ShouldNotBeEmpty();

        private It should_not_have_a_parent = () => Catch.Exception(() => urn.Parent).ShouldNotBeNull();
    }
}