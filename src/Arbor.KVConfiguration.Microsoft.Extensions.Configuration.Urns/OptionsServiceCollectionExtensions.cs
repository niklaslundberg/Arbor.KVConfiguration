using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for using options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration"></param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddKeyValueOptions(
            this IServiceCollection services,
            [NotNull] IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.TryAddSingleton(configuration);

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IConfigureConfigurationValue<>),
                typeof(ConfigureFromConfigurationOptions<>)));

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IConfigurationValue<>),
                typeof(UrnOpenGenericsManager<>)));

            return services;
        }
    }
}
