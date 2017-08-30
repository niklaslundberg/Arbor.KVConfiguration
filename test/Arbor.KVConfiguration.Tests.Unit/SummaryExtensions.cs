using System.Linq;
using System.Text;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Validators;

namespace Arbor.KVConfiguration.Tests.Unit
{
    internal static class SummaryExtensions
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

                foreach (KeyValueConfigurationValidationResult keyValueConfigurationValidationResult in errors)
                {
                    builder.AppendLine($"# {keyValueConfigurationValidationResult.KeyMetadata.Key}");

                    foreach (ValidationError validationError in keyValueConfigurationValidationResult.ValidationErrors)
                    {
                        builder.AppendLine($" * {validationError.ErrorMessage}");
                    }

                    builder.AppendLine(
                        $"Value: [{string.Join(", ", keyValueConfigurationValidationResult.Values.Select(item => $"'{item}'"))}]");
                }
            }

            return builder.ToString();
        }
    }
}
