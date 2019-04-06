using System.Collections.Generic;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    [Optional]
    [Urn(Urn)]
    public class ValidatableOptional : IValidationObject
    {
        public const string Urn = "urn:optional:type:with:validation";

        public ValidatableOptional(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public int Value { get; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
        }

        public IEnumerable<ValidationError> Validate()
        {
            if (Name is null)
            {
                yield return new ValidationError("Name cannot be null");
            }
            else if (Name.Length == 0)
            {
                yield return new ValidationError("Name cannot be empty");
            }

            if (Value <= 0)
            {
                yield return new ValidationError($"Value cannot be 0 or negative, was {Value}");
            }
        }
    }
}
