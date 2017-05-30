namespace Arbor.KVConfiguration.Samples.AspNetCore.Controllers
{
    public class SampleViewModel
    {
        public MySample MySample { get; }

        public SampleViewModel(MySample mySample)
        {
            MySample = mySample;
        }
    }
}