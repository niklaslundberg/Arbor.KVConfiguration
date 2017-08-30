using System;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class IntValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("int", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            if (!int.TryParse(value, out int parsedResult))
            {
                return new ValidationError("Not a valid integer value").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
