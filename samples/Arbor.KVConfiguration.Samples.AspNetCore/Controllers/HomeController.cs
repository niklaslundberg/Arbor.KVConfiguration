using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Urns;
using Microsoft.AspNetCore.Mvc;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySampleConfiguration _mySampleConfiguration;

        public HomeController(MySampleConfiguration mySample)
        {
            _mySampleConfiguration =
                mySample ?? throw new InvalidOperationException("Could not get value for mySample");
        }

        public IActionResult Index()
        {
            return View(new SampleViewModel(_mySampleConfiguration));
        }

        [Route("~/diagnostics")]
        [HttpGet]
        public object Diagnostics(
            [FromServices] ConfigurationInstanceHolder configurationInstanceHolder,
            [FromServices] IEnumerable<MySampleMultipleInstance> multipleInstances,
            [FromServices] KeyValueConfigurationValidationSummary validationSummary)
        {
            return new
            {
                Instances = configurationInstanceHolder.RegisteredTypes
                    .Select(type => new
                    {
                        type.FullName,
                        Instances = configurationInstanceHolder.GetInstances(type).ToArray()
                    })
                    .ToArray(),
                multipleInstances,
                validationSummary
            };
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
