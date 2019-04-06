using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;

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
            if (!int.TryParse(value, out int _))
            {
                return new ValidationError($"'{value}' is not a valid integer value").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
