namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    public class UrnEqualsTests
    {
        [Fact]
        public void UrnsWithDifferentUrnOrNidShouldBeEqual()
        {
            var urn1 = Primitives.Urn.Parse("urn:foo:a123,456");
            var urn2 = Primitives.Urn.Parse("URN:foo:a123,456");
            var urn3 = Primitives.Urn.Parse("urn:FOO:a123,456");

            Assert.True(urn1.Equals(urn2));
            Assert.True(urn2.Equals(urn3));
        }

        [Fact]
        public void UrnsCasingShouldNotBeEqual()
        {
            var urn1 = Primitives.Urn.Parse("urn:foo:a123,456");
            var urn2 = Primitives.Urn.Parse("URN:foo:A123,456");
            var urn3 = Primitives.Urn.Parse("urn:FOO:a123,456");

            Assert.Equal(urn1, urn3);
            Assert.NotEqual(urn1, urn2);
            Assert.NotEqual(urn2, urn3);
        }
    }
}