﻿using System;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class BoolValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            return string.Equals("bool", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            if (!bool.TryParse(value, out bool _))
            {
                return new ValidationError($"'{value}' is not a valid boolean value").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
