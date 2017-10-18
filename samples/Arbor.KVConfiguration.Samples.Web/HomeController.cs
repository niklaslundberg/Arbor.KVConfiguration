using System.Collections.Immutable;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Core.Metadata.Extensions;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Samples.Web
{
    [Route]
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        private readonly IKeyValueConfiguration _keyValueConfiguration;

        public HomeController(IKeyValueConfiguration keyValueConfiguration)
        {
            _keyValueConfiguration = keyValueConfiguration;
        }

        [Route]
        public ActionResult Index()
        {
            StringPair[] valuePerKey = _keyValueConfiguration.AllKeys
                .Select(key => new StringPair(key, _keyValueConfiguration[key])).ToArray();

            object sourceForKey = new { };
            ImmutableArray<KeyValueConfigurationItem> configurationItems;
            if (_keyValueConfiguration is MultiSourceKeyValueConfiguration multiSourceKeyValueConfiguration)
            {
                sourceForKey = multiSourceKeyValueConfiguration.AllKeys.Select(key => new
                {
                    key,
                    source = multiSourceKeyValueConfiguration
                        .ConfiguratorFor(key).GetType().Name
                }).ToImmutableArray();

                configurationItems = multiSourceKeyValueConfiguration.ConfigurationItems;
            }
            else
            {
                configurationItems = _keyValueConfiguration.GetKeyValueConfigurationItems();
            }

            var appData = ConfigurationManager.AppSettings.AllKeys.Select(key => new {Key=key,Value=ConfigurationManager.AppSettings[key] }).ToArray();

            var data = new
            {
                _keyValueConfiguration.AllKeys,
                valuePerKey,
                _keyValueConfiguration.AllValues,
                _keyValueConfiguration.AllWithMultipleValues,
                ConfigurationItems = configurationItems,
                sourceForKey,
                AppDataNonExistingKey = ConfigurationManager.AppSettings["non-existing-key"],
                AppDataExistingKey = ConfigurationManager.AppSettings["ATestKey"],
                appData
            };

            var contentResult = new ContentResult
            {
                Content =
                    JsonConvert.SerializeObject(data),
                ContentEncoding = Encoding.UTF8,
                ContentType = "text/plain"
            };
            return contentResult;
        }
    }
}
