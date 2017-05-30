namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class UrnOpenGenericsManager<TOptions> : IConfigurationValue<TOptions> where TOptions : class
    {
        public UrnOpenGenericsManager(IConfigureConfigurationValue<TOptions> configurator)
        {
            Value = configurator.Configure();
        }

        public TOptions Value { get; }
    }
}
