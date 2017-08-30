using Arbor.KVConfiguration.Core.Decorators;

namespace Arbor.KVConfiguration.Samples.Web
{
    internal class AddSuffixDecorator : DecoratorBase
    {
        private readonly string _suffix;

        public AddSuffixDecorator(string suffix)
        {
            _suffix = suffix;
        }

        public override string GetValue(string value)
        {
            return $"{value}{_suffix}";
        }
    }
}
