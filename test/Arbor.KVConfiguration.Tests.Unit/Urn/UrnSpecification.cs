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
        public void ParseEmptyStringShouldThrow() => Assert.Throws<ArgumentException>(() => Primitives.Urn.Parse(""));

        [Fact]
        public void ParseInvalidUriStringShouldThrow() =>
            Assert.Throws<FormatException>(() => Primitives.Urn.Parse("-"));

        [Fact]
        public void ParseInvalidUrnStringShouldThrow() =>
            Assert.Throws<FormatException>(() => Primitives.Urn.Parse("http://abc"));

        [Fact]
        public void ParseNullStringShouldThrow() => Assert.Throws<ArgumentException>(() => Primitives.Urn.Parse(null!));

        [Fact]
        public void ParseUrnStringTooShortShouldThrow() =>
            Assert.Throws<FormatException>(() => Primitives.Urn.Parse("urn:\\"));

        [Fact]
        public void ParseUrnStringWithDoubleColonShouldThrow() =>
            Assert.Throws<FormatException>(() => Primitives.Urn.Parse("urn::"));

        [Fact]
        public void EqualsForSameShouldEqualTrue()
        {
            var urn = Primitives.Urn.Parse("urn:a:b:c");

            Assert.True(urn.Equals(urn));
        }

        [Fact]
        public void EqualsForOtherShouldEqualFalse()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            var b = Primitives.Urn.Parse("urn:a:b:d");

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherWithDifferentNidCasingShouldEqualTrue()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            var b = Primitives.Urn.Parse("urn:A:b:c");

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void NssShouldNotBeNonEmpty()
        {
            var a = Primitives.Urn.Parse("urn:a:b");

            Assert.Equal("b", a.Nss);
        }

        [Fact]
        public void NssShouldNotBeNonEmptyFor4PartUrn()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");

            Assert.Equal("b:c", a.Nss);
        }

        [Fact]
        public void EqualsForOtherWithDifferentUrnCasingShouldEqualTrue()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            var b = Primitives.Urn.Parse("URN:a:b:c");

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherWithDifferentContentCasingShouldEqualFalse()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            var b = Primitives.Urn.Parse("urn:a:B:c");

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForOtherTypeShouldEqualFalse()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");

            Assert.False(a.Equals(new object()));
        }

        [Fact]
        public void EqualsForSameAsObjectShouldEqualTrue()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            object asAObject = a;

            Assert.True(a.Equals(asAObject));
        }

        [Fact]
        public void EqualsForNullAsObjectShouldEqualFalse()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            object? b = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForNullAsUrnShouldEqualFalse()
        {
            var a = Primitives.Urn.Parse("urn:a:b:c");
            Primitives.Urn? b = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void TryParseUrnWithEncodedCharactersShouldReturnTrue()
        {
            bool parsed = Primitives.Urn.TryParse("urn:example:a123%2Cz456", out var urn);

            Assert.True(parsed);
            Assert.Equal("urn:example:a123%2Cz456", urn!.Value.NameString);
        }

        [Fact]
        public void EqualsShouldReturnFalseForEncodedCharacterDifference()
        {
            Primitives.Urn.TryParse("urn:example:a123%2Cz456", out var a);
            Primitives.Urn.TryParse("urn:example:a123,Cz456", out var b);

            Assert.NotEqual(a,b);
        }

        [Fact]
        public void EqualsShouldReturnFalseForDifferentPartAfterSlash()
        {
            Primitives.Urn.TryParse("urn:example:a123,z456/foo", out var a);
            Primitives.Urn.TryParse("urn:example:a123,z456/bar", out var b);
            Primitives.Urn.TryParse("urn:example:a123,z456/baz", out var c);

            Assert.NotEqual(a,b);
            Assert.NotEqual(b,c);
            Assert.NotEqual(a,c);
        }

        [Fact]
        public void TryParseWithNullShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse(null, out _));

        [Fact]
        public void TryParseWithEmptyShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("", out _));

        [Fact]
        public void TryParseInvalidUriShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:a{", out _));

        [Fact]
        public void TryParseInvalidUriWithQBeforeRShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:abc:123?=abc?+", out _));

        [Fact]
        public void TryParseInvalidUriWithQInFragmentShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:abc:123?=abc?+#?=", out _));

        [Fact]
        public void TryParseInvalidUriWithRInFragmentShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:abc:123?+abc?+#?+", out _));

        [Fact]
        public void TryParseInvalidUriWithFragmentInFragmentShouldReturnFalse() => Assert.False(Primitives.Urn.TryParse("urn:abc:123#123#456", out _));

        [Fact]
        public void ParseInvalidUrnShouldThrow() =>
            Assert.Throws<FormatException>(() => Primitives.Urn.Parse("urn:a{"));

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
        [InlineData("URN:ABC:123?+", "urn:abc:123")]
        [InlineData("URN:ABC:123?=", "urn:abc:123")]
        [InlineData("URN:ABC:123#", "urn:abc:123")]
        public void ValidUrnShouldHaveNormalizedName(string urn, string expectedNormalized)
        {
            Primitives.Urn.TryParse(urn, out var parsed).ShouldBeTrue();

            parsed.Value.AssignedName.ShouldEqual(expectedNormalized);
        }

        [Theory]
        [InlineData("urn:abc:123", "urn:abc:123")]
        [InlineData("URN:abc:123", "urn:abc:123")]
        [InlineData("URN:ABC:123", "urn:ABC:123")]
        [InlineData("URN:ABC:123?+", "urn:ABC:123?+")]
        [InlineData("URN:ABC:123?=", "urn:ABC:123?=")]
        [InlineData("URN:ABC:123#", "urn:ABC:123#")]
        public void ValidUrnShouldHaveFullName(string urn, string expectedFullName)
        {
            Primitives.Urn.TryParse(urn, out var parsed).ShouldBeTrue();

            parsed.Value.NameString.ShouldEqual(expectedFullName);
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
        [InlineData("urn:example:a123,z456?+abc?=def#ghi", "abc", "def", "ghi")]
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
            urnItem.FComponent.ShouldEqual(expectedFragment);
        }
    }
}