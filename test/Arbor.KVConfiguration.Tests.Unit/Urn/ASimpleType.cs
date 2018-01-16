using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Urn("urn:a:simple:type")]
    public class ASimpleType
    {
        public string Url { get; }
        public string Text { get; }

        public ASimpleType(string url, string text)
        {
            Url = url;
            Text = text;
        }
    }
}