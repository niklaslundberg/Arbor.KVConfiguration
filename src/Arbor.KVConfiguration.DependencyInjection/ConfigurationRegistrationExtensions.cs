using System;
using System.Collections.Generic;
using System.Reflection;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.KVConfiguration.DependencyInjection
{
    public static class ConfigurationRegistrationExtensions
    {
        public static IServiceCollection AddConfigurationInstanceHolder(
            this IServiceCollection services,
            ConfigurationInstanceHolder holder)
        {
            foreach (Type holderRegisteredType in holder.RegisteredTypes)
            {
                Type genericType = typeof(INamedInstance<>).MakeGenericType(holderRegisteredType);

                foreach (KeyValuePair<string, object> instance in holder.GetInstances(holderRegisteredType))
                {
                    services.Add(new ServiceDescriptor(holderRegisteredType,
                        provider => instance.Value,
                        ServiceLifetime.Transient));

                    Type concreteGenericType = typeof(NamedInstance<>).MakeGenericType(holderRegisteredType);

                    services.Add(new ServiceDescriptor(genericType,
                        provider => Activator.CreateInstance(concreteGenericType,
                            instance.Value,
                            instance.Key),
                        ServiceLifetime.Transient));
                }
            }

            return services;
        }
    }
}
