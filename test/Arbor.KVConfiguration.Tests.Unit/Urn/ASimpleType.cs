using Arbor.KVConfiguration.Urns;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Urn("urn:a:simple:type")]
    public class ASimpleType
    {
        public ASimpleType(string url, string text)
        {
            Url = url;
            Text = text;
        }

        public string Url { get; }

        public string Text { get; }
    }
}
