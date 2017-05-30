namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class SampleViewModel
    {
        public SampleViewModel(MySample mySample)
        {
            MySample = mySample;
        }

        public MySample MySample { get; }
    }
}
