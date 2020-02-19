namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [UsedImplicitly]
    public class ComplexChild
    {
        public ComplexChild(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; }

        public int Count { get; }
    }
}