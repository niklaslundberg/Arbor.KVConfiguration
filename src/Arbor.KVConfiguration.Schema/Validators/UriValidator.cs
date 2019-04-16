using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;

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
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                return new ValidationError($"'{value}' is not a valid URI").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
