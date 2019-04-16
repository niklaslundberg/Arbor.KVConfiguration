using Arbor.KVConfiguration.DependencyInjection;
using Arbor.KVConfiguration.Urns;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit.DependencyInjection
{
    public class WhenRegisteringANamedGenericInstanceFromInterface
    {
        private class MyConfiguration
        {
            public MyConfiguration(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        [Fact]
        public void ItShouldBeResolvableByInterfaceAndItsWrappedType()
        {
            var configurationInstanceHolder = new ConfigurationInstanceHolder();

            var configuration = new MyConfiguration(123);
            var namedInstance = new NamedInstance<MyConfiguration>(configuration, "myInstance");
            configurationInstanceHolder.Add(namedInstance);

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddConfigurationInstanceHolder(configurationInstanceHolder)
                .BuildServiceProvider();

            var myConfiguration = serviceProvider.GetService<MyConfiguration>();

            Assert.NotNull(myConfiguration);
            Assert.Equal(123, myConfiguration.Id);

            var namedFromProviderInstance = serviceProvider.GetService<INamedInstance<MyConfiguration>>();

            Assert.NotNull(namedFromProviderInstance);
            Assert.Equal(123, namedFromProviderInstance.Value.Id);
            Assert.Equal("myInstance", namedFromProviderInstance.Name);
            Assert.Equal(namedInstance, namedFromProviderInstance);
        }
    }
}
