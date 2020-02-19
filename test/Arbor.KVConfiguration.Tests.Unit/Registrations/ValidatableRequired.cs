using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    [Urn(Urn)]
    public class ValidatableRequired : IValidatableObject
    {
        public const string Urn = "urn:required:type:with:validation";

        public ValidatableRequired(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [Required] public string Name { get; }

        [Range(0, int.MaxValue)] public int Value { get; }

        public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == "fail")
            {
                yield return new ValidationResult("Name cannot be fail");
            }
        }
    }
}