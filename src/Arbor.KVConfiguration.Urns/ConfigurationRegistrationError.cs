namespace Arbor.KVConfiguration.Urns
{
    public class ConfigurationRegistrationError
    {
        public ConfigurationRegistrationError(string error)
        {
            Error = error;
        }

        public string Error { get; }
    }
}
