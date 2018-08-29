namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Configuration value holder
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigurationValue<out TOptions> where TOptions : class
    {
        TOptions Value { get; }
    }
}
