using System;
using System.Text;
using System.Web.Mvc;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Samples.Web
{
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
            var contentResult = new ContentResult
            {
                Content =
                    string.Join(
                        Environment.NewLine,
                        _keyValueConfiguration.AllValues),
                ContentEncoding = Encoding.UTF8,
                ContentType = "text/plain"
            };
            return contentResult;
        }
    }
}
