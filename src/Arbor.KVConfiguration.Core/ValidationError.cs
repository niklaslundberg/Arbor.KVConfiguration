using System;

namespace Arbor.KVConfiguration.Core
{
    public class ValidationError
    {
        public ValidationError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(errorMessage));
            }

            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}
