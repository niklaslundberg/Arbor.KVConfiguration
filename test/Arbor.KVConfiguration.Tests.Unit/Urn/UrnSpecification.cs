using System;
using Machine.Specifications;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    public class UrnSpecification
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UrnSpecification(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void EmptyStringShouldThrowInCtor() => Assert.Throws<ArgumentException>(() => new Primitives.Urn(""));

        [Fact]
        public void InvalidUriStringShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("-"));

        [Fact]
        public void InvalidUrnStringShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("http://abc"));

        [Fact]
        public void NullStringShouldThrowInCtor() => Assert.Throws<ArgumentException>(() => new Primitives.Urn(null!));

        [Fact]
        public void UrnStringTooShortShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("urn:/"));

        [Fact]
        public void UrnStringWithDoubleColonShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("urn::"));

        [Fact]
        public void EqualsForSameShouldEqualTrue()
        {
            var urn = new Primitives.Urn("urn:a:b:c");

            Assert.True(urn.Equals(urn));
        }

        [Fact]
        public void EqualsForOtherShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var b = new Primitives.Urn("urn:a:b:d");

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherWithDifferentNidCasingShouldEqualTrue()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var b = new Primitives.Urn("urn:A:b:c");

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void NssShouldNotBeNonEmpty()
        {
            var a = new Primitives.Urn("urn:a:b");

            Assert.Equal("b", a.Nss);
        }

        [Fact]
        public void NssShouldNotBeNonEmptyFor4PartUrn()
        {
            var a = new Primitives.Urn("urn:a:b:c");

            Assert.Equal("b:c", a.Nss);
        }

        [Fact]
        public void EqualsForOtherWithDifferentUrnCasingShouldEqualTrue()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var b = new Primitives.Urn("URN:a:b:c");

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherWithDifferentContentCasingShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var b = new Primitives.Urn("urn:a:B:c");

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherTypeShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");

            Assert.False(a.Equals(new object()));
        }

        [Fact]
        public void EqualsForSameAsObjectShouldEqualTrue()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            object asAObject = a;

            Assert.True(a.Equals(asAObject));
        }

        [Fact]
        public void EqualsForNullAsObjectShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            object? b = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForNullAsUrnShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            Primitives.Urn? b = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void TryParseWithNullShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse(null, out var _));

        [Fact]
        public void TryParseWithEmptyShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("", out var _));

        [Fact]
        public void TryParseInvalidUriShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:a{", out var _));

        [Fact]
        public void CtorWithInvalidUrnShouldThrow() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("urn:a{"));

        // Examples from https://en.wikipedia.org/wiki/Uniform_Resource_Name
        [Theory]
        [InlineData("urn:isbn:0451450523")]
        [InlineData("urn:isan:0000-0000-2CEA-0000-1-0000-0000-Y")]
        [InlineData("urn:ISSN:0167-6423")]
        [InlineData("urn:ietf:rfc:2648")]
        [InlineData("urn:mpeg:mpeg7:schema:2001")]
        [InlineData("urn:oid:2.16.840")]
        [InlineData("urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66")]
        [InlineData("urn:nbn:de:bvb:19-146642")]
        [InlineData("urn:lex:eu:council:directive:2010-03-09;2010-19-UE")]
        [InlineData("urn:lsid:zoobank.org:pub:CDC8D258-8F57-41DC-B560-247E17D3DC8C")]
        public void TryParseValidUrnShouldReturnTrue(string urn) => Primitives.Urn.TryParse(urn, out _).ShouldBeTrue();

        [Theory]
        [InlineData("urn:abc:123", "urn:abc:123")]
        [InlineData("URN:abc:123", "urn:abc:123")]
        [InlineData("URN:ABC:123", "urn:abc:123")]
        public void ValidUrnShouldHaveNormalizedName(string urn, string expectedNormalized)
        {
            Primitives.Urn.TryParse(urn, out var parsed).ShouldBeTrue();

            parsed.Normalized.ShouldEqual(expectedNormalized);
            parsed.ToString().ShouldEqual(expectedNormalized);
        }

        [Theory]
        [InlineData("urn:example:a123,z456?+abc", "urn:example:a123,z456?=xyz", "urn:example:a123,z456#789")]
        public void UrnWithDifferentComponentAreEqual(string a, string b, string c)
        {
            _testOutputHelper.WriteLine(a);
            _testOutputHelper.WriteLine(b);
            _testOutputHelper.WriteLine(c);
            var urnA = Primitives.Urn.Parse(a);
            var urnB = Primitives.Urn.Parse(b);
            var urnC = Primitives.Urn.Parse(c);

            urnA.Equals(urnB).ShouldBeTrue();
            urnB.Equals(urnC).ShouldBeTrue();
        }

        [Theory]
        [InlineData("urn:example:a123,z456?=abc", "", "abc", "")]
        [InlineData("urn:example:a123,z456?+abc?=def", "abc", "def", "")]
        [InlineData("urn:example:a123,z456?=abc#123", "", "abc", "123")]
        [InlineData("urn:example:a123,z456?+abc#123", "abc", "", "123")]
        [InlineData("urn:example:a123,z456?=", "", "", "")]
        [InlineData("urn:example:a123,z456?+?=#", "", "", "")]
        [InlineData("urn:example:a123,z456", "", "", "")]
        [InlineData("urn:example:a123,z456#abc", "", "", "abc")]
        public void Components(string urn,
            string expectedRComponent,
            string expectedQComponent,
            string expectedFragment)
        {
            var urnItem = Primitives.Urn.Parse(urn);
            urnItem.QComponent.ShouldEqual(expectedQComponent);
            urnItem.RComponent.ShouldEqual(expectedRComponent);
            urnItem.Fragment.ShouldEqual(expectedFragment);
        }
    }
}