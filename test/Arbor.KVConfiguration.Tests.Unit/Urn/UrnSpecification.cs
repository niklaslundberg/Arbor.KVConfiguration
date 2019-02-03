using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    public class UrnSpecification
    {
        [Fact]
        public void EmptyStringShouldThrowInCtor()
        {
            Assert.Throws<ArgumentException>(() => new Urns.Urn(""));
        }
        [Fact]
        public void NullStringShouldThrowInCtor()
        {
            Assert.Throws<ArgumentException>(() => new Urns.Urn(null));
        }
        [Fact]
        public void InvalidUriStringShouldThrowInCtor()
        {
            Assert.Throws<FormatException>(() => new Urns.Urn("-"));
        }
        [Fact]
        public void InvalidUrnStringShouldThrowInCtor()
        {
            Assert.Throws<FormatException>(() => new Urns.Urn("http://abc"));
        }

        [Fact]
        public void UrnStringTooShortShouldThrowInCtor()
        {
            Assert.Throws<FormatException>(() => new Urns.Urn("urn:/"));
        }
        [Fact]
        public void UrnStringWithDoubleColonShouldThrowInCtor()
        {
            Assert.Throws<FormatException>(() => new Urns.Urn("urn::"));
        }
    }
}
