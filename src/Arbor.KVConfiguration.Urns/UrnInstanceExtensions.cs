using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnInstanceExtensions
    {
        public static ConfigurationRegistrations ScanRegistrations(
            this IKeyValueConfiguration keyValueConfiguration,
            params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            ImmutableArray<UrnTypeMapping> urnTypesInAssemblies = UrnTypes.GetUrnTypesInAssemblies(assemblies);

            IEnumerable<UrnTypeRegistration> configurationInstanceHolders =
                urnTypesInAssemblies.SelectMany(urnType => GetInstancesForType(keyValueConfiguration, urnType.Type));

            return new ConfigurationRegistrations(configurationInstanceHolders.ToImmutableArray());
        }

        public static ConfigurationRegistrations GetRegistrations(
            this IKeyValueConfiguration keyValueConfiguration,
            Type type)
        {
            IEnumerable<UrnTypeRegistration> configurationInstanceHolders =
                GetInstancesForType(keyValueConfiguration, type);

            return new ConfigurationRegistrations(configurationInstanceHolders.ToImmutableArray());
        }

        public static ConfigurationInstanceHolder CreateHolder(this ConfigurationRegistrations configurationRegistrations)
        {
            var configurationInstanceHolder = new ConfigurationInstanceHolder();
            foreach (UrnTypeRegistration registration in configurationRegistrations.UrnTypeRegistrations)
            {
                configurationInstanceHolder.Add(registration.Instance);
            }

            return configurationInstanceHolder;
        }

        public static ImmutableArray<UrnTypeRegistration> GetInstancesForType(
            IKeyValueConfiguration keyValueConfiguration,
            Type type)
        {
            var urnAttribute = type.GetCustomAttribute<UrnAttribute>();

            if (urnAttribute is null)
            {
                throw new InvalidOperationException(
                    $"Type {type.FullName} does not have an {typeof(UrnAttribute).FullName} attribute");
            }

            ImmutableArray<INamedInstance<object>> instances = keyValueConfiguration.GetNamedInstances(type);

            var urnTypeMapping = new UrnTypeMapping(type, urnAttribute.Urn);

            if (instances.IsDefaultOrEmpty)
            {
                var optionalAttribute = type.GetCustomAttribute<OptionalAttribute>();

                if (optionalAttribute != null)
                {
                    return ImmutableArray<UrnTypeRegistration>.Empty;
                }


                ImmutableArray<UrnTypeRegistration> urnTypeRegistrations = new[]
                {
                    new UrnTypeRegistration(
                        urnTypeMapping,
                        null,
                        new ConfigurationRegistrationError($"Could not get any instance of type {type.FullName}"))
                }.ToImmutableArray();

                return urnTypeRegistrations;
            }

            ImmutableArray<INamedInstance<IValidationObject>> validationObjects = instances
                .OfType<INamedInstance<IValidationObject>>()
                .Where(item => item != null)
                .ToImmutableArray();

            if (!validationObjects.IsDefaultOrEmpty && validationObjects.Length > 0
                                                    && validationObjects.Any(validatedObject =>
                                                        validatedObject.Value.Validate().Any()))
            {
                ImmutableArray<UrnTypeRegistration> urnTypeRegistrations = validationObjects
                    .Select(validationObject =>
                    {
                        ValidationError[] errors = validationObject.Value.Validate().ToArray();

                        if (errors.Length == 0)
                        {
                            return new UrnTypeRegistration(urnTypeMapping, validationObject);
                        }

                        return new UrnTypeRegistration(urnTypeMapping,
                            validationObject,
                            errors.Select(e => new ConfigurationRegistrationError(e.ErrorMessage))
                                .ToArray());
                    })
                    .ToImmutableArray();

                return urnTypeRegistrations;
            }

            ImmutableArray<INamedInstance<IValidationObject>> validInstances = validationObjects
                .Where(validationObject => validationObject.Value.Validate().Any())
                .ToImmutableArray();

            if (!validInstances.IsDefaultOrEmpty && validInstances.Length == 1)
            {
                INamedInstance<IValidationObject> validationObject = validInstances.Single();

                return new[]
                {
                    new UrnTypeRegistration(
                        urnTypeMapping,
                        validationObject)
                }.ToImmutableArray();
            }

            if (!validInstances.IsDefaultOrEmpty && validInstances.Length > 1)
            {
                return validInstances
                    .Select(item => new UrnTypeRegistration(urnTypeMapping, item))
                    .ToImmutableArray();
            }

            if (!validationObjects.IsDefaultOrEmpty)
            {
                return validInstances
                    .Select(item => new UrnTypeRegistration(urnTypeMapping,
                        item,
                        new ConfigurationRegistrationError($"Invalid instance {item}")))
                    .ToImmutableArray();
            }


            return instances
                .Select(item => new UrnTypeRegistration(urnTypeMapping, item))
                .ToImmutableArray();
        }
    }
}
