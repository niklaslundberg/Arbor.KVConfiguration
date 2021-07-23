using System;
using System.Collections.Generic;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.KVConfiguration.DependencyInjection
{
    public static class ConfigurationRegistrationExtensions
    {
        [PublicAPI]
        public static IServiceCollection AddConfigurationInstancesFromAssemblies(
            [NotNull] this IServiceCollection services,
            [NotNull] IKeyValueConfiguration keyValueConfiguration,
            params Assembly[] assemblies) =>
            AddConfigurationInstancesFromAssemblies(services, keyValueConfiguration, null, assemblies);

        [PublicAPI]
        public static IServiceCollection AddConfigurationInstancesFromAssemblies(
            [NotNull] this IServiceCollection services,
            [NotNull] IKeyValueConfiguration keyValueConfiguration,
            Action<Exception>? exceptionHandler,
            params Assembly[] assemblies)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (assemblies.Length == 0)
            {
                assemblies = new[] {Assembly.GetCallingAssembly()};
            }

            var configurationRegistrations = keyValueConfiguration.ScanRegistrations(exceptionHandler, assemblies);

            var configurationInstanceHolder = configurationRegistrations.CreateHolder();

            services.AddSingleton(configurationInstanceHolder);

            return services.AddConfigurationInstanceHolder(configurationInstanceHolder);
        }

        public static IServiceCollection AddConfigurationInstanceHolder(
            this IServiceCollection services,
            ConfigurationInstanceHolder holder)
        {
            foreach (Type holderRegisteredType in holder.RegisteredTypes)
            {
                var genericType = typeof(INamedInstance<>).MakeGenericType(holderRegisteredType);

                foreach (KeyValuePair<string, object> instance in holder.GetInstances(holderRegisteredType))
                {
                    services.Add(new ServiceDescriptor(holderRegisteredType,
                        provider => instance.Value,
                        ServiceLifetime.Transient));

                    var concreteGenericType = typeof(NamedInstance<>).MakeGenericType(holderRegisteredType);

                    services.Add(new ServiceDescriptor(genericType,
                        provider => Activator.CreateInstance(concreteGenericType,
                            instance.Value,
                            instance.Key) ?? throw new InvalidOperationException(
                            $"Could not create type {concreteGenericType.FullName}"),
                        ServiceLifetime.Transient));
                }
            }

            return services;
        }
    }
}