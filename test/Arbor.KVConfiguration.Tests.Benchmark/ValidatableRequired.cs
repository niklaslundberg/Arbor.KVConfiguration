using System.ComponentModel.DataAnnotations;
using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    [Urn(Urn)]
    public class ValidatableRequired
    {
        public const string Urn = "urn:required:type:with:validation";

        public ValidatableRequired(string name, int value)
        {
            Name = name;
            Value = value;
        }

        [Required] public string Name { get; }

        [Range(1, int.MaxValue)] public int Value { get; }

        public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
    }
}