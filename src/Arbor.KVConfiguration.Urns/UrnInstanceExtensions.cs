﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
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

        public static ConfigurationInstanceHolder CreateHolder(
            this ConfigurationRegistrations configurationRegistrations)
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

                if (optionalAttribute is object _)
                {
                    return ImmutableArray<UrnTypeRegistration>.Empty;
                }

                ImmutableArray<UrnTypeRegistration> urnTypeRegistrations = new[]
                {
                    new UrnTypeRegistration(
                        urnTypeMapping,
                        null,
                        new ValidationResult($"Could not get any instance of type {type.FullName}"))
                }.ToImmutableArray();

                return urnTypeRegistrations;
            }

            (INamedInstance<object> Object, bool Valid, ValidationResult[] Errors)[] validatedObjects = instances
                .Select(
                    instance =>
                        (Object: instance,
                            Valid: DataAnnotationsValidator.TryValidate(instance.Value,
                                out ImmutableArray<ValidationResult> details),
                            Errors: details.ToArray()))
                .ToArray();

            if (validatedObjects.Any(pair => pair.Errors.Length > 0))
            {
                ImmutableArray<UrnTypeRegistration> urnTypeRegistrations = validatedObjects
                    .Select(validationObject =>
                    {
                        ValidationResult[] errors = validationObject.Errors;

                        if (errors.Length == 0)
                        {
                            return new UrnTypeRegistration(urnTypeMapping, validationObject.Object);
                        }

                        return new UrnTypeRegistration(urnTypeMapping,
                            validationObject.Object,
                            errors);
                    })
                    .ToImmutableArray();

                return urnTypeRegistrations;
            }

            return instances
                .Select(instance => new UrnTypeRegistration(urnTypeMapping, instance))
                .ToImmutableArray();
        }
    }
}
