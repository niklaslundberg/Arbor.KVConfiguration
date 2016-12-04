namespace Arbor.KVConfiguration.Core.Extensions
{
    public static class BoolExtensions
    {
        public static bool ValueOrDefault(this string value, bool defaultValue)
        {
            bool parsedResultValue;

            if (!bool.TryParse(value, out parsedResultValue))
            {
                return defaultValue;
            }

            return parsedResultValue;
        }
    }
}
