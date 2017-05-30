using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Arbor.KVConfiguration.Core.Extensions;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by reflection")]
    public class ConfigurationMetadata
    {
        public ConfigurationMetadata(
            string key,
            string valueType,
            string memberName = "",
            string description = "",
            string partInvariantName = "",
            string partFullName = "",
            [CanBeNull] Type containingClass = null,
            int sourceLine = -1,
            string sourceFile = "",
            bool isRequired = true,
            string defaultValue = "",
            string notes = "",
            bool allowMultipleValues = false,
            [CanBeNull] IEnumerable<string> examples = null,
            [CanBeNull] IEnumerable<string> tags = null,
            string keyType = "")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(key));
            }

            Key = key;
            MemberName = memberName;
            Description = description;
            ValueType = valueType;
            PartInvariantName = partInvariantName;
            PartFullName = partFullName;
            ContainingClass = containingClass;
            SourceLine = sourceLine;
            SourceFile = sourceFile;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            Notes = notes;
            AllowMultipleValues = allowMultipleValues;
            KeyType = keyType;
            Examples = examples.SafeToImmutableArray();
            Tags = tags.SafeToImmutableArray();
        }

        public bool AllowMultipleValues { get; }
        public string KeyType { get; }

        public Type ContainingClass { get; }

        public string DefaultValue { get; }

        public string Description { get; }

        public ImmutableArray<string> Examples { get; }

        public bool IsRequired { get; }

        public string Key { get; }

        public string MemberName { get; }

        public string Notes { get; }

        public string PartFullName { get; }

        public string PartInvariantName { get; }

        public string SourceFile { get; }

        public int SourceLine { get; }

        public ImmutableArray<string> Tags { get; }

        public string ValueType { get; }

        public override string ToString()
        {
            return $"[{nameof(Key)}: {Key}] [{nameof(ValueType)}: {ValueType}]";
        }
    }
}
