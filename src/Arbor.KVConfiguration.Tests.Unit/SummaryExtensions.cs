using System.Linq;
using System.Text;

using Arbor.KVConfiguration.Schema;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public static class SummaryExtensions
    {
        public static string Print(this KeyValueConfigurationValidationSummary summary)
        {
            var builder = new StringBuilder();

            if (summary.IsValid)
            {
                builder.AppendLine("VALID");
            }
            else
            {
                builder.AppendLine("INVALID");

                KeyValueConfigurationValidationResult[] errors =
                    summary.KeyValueConfigurationValidationResults.Where(_ => !_.IsValid).ToArray();

                foreach (var keyValueConfigurationValidationResult in errors)
                {
                    builder.AppendLine("# " + keyValueConfigurationValidationResult.KeyMetadata.Key);
                    foreach (ValidationError validationError in keyValueConfigurationValidationResult.ValidationErrors)
                    {
                        builder.AppendLine(" * " + validationError.ErrorMessage);
                    }
                }
            }

            return builder.ToString();
        }
    }
}