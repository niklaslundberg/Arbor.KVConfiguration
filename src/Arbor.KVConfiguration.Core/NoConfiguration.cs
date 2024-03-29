﻿using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class NoConfiguration : IKeyValueConfiguration
    {
        [NotNull] public static readonly IKeyValueConfiguration Empty =
            new NoConfiguration();

        private NoConfiguration()
        {
        }

        public ImmutableArray<string> AllKeys => ImmutableArray<string>.Empty;

        public ImmutableArray<StringPair> AllValues => ImmutableArray<StringPair>.Empty;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues =>
            ImmutableArray<MultipleValuesStringPair>.Empty;

        public string this[string? key] => string.Empty;
    }
}