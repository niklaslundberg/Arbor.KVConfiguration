namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class LongExtensions
    {
        public static long ValueOrDefault(this string value, long defaultValue)
        {
            long parsedResultValue;

            if (!long.TryParse(value, out parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}