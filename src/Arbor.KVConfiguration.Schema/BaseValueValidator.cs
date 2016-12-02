using System;
using System.Collections.Generic;

namespace Arbor.KVConfiguration.Schema
{
    public abstract class BaseValueValidator : IValueValidator
    {
        protected abstract IEnumerable<ValidationError> DoValidate(string type, string value);
        public abstract bool CanValidate(string type);

        public IEnumerable<ValidationError> Validate(string type, string value)
        {
            if (!CanValidate(type))
            {
                throw new InvalidOperationException(
                    $"Could not validate type '{type}', make sure to call {nameof(CanValidate)} method first");
            }

            return DoValidate(type, value);
        }
    }
}