using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class MetadataAttribute : Attribute
    {
        public MetadataAttribute(
            string valueType = "",
            string memberName = "",
            string description = "",
            string partInvariantName = "",
            string partFullName = "",
            [CanBeNull] Type containingClass = null,
            [CallerLineNumber]int sourceLine = -1,
            [CallerFilePath] string sourceFile = "",
            bool isRequired = true,
            string defaultValue = "",
            string notes = "",
            bool allowMultipleValues = false,
            [CanBeNull] string[] examples = null,
            [CanBeNull] string[] tags = null,
            string keyType = "")
        {
            MemberName = memberName ?? "";
            Description = description ?? "";
            ValueType = valueType ?? "";
            PartInvariantName = partInvariantName ?? "";
            PartFullName = partFullName ?? "";
            ContainingClass = containingClass;
            SourceLine = sourceLine;
            SourceFile = sourceFile ?? "";
            IsRequired = isRequired;
            DefaultValue = defaultValue ?? "";
            Notes = notes ?? "";
            AllowMultipleValues = allowMultipleValues;
            KeyType = keyType ?? "";
            Examples = new ReadOnlyCollection<string>(examples?.ToList() ?? new List<string>());
            Tags = new ReadOnlyCollection<string>(tags?.ToList() ?? new List<string>());
        }

        public bool AllowMultipleValues { get; }
        public string KeyType { get; set; }

        public Type ContainingClass { get; private set; }

        public string DefaultValue { get; }

        public string Description { get; }

        public IReadOnlyCollection<string> Examples { get; }

        public bool IsRequired { get; }


        public string MemberName { get; }

        public string Notes { get; }

        public string PartFullName { get; }

        public string PartInvariantName { get; }

        public string SourceFile { get; }

        public int SourceLine { get; }

        public IReadOnlyCollection<string> Tags { get; }

        public string ValueType { get; }

        public override string ToString()
        {
            return $"[{nameof(MemberName)}: {MemberName}] [{nameof(ValueType)}: {ValueType}]";
        }
    }
}
