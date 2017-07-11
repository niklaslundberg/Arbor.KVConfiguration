using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Core
{
    public class NullDecorator : DecoratorBase
    {
        public override string GetValue(string value)
        {
            return value;
        }
    }
}
