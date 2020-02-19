using System;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Schema.Validators
{
    public abstract class BaseValueValidator : IValueValidator
    {
        public abstract bool CanValidate(string type);

        public ImmutableArray<ValidationError> Validate(string type, string? value)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));
            }

            if (!CanValidate(type))
            {
                throw new InvalidOperationException(
                    $"Could not validate type '{type}', make sure to call {nameof(CanValidate)} method first");
            }

            return DoValidate(type, value);
        }

        protected abstract ImmutableArray<ValidationError> DoValidate(string type, string? value);
    }
}