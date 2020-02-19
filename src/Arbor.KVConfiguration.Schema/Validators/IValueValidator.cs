using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public interface IValueValidator
    {
        bool CanValidate([NotNull] string type);

        ImmutableArray<ValidationError> Validate([NotNull] string type, string? value);
    }
}