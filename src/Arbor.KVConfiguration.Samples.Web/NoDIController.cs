using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Arbor.KVConfiguration.Core;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Samples.Web
{
    [RoutePrefix("nodi")]
    public class NoDIController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            ImmutableArray<MultipleValuesStringPair> multipleValuesStringPairs =
                KVConfigurationManager.AppSettings.AllWithMultipleValues;

            string pairs = string.Join(
                Environment.NewLine,
                multipleValuesStringPairs.Select(
                    pair =>
                    $"<li>{pair.Key} <ul>{string.Join(Environment.NewLine, pair.Values.Select(value => $"<li>{value}</li>"))}</ul></li>"));

            var contentResult = new ContentResult
                                    {
                                        Content =
                                            $"<!DOCTYPE html><html><body><ul>{pairs}</ul></body></html>",
                                        ContentEncoding = Encoding.UTF8,
                                        ContentType = "text/html"
                                    };
            return contentResult;
        }
    }
}
