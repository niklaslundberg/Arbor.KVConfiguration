using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public interface IValueValidator
    {
        bool CanValidate([NotNull] string type);

        ImmutableArray<ValidationError> Validate([NotNull] string type, [CanBeNull] string value);
    }
}
