namespace Arbor.KVConfiguration.Core.Decorators
{
    internal class NullDecorator : DecoratorBase
    {
        public override string GetValue(string value) => value;
    }
}