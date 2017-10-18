using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Xml;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers
{
    public abstract class KeyValueConfigurationBuilder : ConfigurationBuilder
    {
        protected abstract IKeyValueConfiguration GetKeyValueConfiguration();

        public override XmlNode ProcessRawXml(XmlNode rawXml)
        {
            foreach (MultipleValuesStringPair appSettingsAllWithMultipleValue in GetKeyValueConfiguration().AllWithMultipleValues)
            {
                foreach (string value in appSettingsAllWithMultipleValue.Values)
                {
                    XmlElement xmlElement = rawXml.OwnerDocument.CreateElement("add");

                    xmlElement.SetAttribute("key", appSettingsAllWithMultipleValue.Key);
                    xmlElement.SetAttribute("value", value);

                    rawXml.AppendChild(xmlElement);
                }
            }

            return base.ProcessRawXml(rawXml: rawXml);
        }

    }
}
