namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Represents something that configures the TOptions type.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigureConfigurationValue<TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        TOptions Configure();
    }
}