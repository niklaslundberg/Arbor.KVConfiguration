namespace Arbor.KVConfiguration.Urns
{
    public interface INamedInstance<out T>
    {
        T Value { get; }

        string Name { get; }
    }
}