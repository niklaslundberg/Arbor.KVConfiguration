using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfigurationDecorator
    {
        ImmutableArray<string> GetAllKeys(IKeyValueConfiguration keyValueConfiguration);

        string GetValue([NotNull] string value);

        ImmutableArray<StringPair> GetAllValues([NotNull] IKeyValueConfiguration keyValueConfiguration);

        ImmutableArray<MultipleValuesStringPair> GetAllWithMultipleValues(
            [NotNull] IKeyValueConfiguration keyValueConfiguration);
    }
}
