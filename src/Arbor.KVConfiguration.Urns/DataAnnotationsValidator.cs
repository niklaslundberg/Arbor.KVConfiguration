using System.Collections.Generic;

namespace Arbor.KVConfiguration.Urns
{
    public static class DataAnnotationsValidator
    {
        public static bool TryValidate(object instance, out ImmutableArray<ValidationResult> results)
        {
            if (instance is null)
            {
                results = ImmutableArray<ValidationResult>.Empty;
                return false;
            }

            var context = new ValidationContext(instance, null, null);
            var validationResults = new List<ValidationResult>();

            bool tryValidateObject = Validator.TryValidateObject(
                instance,
                context,
                validationResults,
                true
            );

            results = validationResults.ToImmutableArray();

            return tryValidateObject;
        }
    }
}