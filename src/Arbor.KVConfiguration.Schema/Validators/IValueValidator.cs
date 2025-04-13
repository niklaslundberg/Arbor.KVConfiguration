using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema.Validators;

public interface IValueValidator
{
    bool CanValidate(string type);

    ImmutableArray<ValidationError> Validate(string type, string? value);
}