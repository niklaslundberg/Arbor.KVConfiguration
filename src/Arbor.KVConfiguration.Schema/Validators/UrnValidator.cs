using System;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class UrnValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("urn", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                return new ValidationError("Invalid URN").ValueToImmutableArray();
            }

            bool parsed = Uri.TryCreate(value, UriKind.Absolute, out Uri parsedUri);

            if (!parsed || !parsedUri.IsAbsoluteUri
                || !parsedUri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationError("Invalid URN but valid URI").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
