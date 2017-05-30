using Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;
using Arbor.KVConfiguration.Urns;
using Microsoft.AspNetCore.Mvc;

namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySample _mySample;

        public HomeController(IConfigurationValue<MySample> mySample)
        {
            _mySample = mySample.Value;
        }

        public IActionResult Index()
        {
            return View(new SampleViewModel(_mySample));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
