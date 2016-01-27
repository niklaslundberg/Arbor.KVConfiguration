using System;

namespace Arbor.KVConfiguration.Schema
{
    public class ValidationError
    {
        public string ErrorMessage { get; }

        public ValidationError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(errorMessage));
            }

            ErrorMessage = errorMessage;
        }
    }
}