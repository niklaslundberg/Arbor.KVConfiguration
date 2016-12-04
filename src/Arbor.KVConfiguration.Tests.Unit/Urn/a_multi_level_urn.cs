using System.Diagnostics.CodeAnalysis;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Behaviors]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class a_multi_level_urn
    {
        protected static Urns.Urn urn;

        It should_have_a_parent = () => urn.Parent.ShouldNotBeNull();

        It should_have_original_value = () => urn.OriginalValue.ShouldNotBeEmpty();
    }
}
