using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class TimeSpanValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("timespan", type, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override IEnumerable<ValidationError> DoValidate(string type, string value)
        {
            TimeSpan parsedResult;

            if (!TimeSpan.TryParse(value, out parsedResult))
            {
                yield return new ValidationError("Not a valid timespan value");
            }
        }
    }
}