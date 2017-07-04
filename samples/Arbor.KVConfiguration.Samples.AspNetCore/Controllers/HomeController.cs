using Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;
using Microsoft.AspNetCore.Mvc;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySampleConfiguration _mySampleConfiguration;

        public HomeController(IConfigurationValue<MySampleConfiguration> mySample)
        {
            _mySampleConfiguration = mySample.Value;
        }

        public IActionResult Index()
        {
            return View(new SampleViewModel(_mySampleConfiguration));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
