using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.AddressValidation
{
    public class AddressValidatorTest
    {
        private AddressValidationResult result1;
        private AddressValidationResult result2;
        private AddressValidationWebClientValidateAddressResult results;
        private OrderEntity sampleOrder;
        private Mock<IAddressValidationWebClient> webClient;
        private AddressValidator testObject;
        private string errorMessage;

        public AddressValidatorTest()
        {
            result1 = new AddressValidationResult
            {
                Street1 = "Street 1",
                Street2 = "Street 2",
                Street3 = "Street 3",
                City = "City",
                StateProvCode = "MO",
                CountryCode = "US",
                PostalCode = "63102"
            };

            result2 = new AddressValidationResult
            {
                Street1 = "Street 4",
                Street2 = "Street 5",
                Street3 = "Street 6",
                City = "City",
                StateProvCode = "IL",
                CountryCode = "US",
                PostalCode = "63102"
            };

            results = new AddressValidationWebClientValidateAddressResult()
            {
                AddressValidationError = string.Empty,
                AddressValidationResults = new List<AddressValidationResult> { result1, result2 }
            };

            sampleOrder = new OrderEntity
            {
                ShipStreet1 = "Street 1",
                ShipStreet2 = "Street 2",
                ShipStreet3 = "Street 3",
                ShipCity = "City",
                ShipStateProvCode = "MO",
                ShipCountryCode = "US",
                ShipPostalCode = "63102",
                ShipAddressValidationStatus = (int)AddressValidationStatusType.NotChecked
            };

            errorMessage = string.Empty;

            webClient = new Mock<IAddressValidationWebClient>();
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>())).Returns(results);
            testObject = new AddressValidator(webClient.Object);
        }

        [Fact]
        public void Validate_DoesNotCallWebClient_WhenAddressHasBeenValidated()
        {
            EnumHelper.GetEnumList<AddressValidationStatusType>()
                .Where(x => x.Value != AddressValidationStatusType.NotChecked &&
                            x.Value != AddressValidationStatusType.Pending &&
                            x.Value != AddressValidationStatusType.Error)
                .ToList()
                .ForEach(status =>
                {
                    sampleOrder.ShipAddressValidationStatus = (int)status.Value;
                    testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });
                    webClient.Verify(x => x.ValidateAddress(It.IsAny<AddressAdapter>()), Times.Never);
                });
        }

        [Fact]
        public void Validate_CallsWebClient_IfValidationIsNeeded()
        {
            AddressAdapter address = new AddressAdapter()
            {
                Street1 = "Street 1",
                Street2 = "Street 2",
                City = "City",
                StateProvCode = "MO",
                PostalCode = "63102",
                CountryCode = "US"
            };

            webClient.Setup(x => x.ValidateAddress(address))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>() });

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            webClient.Verify(x => x.ValidateAddress(It.Is<AddressAdapter>(a =>
                    a.Street1 == address.Street1 &&
                    a.Street2 == address.Street2 &&
                    a.City == address.City &&
                    a.StateProvCode == address.StateProvCode &&
                    a.PostalCode == address.PostalCode &&
                    a.CountryCode == address.CountryCode
                )));
        }

        [Fact]
        public void Validate_CallsSave_WithOriginalAddress()
        {
            ValidatedAddressEntity originalAddress = null;
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => originalAddress = x);

            Assert.Equal("Street 1", originalAddress.Street1);
            Assert.Equal("Street 2", originalAddress.Street2);
            Assert.Equal("Street 3", originalAddress.Street3);
            Assert.Equal("City", originalAddress.City);
            Assert.Equal("MO", originalAddress.StateProvCode);
            Assert.Equal("63102", originalAddress.PostalCode);
            Assert.Equal("US", originalAddress.CountryCode);
            Assert.True(originalAddress.IsOriginal);
        }

        [Fact]
        public void Validate_CallsSave_WithSuggestedAddresses()
        {
            List<ValidatedAddressEntity> suggestedAddresses = null;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => suggestedAddresses = y.OrderBy(z => z.Street1).ToList());

            Assert.Equal(result1.Street1, suggestedAddresses[0].Street1);
            Assert.Equal(result1.Street2, suggestedAddresses[0].Street2);
            Assert.Equal(result1.Street3, suggestedAddresses[0].Street3);
            Assert.Equal(result1.City, suggestedAddresses[0].City);
            Assert.Equal(result1.StateProvCode, suggestedAddresses[0].StateProvCode);
            Assert.Equal(result1.PostalCode, suggestedAddresses[0].PostalCode);
            Assert.Equal(result1.CountryCode, suggestedAddresses[0].CountryCode);
            Assert.False(suggestedAddresses[0].IsOriginal);

            Assert.Equal(result2.Street1, suggestedAddresses[1].Street1);
            Assert.Equal(result2.Street2, suggestedAddresses[1].Street2);
            Assert.Equal(result2.Street3, suggestedAddresses[1].Street3);
            Assert.Equal(result2.City, suggestedAddresses[1].City);
            Assert.Equal(result2.StateProvCode, suggestedAddresses[1].StateProvCode);
            Assert.Equal(result2.PostalCode, suggestedAddresses[1].PostalCode);
            Assert.Equal(result2.CountryCode, suggestedAddresses[1].CountryCode);
            Assert.False(suggestedAddresses[1].IsOriginal);
        }

        [Fact]
        public void Validate_UpdatesSuggestionCount_WithSuggestedAddresses()
        {
            //ValidatedAddressEntity orderAddress = new ValidatedAddressEntity();
            //ValidatedAddressEntity suggestion1 = new ValidatedAddressEntity();
            //ValidatedAddressEntity suggestion2 = new ValidatedAddressEntity();

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(2, sampleOrder.ShipAddressValidationSuggestionCount);
        }

        [Fact]
        public void Validate_SetsValidationStatusToNotValid_WhenNoResultsAreReturned()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>() });
            
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.BadAddress, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_SetsValidationStatusToValid_WhenOneValidIdenticalAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.Valid, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_DoesNotChangeAddress_WhenOneValidIdenticalAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = sampleOrder.ShipStreet1;
            result1.Street2 = sampleOrder.ShipStreet2;
            result1.Street3 = sampleOrder.ShipStreet3;
            result1.City = sampleOrder.ShipCity;
            result1.StateProvCode = sampleOrder.ShipStateProvCode;
            result1.CountryCode = sampleOrder.ShipCountryCode;
            result1.PostalCode = sampleOrder.ShipPostalCode;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal("Street 1", sampleOrder.ShipStreet1);
            Assert.Equal("Street 2", sampleOrder.ShipStreet2);
            Assert.Equal("Street 3", sampleOrder.ShipStreet3);
            Assert.Equal("City", sampleOrder.ShipCity);
            Assert.Equal("MO", sampleOrder.ShipStateProvCode);
            Assert.Equal("63102", sampleOrder.ShipPostalCode);
            Assert.Equal("US", sampleOrder.ShipCountryCode);
        }

        [Fact]
        public void Validate_DoesNotChangeAddress_WhenAddressShouldBeAdjustedButStoreIsNotSetToApply()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "FO";
            result1.CountryCode = "BA";
            result1.PostalCode = "12345";

            testObject.Validate(sampleOrder, "Ship", false, (x, y) => { });

            Assert.Equal("Street 1", sampleOrder.ShipStreet1);
            Assert.Equal("Street 2", sampleOrder.ShipStreet2);
            Assert.Equal("Street 3", sampleOrder.ShipStreet3);
            Assert.Equal("City", sampleOrder.ShipCity);
            Assert.Equal("MO", sampleOrder.ShipStateProvCode);
            Assert.Equal("63102", sampleOrder.ShipPostalCode);
            Assert.Equal("US", sampleOrder.ShipCountryCode);
        }

        [Fact]
        public void Validate_SetsStatusToNeedsAttention_WhenAddressShouldBeAdjustedButStoreIsNotSetToApply()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "FO";
            result1.CountryCode = "BA";
            result1.PostalCode = "12345";

            testObject.Validate(sampleOrder, "Ship", false, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_SetsAddressDetails_WhenAddressShouldBeAdjustedButStoreIsNotSetToApply()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "AA";
            result1.CountryCode = "US";
            result1.PostalCode = "12345";
            result1.ResidentialStatus = ValidationDetailStatusType.No;
            result1.POBox = ValidationDetailStatusType.Yes;

            testObject.Validate(sampleOrder, "Ship", false, (x, y) => { });

            Assert.Equal(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipResidentialStatus);
            Assert.Equal(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipPOBox);
            Assert.Equal(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipMilitaryAddress);
            Assert.Equal(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipUSTerritory);
        }

        [Fact]
        public void Validate_SetsValidationStatusToAdjusted_WhenOneValidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.Fixed, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_ChangesAddress_WhenOneValidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "FO";
            result1.CountryCode = "BA";
            result1.PostalCode = "12345";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal("Foo 1", sampleOrder.ShipStreet1);
            Assert.Equal("Foo 2", sampleOrder.ShipStreet2);
            Assert.Equal("Foo 3", sampleOrder.ShipStreet3);
            Assert.Equal("Foo City", sampleOrder.ShipCity);
            Assert.Equal("FO", sampleOrder.ShipStateProvCode);
            Assert.Equal("12345", sampleOrder.ShipPostalCode);
            Assert.Equal("BA", sampleOrder.ShipCountryCode);
        }

        [Fact]
        public void Validate_SetsAddressDetails_WhenOneValidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "AA";
            result1.CountryCode = "US";
            result1.PostalCode = "12345";
            result1.ResidentialStatus = ValidationDetailStatusType.No;
            result1.POBox = ValidationDetailStatusType.Yes;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipResidentialStatus);
            Assert.Equal(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipPOBox);
            Assert.Equal(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipMilitaryAddress);
            Assert.Equal(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipUSTerritory);
        }

        [Fact]
        public void Validate_DoesNotChangeOriginalAddress_WhenOneValidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "FO";
            result1.CountryCode = "BA";
            result1.PostalCode = "12345";

            ValidatedAddressEntity originalAddress = null;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { originalAddress = x; });

            Assert.Equal("Street 1", originalAddress.Street1);
            Assert.Equal("Street 2", originalAddress.Street2);
            Assert.Equal("Street 3", originalAddress.Street3);
            Assert.Equal("City", originalAddress.City);
            Assert.Equal("MO", originalAddress.StateProvCode);
            Assert.Equal("63102", originalAddress.PostalCode);
            Assert.Equal("US", originalAddress.CountryCode);
        }

        [Fact]
        public void Validate_SetsValidationStatusToNeedsAttention_WhenOneInvalidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_DoesNotChangeAddress_WhenOneInvalidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal("Street 1", sampleOrder.ShipStreet1);
            Assert.Equal("Street 2", sampleOrder.ShipStreet2);
            Assert.Equal("Street 3", sampleOrder.ShipStreet3);
            Assert.Equal("City", sampleOrder.ShipCity);
            Assert.Equal("MO", sampleOrder.ShipStateProvCode);
            Assert.Equal("63102", sampleOrder.ShipPostalCode);
            Assert.Equal("US", sampleOrder.ShipCountryCode);
        }

        [Fact]
        public void Validate_SetsValidationStatusToNeedsAttention_WhenMultipleInvalidAddressesAreReturned()
        {
            result1.Street1 = "Foo";
            result2.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_DoesNotChangeAddress_WhenMultipleInvalidAddressesAreReturned()
        {
            result1.Street1 = "Foo";
            result2.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.Equal("Street 1", sampleOrder.ShipStreet1);
            Assert.Equal("Street 2", sampleOrder.ShipStreet2);
            Assert.Equal("Street 3", sampleOrder.ShipStreet3);
            Assert.Equal("City", sampleOrder.ShipCity);
            Assert.Equal("MO", sampleOrder.ShipStateProvCode);
            Assert.Equal("63102", sampleOrder.ShipPostalCode);
            Assert.Equal("US", sampleOrder.ShipCountryCode);
        }

        [Fact]
        public void Validate_OrderStatusIsError_WhenWebClientThrowsAddressValidationException()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, (validatedAddressEntity, addressList) =>
            {
            });

            Assert.Equal(AddressValidationStatusType.Error, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_StoresValidationError_WhenWebClientThrowsAddressValidationException()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, (validatedAddressEntity, addressList) =>
            {
            });

            Assert.True(sampleOrder.ShipAddressValidationError.StartsWith("Error communicating with Address Validation Server.", StringComparison.InvariantCulture));
        }

        [Fact]
        public void Validate_StoresValidationError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = errorMessage });

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.Equal(errorMessage, sampleOrder.ShipAddressValidationError);
        }

        [Fact]
        public void Validate_SetsStatusToError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>() });

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.Equal(AddressValidationStatusType.BadAddress, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [Fact]
        public void Validate_UnsetsValidationError_WhenWebClientReturnsErrorMessageThenReturnsNoErrorMessage()
        {
            errorMessage = "blah";
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = errorMessage });


            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.Equal(errorMessage, sampleOrder.ShipAddressValidationError);

            errorMessage = string.Empty;
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = string.Empty});

            sampleOrder.ShipAddressValidationStatus = (int)AddressValidationStatusType.Pending;

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.Equal(string.Empty, sampleOrder.ShipAddressValidationError);
        }
    }
}
