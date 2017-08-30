using System;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class UriValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("uri", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            if (!CanValidate(type))
            {
                throw new InvalidOperationException(
                    $"Could not validate type '{type}', make sure to call {nameof(CanValidate)} method first");
            }

            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                return new ValidationError("Invalid URI").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
