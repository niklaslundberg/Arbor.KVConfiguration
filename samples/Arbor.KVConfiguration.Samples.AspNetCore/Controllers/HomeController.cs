using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.KVConfiguration.Urns;

using Microsoft.AspNetCore.Mvc;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers;

public class HomeController : Controller
{
    private readonly MySampleConfiguration _mySampleConfiguration;

    public HomeController(MySampleConfiguration mySample) =>
        _mySampleConfiguration =
            mySample ?? throw new InvalidOperationException("Could not get value for mySample");

    public IActionResult Index() => View(new SampleViewModel(_mySampleConfiguration));

    
    [Route("~/diagnostics")]
    [HttpGet]
    public object Diagnostics(
        [FromServices] ConfigurationInstanceHolder configurationInstanceHolder,
        [FromServices] IEnumerable<MySampleMultipleInstance> multipleInstances)
    {
        ArgumentNullException.ThrowIfNull(configurationInstanceHolder);

        ArgumentNullException.ThrowIfNull(multipleInstances);

        return new
        {
            Instances = configurationInstanceHolder!.RegisteredTypes
                .Select(type => new
                {
                    type.FullName, Instances = configurationInstanceHolder.GetInstances(type).ToArray()
                })
                .ToArray(),
            multipleInstances
        };
    }

    public IActionResult Error() => View();
}