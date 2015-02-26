using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.AddressValidation
{
    [TestClass]
    public class AddressValidatorTest
    {
        private AddressValidationResult result1;
        private AddressValidationResult result2;
        private AddressValidationWebClientValidateAddressResult results;
        private OrderEntity sampleOrder;
        private Mock<IAddressValidationWebClient> webClient;
        private AddressValidator testObject;
        private string errorMessage;

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void Validate_CallsSave_WithOriginalAddress()
        {
            ValidatedAddressEntity originalAddress = null;
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => originalAddress = x);

            Assert.AreEqual("Street 1", originalAddress.Street1);
            Assert.AreEqual("Street 2", originalAddress.Street2);
            Assert.AreEqual("Street 3", originalAddress.Street3);
            Assert.AreEqual("City", originalAddress.City);
            Assert.AreEqual("MO", originalAddress.StateProvCode);
            Assert.AreEqual("63102", originalAddress.PostalCode);
            Assert.AreEqual("US", originalAddress.CountryCode);
            Assert.IsTrue(originalAddress.IsOriginal);
        }

        [TestMethod]
        public void Validate_CallsSave_WithSuggestedAddresses()
        {
            List<ValidatedAddressEntity> suggestedAddresses = null;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => suggestedAddresses = y.OrderBy(z => z.Street1).ToList());

            Assert.AreEqual(result1.Street1, suggestedAddresses[0].Street1);
            Assert.AreEqual(result1.Street2, suggestedAddresses[0].Street2);
            Assert.AreEqual(result1.Street3, suggestedAddresses[0].Street3);
            Assert.AreEqual(result1.City, suggestedAddresses[0].City);
            Assert.AreEqual(result1.StateProvCode, suggestedAddresses[0].StateProvCode);
            Assert.AreEqual(result1.PostalCode, suggestedAddresses[0].PostalCode);
            Assert.AreEqual(result1.CountryCode, suggestedAddresses[0].CountryCode);
            Assert.IsFalse(suggestedAddresses[0].IsOriginal);

            Assert.AreEqual(result2.Street1, suggestedAddresses[1].Street1);
            Assert.AreEqual(result2.Street2, suggestedAddresses[1].Street2);
            Assert.AreEqual(result2.Street3, suggestedAddresses[1].Street3);
            Assert.AreEqual(result2.City, suggestedAddresses[1].City);
            Assert.AreEqual(result2.StateProvCode, suggestedAddresses[1].StateProvCode);
            Assert.AreEqual(result2.PostalCode, suggestedAddresses[1].PostalCode);
            Assert.AreEqual(result2.CountryCode, suggestedAddresses[1].CountryCode);
            Assert.IsFalse(suggestedAddresses[1].IsOriginal);
        }

        [TestMethod]
        public void Validate_UpdatesSuggestionCount_WithSuggestedAddresses()
        {
            //ValidatedAddressEntity orderAddress = new ValidatedAddressEntity();
            //ValidatedAddressEntity suggestion1 = new ValidatedAddressEntity();
            //ValidatedAddressEntity suggestion2 = new ValidatedAddressEntity();

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(2, sampleOrder.ShipAddressValidationSuggestionCount);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToNotValid_WhenNoResultsAreReturned()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>() });
            
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.BadAddress, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToValid_WhenOneValidIdenticalAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.Valid, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
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

            Assert.AreEqual("Street 1", sampleOrder.ShipStreet1);
            Assert.AreEqual("Street 2", sampleOrder.ShipStreet2);
            Assert.AreEqual("Street 3", sampleOrder.ShipStreet3);
            Assert.AreEqual("City", sampleOrder.ShipCity);
            Assert.AreEqual("MO", sampleOrder.ShipStateProvCode);
            Assert.AreEqual("63102", sampleOrder.ShipPostalCode);
            Assert.AreEqual("US", sampleOrder.ShipCountryCode);
        }

        [TestMethod]
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

            Assert.AreEqual("Street 1", sampleOrder.ShipStreet1);
            Assert.AreEqual("Street 2", sampleOrder.ShipStreet2);
            Assert.AreEqual("Street 3", sampleOrder.ShipStreet3);
            Assert.AreEqual("City", sampleOrder.ShipCity);
            Assert.AreEqual("MO", sampleOrder.ShipStateProvCode);
            Assert.AreEqual("63102", sampleOrder.ShipPostalCode);
            Assert.AreEqual("US", sampleOrder.ShipCountryCode);
        }

        [TestMethod]
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

            Assert.AreEqual(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
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

            Assert.AreEqual(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipResidentialStatus);
            Assert.AreEqual(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipPOBox);
            Assert.AreEqual(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipMilitaryAddress);
            Assert.AreEqual(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipUSTerritory);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToAdjusted_WhenOneValidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.Fixed, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
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

            Assert.AreEqual("Foo 1", sampleOrder.ShipStreet1);
            Assert.AreEqual("Foo 2", sampleOrder.ShipStreet2);
            Assert.AreEqual("Foo 3", sampleOrder.ShipStreet3);
            Assert.AreEqual("Foo City", sampleOrder.ShipCity);
            Assert.AreEqual("FO", sampleOrder.ShipStateProvCode);
            Assert.AreEqual("12345", sampleOrder.ShipPostalCode);
            Assert.AreEqual("BA", sampleOrder.ShipCountryCode);
        }

        [TestMethod]
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

            Assert.AreEqual(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipResidentialStatus);
            Assert.AreEqual(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipPOBox);
            Assert.AreEqual(ValidationDetailStatusType.Yes, (ValidationDetailStatusType)sampleOrder.ShipMilitaryAddress);
            Assert.AreEqual(ValidationDetailStatusType.No, (ValidationDetailStatusType)sampleOrder.ShipUSTerritory);
        }

        [TestMethod]
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

            Assert.AreEqual("Street 1", originalAddress.Street1);
            Assert.AreEqual("Street 2", originalAddress.Street2);
            Assert.AreEqual("Street 3", originalAddress.Street3);
            Assert.AreEqual("City", originalAddress.City);
            Assert.AreEqual("MO", originalAddress.StateProvCode);
            Assert.AreEqual("63102", originalAddress.PostalCode);
            Assert.AreEqual("US", originalAddress.CountryCode);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToNeedsAttention_WhenOneInvalidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_DoesNotChangeAddress_WhenOneInvalidAddressIsReturned()
        {
            results.AddressValidationResults.Remove(result2);
            result1.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual("Street 1", sampleOrder.ShipStreet1);
            Assert.AreEqual("Street 2", sampleOrder.ShipStreet2);
            Assert.AreEqual("Street 3", sampleOrder.ShipStreet3);
            Assert.AreEqual("City", sampleOrder.ShipCity);
            Assert.AreEqual("MO", sampleOrder.ShipStateProvCode);
            Assert.AreEqual("63102", sampleOrder.ShipPostalCode);
            Assert.AreEqual("US", sampleOrder.ShipCountryCode);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToNeedsAttention_WhenMultipleInvalidAddressesAreReturned()
        {
            result1.Street1 = "Foo";
            result2.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_DoesNotChangeAddress_WhenMultipleInvalidAddressesAreReturned()
        {
            result1.Street1 = "Foo";
            result2.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual("Street 1", sampleOrder.ShipStreet1);
            Assert.AreEqual("Street 2", sampleOrder.ShipStreet2);
            Assert.AreEqual("Street 3", sampleOrder.ShipStreet3);
            Assert.AreEqual("City", sampleOrder.ShipCity);
            Assert.AreEqual("MO", sampleOrder.ShipStateProvCode);
            Assert.AreEqual("63102", sampleOrder.ShipPostalCode);
            Assert.AreEqual("US", sampleOrder.ShipCountryCode);
        }

        [TestMethod]
        public void Validate_OrderStatusIsError_WhenWebClientThrowsAddressValidationException()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, (validatedAddressEntity, addressList) =>
            {
            });

            Assert.AreEqual(AddressValidationStatusType.Error, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_StoresValidationError_WhenWebClientThrowsAddressValidationException()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, (validatedAddressEntity, addressList) =>
            {
            });

            Assert.IsTrue(sampleOrder.ShipAddressValidationError.StartsWith("Error communicating with Address Validation Server.", StringComparison.InvariantCulture));
        }

        [TestMethod]
        public void Validate_StoresValidationError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = errorMessage });

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(errorMessage, sampleOrder.ShipAddressValidationError);
        }

        [TestMethod]
        public void Validate_SetsStatusToError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>() });

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(AddressValidationStatusType.BadAddress, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_UnsetsValidationError_WhenWebClientReturnsErrorMessageThenReturnsNoErrorMessage()
        {
            errorMessage = "blah";
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = errorMessage });


            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(errorMessage, sampleOrder.ShipAddressValidationError);

            errorMessage = string.Empty;
            webClient.Setup(x => x.ValidateAddress(It.IsAny<AddressAdapter>()))
                .Returns(new AddressValidationWebClientValidateAddressResult() { AddressValidationResults = new List<AddressValidationResult>(), AddressValidationError = string.Empty});

            sampleOrder.ShipAddressValidationStatus = (int)AddressValidationStatusType.Pending;

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(string.Empty, sampleOrder.ShipAddressValidationError);
        }
    }
}
