using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    /// <summary>
    /// Configures an option instance by using ConfigurationBinder.Bind against an IConfiguration.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to bind.</typeparam>
    public class ConfigureFromConfigurationOptions<TOptions> : IConfigureConfigurationValue<TOptions> where TOptions : class
    {
        private readonly IConfiguration _configuration;

        public ConfigureFromConfigurationOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TOptions Configure()
        {
           return  KVConfigurationBinder.Bind<TOptions>(_configuration);
        }
    }
}