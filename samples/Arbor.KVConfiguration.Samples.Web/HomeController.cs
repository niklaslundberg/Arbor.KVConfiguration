using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;
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
            StringPair[] valuePerKey = _keyValueConfiguration.AllKeys.Select(key => new StringPair(key, _keyValueConfiguration[key])).ToArray();

            var data = new
            {
                _keyValueConfiguration.AllKeys,
                valuePerKey,
                _keyValueConfiguration.AllValues,
                _keyValueConfiguration.AllWithMultipleValues
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
