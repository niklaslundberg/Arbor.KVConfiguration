﻿using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public class TimeSpanValidator : BaseValueValidator
    {
        public override bool CanValidate(string type) =>
            string.Equals("timespan", type, StringComparison.OrdinalIgnoreCase);

        protected override ImmutableArray<ValidationError> DoValidate(string type, string? value)
        {
            if (!TimeSpan.TryParse(value, out _))
            {
                return new ValidationError($"'{value}' is not a valid timespan value").ValueToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}