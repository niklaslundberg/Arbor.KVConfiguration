using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySampleConfiguration _mySampleConfiguration;

        public HomeController(MySampleConfiguration mySample) =>
            _mySampleConfiguration =
                mySample ?? throw new InvalidOperationException("Could not get value for mySample");

        public IActionResult Index() => View(new SampleViewModel(_mySampleConfiguration));

        [PublicAPI]
        [Route("~/diagnostics")]
        [HttpGet]
        public object Diagnostics(
            [FromServices] ConfigurationInstanceHolder configurationInstanceHolder,
            [FromServices] IEnumerable<MySampleMultipleInstance> multipleInstances)
        {
            if (configurationInstanceHolder is null)
            {
                throw new ArgumentNullException(nameof(configurationInstanceHolder));
            }

            if (multipleInstances is null)
            {
                throw new ArgumentNullException(nameof(multipleInstances));
            }

            return new
            {
                Instances = configurationInstanceHolder!.RegisteredTypes
                    .Select(type => new
                    {
                        type.FullName,
                        Instances = configurationInstanceHolder.GetInstances(type).ToArray()
                    })
                    .ToArray(),
                multipleInstances
            };
        }

        public IActionResult Error() => View();
    }
}
