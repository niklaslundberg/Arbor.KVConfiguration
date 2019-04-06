using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema.Validators;
using Arbor.Primitives;

namespace Arbor.KVConfiguration.Urns
{
    public class UrnValidator : BaseValueValidator
    {
        public override bool CanValidate(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));
            }

            return string.Equals("urn", type, StringComparison.OrdinalIgnoreCase);
        }

        protected override ImmutableArray<ValidationError> DoValidate(string type, string value)
        {
            if (value is null)
            {
                return new[] { new ValidationError("Value is null") }.ToImmutableArray();
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return new[] { new ValidationError("Value is not a valid URN, current value only contains white-spaces") }.ToImmutableArray();
            }

            if (!Urn.TryParse(value, out Urn _))
            {
                return new[] { new ValidationError($"'{value}' is not a valid URN") }.ToImmutableArray();
            }

            return ImmutableArray<ValidationError>.Empty;
        }
    }
}
