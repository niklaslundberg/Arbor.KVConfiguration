using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class BoolValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("bool", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override IEnumerable<ValidationError> DoValidate(string type, string value)
        {
            bool parsedResult;

            if (!bool.TryParse(value, out parsedResult))
            {
                yield return new ValidationError("Not a valid boolean value");
            }
        }
    }
}
