using System;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class TimeSpanValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("timespan", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            TimeSpan parsedResult;

            if (!TimeSpan.TryParse(value, out parsedResult))
            {
                return new ValidationError("Not a valid timespan value").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
