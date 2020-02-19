namespace Arbor.Primitives
{
    internal static class CharExtensions
    {
        public static bool IsAscii(this char character) => character <= 127;
    }
}