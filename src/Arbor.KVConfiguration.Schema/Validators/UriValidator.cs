using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class UriValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("uri", type, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override IEnumerable<ValidationError> DoValidate(string type, string value)
        {
            if (!CanValidate(type))
            {
                throw new InvalidOperationException(
                    $"Could not validate type '{type}', make sure to call {nameof(CanValidate)} method first");
            }

            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                yield return new ValidationError("Invalid URI");
            }
        }
    }
}
