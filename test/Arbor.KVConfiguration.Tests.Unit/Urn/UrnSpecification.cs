using System;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    public class UrnSpecification
    {
        [Fact]
        public void EmptyStringShouldThrowInCtor() => Assert.Throws<ArgumentException>(() => new Primitives.Urn(""));

        [Fact]
        public void InvalidUriStringShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("-"));

        [Fact]
        public void InvalidUrnStringShouldThrowInCtor() =>
            Assert.Throws<FormatException>(() => new Primitives.Urn("http://abc"));

        [Fact]
        public void NullStringShouldThrowInCtor() => Assert.Throws<ArgumentException>(() => new Primitives.Urn(null));

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
        public void EqualsForOtherTypeShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");

            Assert.False(a.Equals(new object()));
        }

        [Fact]
        public void EqualsForSameAsObjectShouldEqualTrue()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var asAObject = (object)a;

            Assert.True(a.Equals(asAObject));
        }

        [Fact]
        public void EqualsForNullAsObjectShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            var b = (object)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualsForNullAsUrnShouldEqualFalse()
        {
            var a = new Primitives.Urn("urn:a:b:c");
            Primitives.Urn b = null;

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
    }
}