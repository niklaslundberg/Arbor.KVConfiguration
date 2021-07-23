using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Arbor.KVConfiguration.Core.Extensions;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Metadata
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MetadataAttribute : Attribute
    {
        public MetadataAttribute(
            string valueType = "",
            string memberName = "",
            string description = "",
            string partInvariantName = "",
            string partFullName = "",
            bool isRequired = true,
            string defaultValue = "",
            string notes = "",
            bool allowMultipleValues = false,
            string[]? examples = null,
            string[]? tags = null,
            string keyType = "",
            [CallerLineNumber] int sourceLine = -1,
            [CallerFilePath] string sourceFile = "")
        {
            MemberName = memberName ?? "";
            Description = description ?? "";
            ValueType = valueType ?? "";
            PartInvariantName = partInvariantName ?? "";
            PartFullName = partFullName ?? "";
            SourceLine = sourceLine;
            SourceFile = sourceFile ?? "";
            IsRequired = isRequired;
            DefaultValue = defaultValue ?? "";
            Notes = notes ?? "";
            AllowMultipleValues = allowMultipleValues;
            KeyType = keyType ?? "";
            Examples = examples!.SafeToImmutableArray();
            Tags = tags!.SafeToImmutableArray();
        }

        public bool AllowMultipleValues { get; }

        public string KeyType { get; }

        public string DefaultValue { get; }

        public string Description { get; }

        public ImmutableArray<string> Examples { get; }

        public bool IsRequired { get; }

        public string MemberName { get; }

        public string Notes { get; }

        public string PartFullName { get; }

        public string PartInvariantName { get; }

        public string SourceFile { get; }

        public int SourceLine { get; }

        public ImmutableArray<string> Tags { get; }

        public string ValueType { get; }

        public override string ToString() => $"[{nameof(MemberName)}: {MemberName}] [{nameof(ValueType)}: {ValueType}]";
    }
}