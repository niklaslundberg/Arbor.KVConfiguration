using System;
using System.Text;
using System.Web.Mvc;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Samples.Web
{
    [RoutePrefix("nodi")]
    public class NoDIController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            var contentResult = new ContentResult
                                    {
                                        Content =
                                            string.Join(
                                                Environment.NewLine, 
                                                KVConfigurationManager.AppSettings.AllValues), 
                                        ContentEncoding = Encoding.UTF8, 
                                        ContentType = "text/plain"
                                    };
            return contentResult;
        }
    }
}