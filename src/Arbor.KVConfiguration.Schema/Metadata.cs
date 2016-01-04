namespace Arbor.KVConfiguration.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Metadata
    {
        public Metadata(
            string key,
            string memberName,
            string description,
            Type valueType,
            string partInvariantName,
            string partFullName,
            Type containingClass,
            int sourceLine,
            string sourceFile,
            bool isRequired = true,
            string defaultValue = "",
            string notes = "",
            IEnumerable<string> examples = null,
            IEnumerable<string> tags = null)
        {
            Key = key;
            MemberName = memberName;
            Description = description;
            ValueType = valueType.FullName;
            PartInvariantName = partInvariantName;
            PartFullName = partFullName;
            ContainingClass = containingClass;
            SourceLine = sourceLine;
            SourceFile = sourceFile;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            Notes = notes;
            Examples = new ReadOnlyCollection<string>(examples?.ToList() ?? new List<string>());
            Tags = new ReadOnlyCollection<string>(tags?.ToList() ?? new List<string>());
        }

        public Type ContainingClass { get; private set; }

        public string DefaultValue { get; }

        public string Description { get; }

        public IReadOnlyCollection<string> Examples { get; }

        public bool IsRequired { get; }

        public string Key { get; }

        public string MemberName { get; }

        public string Notes { get; }

        public string PartFullName { get; }

        public string PartInvariantName { get; }

        public string SourceFile { get; }

        public int SourceLine { get; }

        public IReadOnlyCollection<string> Tags { get; }

        public string ValueType { get; }
    }
}