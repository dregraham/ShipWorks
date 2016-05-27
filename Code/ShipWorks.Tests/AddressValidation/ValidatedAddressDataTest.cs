using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using Xunit;

namespace ShipWorks.Tests.AddressValidation
{
    public class ValidatedAddressDataTest
    {
        [Fact]
        public void All_ContainsOriginal_WhenOriginalIsNotNull()
        {
            var original = new ValidatedAddressEntity();
            var testObject = new ValidatedAddressData(original, Enumerable.Empty<ValidatedAddressEntity>());

            Assert.Contains(original, testObject.AllAddresses);
        }

        [Fact]
        public void All_DoesNotContainsOriginal_WhenOriginalIsNull()
        {
            var original = new ValidatedAddressEntity();
            var testObject = new ValidatedAddressData(null, Enumerable.Empty<ValidatedAddressEntity>());

            Assert.Empty(testObject.AllAddresses);
        }

        [Fact]
        public void All_OriginalIsFirst_WhenOriginalIsNotNullAndThereAreSuggestions()
        {
            var original = new ValidatedAddressEntity();
            var testObject = new ValidatedAddressData(original, new[] { new ValidatedAddressEntity(), new ValidatedAddressEntity() });

            Assert.Equal(original, testObject.AllAddresses.First());
        }

        [Fact]
        public void All_ContainsSuggestions_WhenSuggestionsAreNotNull()
        {
            var address = new ValidatedAddressEntity();
            var testObject = new ValidatedAddressData(null, new[] { address });

            Assert.Contains(address, testObject.AllAddresses);
        }

        [Fact]
        public void All_DoesNotContainSuggestions_WhenSuggestionsAreNull()
        {
            var testObject = new ValidatedAddressData(null, null);

            Assert.Empty(testObject.AllAddresses);
        }

        [Fact]
        public void All_DoesNotContainSuggestions_WhenSuggestionsAreEmpty()
        {
            var testObject = new ValidatedAddressData(null, Enumerable.Empty<ValidatedAddressEntity>());

            Assert.Empty(testObject.AllAddresses);
        }

        [Fact]
        public void All_SuggestionsAreInSameOrder_WhenSuggestionsAreNotNull()
        {
            var address1 = new ValidatedAddressEntity();
            var address2 = new ValidatedAddressEntity();
            var testObject = new ValidatedAddressData(null, new[] { address1, address2 });

            Assert.Equal(address1, testObject.AllAddresses.First());
            Assert.Equal(address2, testObject.AllAddresses.Last());
        }
    }
}
