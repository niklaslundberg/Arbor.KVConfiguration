﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public interface IValueValidator
    {
        bool CanValidate([NotNull]string type);

        IEnumerable<ValidationError> Validate([NotNull]string type, [CanBeNull]string value);
    }
}