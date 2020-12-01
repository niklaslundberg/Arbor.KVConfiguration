using System;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Machine.Specifications;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit
{
    public class DisposeTests
    {
        [Fact]
        public void NoConfigurationDispose()
        {
            IKeyValueConfiguration configuration = NoConfiguration.Empty;

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            configuration.AllKeys.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void InMemoryDisposeAllKeys()
        {
            IKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection());

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllKeys.ToString());
        }

        [Fact]
        public void InMemoryDisposeAllValues()
        {
            IKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection());

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllValues.ToString());
        }

        [Fact]
        public void InMemoryDisposeAllWithMultipleValues()
        {
            IKeyValueConfiguration configuration = new Core.InMemoryKeyValueConfiguration(new NameValueCollection());

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllWithMultipleValues.ToString());
        }

        [Fact]
        public void MultiSourceAllKeys()
        {
            IKeyValueConfiguration configuration = KeyValueConfigurationManager.Add(NoConfiguration.Empty).Build();

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllKeys.ToString());
        }

        [Fact]
        public void MultiSourceShouldDisposeNested()
        {
            IKeyValueConfiguration inMemoryKeyValueConfiguration =
                new Core.InMemoryKeyValueConfiguration(new NameValueCollection());
            IKeyValueConfiguration configuration =
                KeyValueConfigurationManager.Add(inMemoryKeyValueConfiguration).Build();

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => inMemoryKeyValueConfiguration.AllKeys.ToString());
        }

        [Fact]
        public void MultiSourceAllValues()
        {
            IKeyValueConfiguration configuration = KeyValueConfigurationManager.Add(NoConfiguration.Empty).Build();

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllValues.ToString());
        }

        [Fact]
        public void MultiSourceAllWithMultipleValues()
        {
            IKeyValueConfiguration configuration = KeyValueConfigurationManager.Add(NoConfiguration.Empty).Build();

            if (configuration is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => configuration.AllWithMultipleValues.ToString());
        }
    }

    public class NoConfigurationTests
    {
        [Fact]
        public void AllCollectionsShouldBeEmpty()
        {
            IKeyValueConfiguration configuration = NoConfiguration.Empty;

            configuration.AllKeys.IsEmpty.ShouldBeTrue();
            configuration.AllValues.IsEmpty.ShouldBeTrue();
            configuration.AllWithMultipleValues.IsEmpty.ShouldBeTrue();
        }
    }
}