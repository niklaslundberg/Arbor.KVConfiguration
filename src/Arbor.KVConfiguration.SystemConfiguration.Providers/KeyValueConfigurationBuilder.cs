using System.Configuration;
using System.Xml;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers
{
    public abstract class KeyValueConfigurationBuilder : ConfigurationBuilder
    {
        protected abstract IKeyValueConfiguration GetKeyValueConfiguration();

        public override XmlNode ProcessRawXml(XmlNode rawXml)
        {
            if (rawXml is null)
            {
                return null;
            }

            XmlDocument rawXmlOwnerDocument = rawXml.OwnerDocument;

            if (rawXmlOwnerDocument is null)
            {
                return rawXml;
            }

            foreach (MultipleValuesStringPair appSettingsAllWithMultipleValue in GetKeyValueConfiguration().AllWithMultipleValues)
            {
                foreach (string value in appSettingsAllWithMultipleValue.Values)
                {
                    XmlElement xmlElement = rawXmlOwnerDocument.CreateElement("add");

                    xmlElement.SetAttribute("key", appSettingsAllWithMultipleValue.Key);
                    xmlElement.SetAttribute("value", value);

                    rawXml.AppendChild(xmlElement);
                }
            }

            return base.ProcessRawXml(rawXml: rawXml);
        }

    }
}
