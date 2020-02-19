using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Registrations
{
    [Optional]
    [Urn(Urn)]
    public class ValidatableOptional
    {
        public const string Urn = "urn:optional:type:with:validation";

        public ValidatableOptional(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [Required] public string Name { get; }

        [Range(1, int.MaxValue)] public int Value { get; }

        public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
    }
}