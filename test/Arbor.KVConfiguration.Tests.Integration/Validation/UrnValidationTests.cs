using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Schema.Validators;
using Arbor.KVConfiguration.Urns;
using Xunit;

namespace Arbor.KVConfiguration.Tests.Integration.Validation
{
    public class UrnValidationTests
    {
        [Fact]
        public void CanValidateEmptyShouldThrow()
        {
            IValueValidator urnValidator = new UrnValidator();

            Assert.Throws<ArgumentException>(() => { urnValidator.CanValidate(""); });
        }

        [Fact]
        public void CanValidateNullShouldThrow()
        {
            IValueValidator urnValidator = new UrnValidator();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => { urnValidator.CanValidate(null); });
        }

        [Fact]
        public void CanValidateUrnShouldReturnTrue()
        {
            IValueValidator urnValidator = new UrnValidator();

            bool canValidate = urnValidator.CanValidate("urn");

            Assert.True(canValidate);
        }

        [Fact]
        public void ItShouldValidateEmptyStringAsInvalid()
        {
            IValueValidator urnValidator = new UrnValidator();

            ImmutableArray<ValidationError> validationErrors = urnValidator.Validate("urn", "");

            Assert.Single(validationErrors);
        }

        [Fact]
        public void ItShouldValidateNullStringAsInvalid()
        {
            IValueValidator urnValidator = new UrnValidator();

            ImmutableArray<ValidationError> validationErrors = urnValidator.Validate("urn", null);

            Assert.Single(validationErrors);
        }

        [Fact]
        public void ValidateValidUrnShouldProduceNoErrors()
        {
            IValueValidator urnValidator = new UrnValidator();

            ImmutableArray<ValidationError> validationErrors = urnValidator.Validate("urn", "urn:test:abc");

            Assert.Empty(validationErrors);
        }

        [Fact]
        public void ValidateValidUrnWithWrongTypeShouldProduceNoErrors()
        {
            IValueValidator urnValidator = new UrnValidator();

            Assert.Throws<InvalidOperationException>(() => { urnValidator.Validate("bool", "urn:abc"); });
        }
    }
}