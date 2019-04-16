using System;

namespace Arbor.KVConfiguration.Core
{
    public class ValidationError
    {
        public ValidationError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(errorMessage));
            }

            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}
