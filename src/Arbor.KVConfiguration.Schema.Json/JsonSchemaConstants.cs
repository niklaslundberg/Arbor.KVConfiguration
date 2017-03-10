using System.Diagnostics.CodeAnalysis;

namespace Arbor.KVConfiguration.Schema.Json
{
    public static class JsonSchemaConstants
    {
        public const string VersionPropertyKey = "urn:arbor:kvconfiguration:schema:version";

        // ReSharper disable InconsistentNaming
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        public const string Version1_0 = "1.0";

        // ReSharper restore InconsistentNaming
    }
}
