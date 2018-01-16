using System;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_missing_required_values
    {
        private static IKeyValueConfiguration configuration;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                { "urn:type:with:required:ctor:instance1:other", "abc" },
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            exception = Catch.Exception( ()=> configuration.GetInstance(typeof(TypeWithRequiredCtor)) as TypeWithRequiredCtor);
        };

        private static Exception exception;

        private It should_throw_invalid_operation_exception = () =>
        {
            Console.WriteLine(exception);
            exception.ShouldBeOfExactType<InvalidOperationException>();
        };

        private It should_throw_invalid_operation_exception_with_inner_argument_exception_from_type = () =>
        {
            Console.WriteLine(exception.InnerException);
            exception?.InnerException.ShouldBeOfExactType<ArgumentException>();
        };

        private It should_throw_invalid_operation_exception_with_inner_exception_from_type = () =>
        {
            Console.WriteLine(exception.InnerException);
            exception.InnerException.ShouldNotBeNull();
        };
    }
}