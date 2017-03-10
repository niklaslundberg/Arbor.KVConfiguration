using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class IntValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("int", type, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override IEnumerable<ValidationError> DoValidate(string type, string value)
        {
            int parsedResult;
            if (!int.TryParse(value, out parsedResult))
            {
                yield return new ValidationError("Not a valid integer value");
            }
        }
    }
}