using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class UrnValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("urn", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override IEnumerable<ValidationError> DoValidate(string type, string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                yield return new ValidationError("Invalid URN");
            }
            else
            {
                Uri parsedUri;

                bool parsed = Uri.TryCreate(value, UriKind.Absolute, out parsedUri);

                if (!parsed || !parsedUri.IsAbsoluteUri
                    || !parsedUri.Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
                {
                    yield return new ValidationError("Invalid URN but valid URI");
                }
            }
        }
    }
}
