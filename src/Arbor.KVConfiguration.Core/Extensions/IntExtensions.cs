namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class IntExtensions
    {
        public static int ValueOrDefault(this string value, int defaultValue)
        {
            int parsedResultValue;

            if (!int.TryParse(value, out parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }

    }
}
