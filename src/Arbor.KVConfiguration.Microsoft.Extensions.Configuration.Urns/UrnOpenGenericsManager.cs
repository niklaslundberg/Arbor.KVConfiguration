using System.Collections.Generic;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
   public class UrnOpenGenericsManager<TOptions> : IConfigurationValue<TOptions> where TOptions : class
    {
        private readonly IConfigureConfigurationValue<TOptions> _configurator;

        public UrnOpenGenericsManager(IConfigureConfigurationValue<TOptions> configurator)
        {
            _configurator = configurator;
        }
    }
}