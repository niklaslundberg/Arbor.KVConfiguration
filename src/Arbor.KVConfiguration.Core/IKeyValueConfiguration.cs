﻿using System.Collections.Generic;

using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public interface IKeyValueConfiguration
    {
        IReadOnlyCollection<string> AllKeys { get; }

        IReadOnlyCollection<StringPair> AllValues { get; }

        IReadOnlyCollection<MultipleValuesStringPair> AllWithMultipleValues { get; }

        [CanBeNull]
        string this[[CanBeNull] string key] { get; }
    }
}
