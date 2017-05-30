namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public interface IConfigurationValue<out TOptions> where TOptions : class
    {
        TOptions Value { get; }
    }
}
