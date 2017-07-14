using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.InMemoryKeyValueConfiguration
{
    [Subject(typeof(MultipleValuesStringPair))]
    public class when_getting_default_instance
    {
        private static MultipleValuesStringPair instance;
        private Because of = () => instance = default(MultipleValuesStringPair);

        private It should_return_empty_values = () => instance.Values.ShouldBeEmpty();

        private It should_return_false_for_has_non_empty_value = () => instance.HasNonEmptyValue.ShouldBeFalse();
    }
}
