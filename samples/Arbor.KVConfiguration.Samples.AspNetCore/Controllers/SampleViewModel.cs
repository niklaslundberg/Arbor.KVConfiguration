namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class SampleViewModel
    {
        public SampleViewModel(MySampleConfiguration mySampleConfiguration)
        {
            MySampleConfiguration = mySampleConfiguration;
        }

        public MySampleConfiguration MySampleConfiguration { get; }
    }
}
