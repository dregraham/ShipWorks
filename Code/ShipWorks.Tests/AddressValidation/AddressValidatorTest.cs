using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;

namespace ShipWorks.Tests.AddressValidation
{
    [TestClass]
    public class AddressValidatorTest
    {
        private AddressValidationResult result1;
        private AddressValidationResult result2;
        private List<AddressValidationResult> results; 
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

            results = new List<AddressValidationResult> {result1, result2};

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
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage))
                .Returns(results);
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
                sampleOrder.ShipAddressValidationStatus = (int) status.Value;
                testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });
                webClient.Verify(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage), Times.Never);
            });
        }

        [TestMethod]
        public void Validate_CallsWebClient_IfValidationIsNeeded()
        {
            webClient.Setup(x => x.ValidateAddress("Street 1", "Street 2", "City", "MO", "63102", out errorMessage))
                .Returns(new List<AddressValidationResult>());
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });
            webClient.Verify(x => x.ValidateAddress("Street 1", "Street 2", "City", "MO", "63102", out errorMessage));
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
            List< ValidatedAddressEntity> suggestedAddresses = null;

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
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage))
                .Returns((List<AddressValidationResult>)null);
            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.NotValid, (AddressValidationStatusType) sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToValid_WhenOneValidIdenticalAddressIsReturned()
        {
            results.Remove(result2);
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.Valid, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_DoesNotChangeAddress_WhenOneValidIdenticalAddressIsReturned()
        {
            results.Remove(result2);
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
            results.Remove(result2);
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
            results.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "FO";
            result1.CountryCode = "BA";
            result1.PostalCode = "12345";

            testObject.Validate(sampleOrder, "Ship", false, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.NeedsAttention, (AddressValidationStatusType) sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_SetsAddressDetails_WhenAddressShouldBeAdjustedButStoreIsNotSetToApply()
        {
            results.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "AA";
            result1.CountryCode = "US";
            result1.PostalCode = "12345";
            result1.ResidentialStatus = ResidentialStatusType.Commercial;
            result1.POBox = POBoxType.POBox;

            testObject.Validate(sampleOrder, "Ship", false, (x, y) => { });

            Assert.AreEqual(ResidentialStatusType.Commercial, (ResidentialStatusType)sampleOrder.ShipResidentialStatus);
            Assert.AreEqual(POBoxType.POBox, (POBoxType)sampleOrder.ShipPOBox);
            Assert.AreEqual(MilitaryAddressType.MilitaryAddress, (MilitaryAddressType)sampleOrder.ShipMilitaryAddress);
            Assert.AreEqual(InternationalTerritoryType.NotInternationalTerritory, (InternationalTerritoryType)sampleOrder.ShipInternationalTerritory);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToAdjusted_WhenOneValidAddressIsReturned()
        {
            results.Remove(result2);
            result1.Street1 = "Foo";
            result1.IsValid = true;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.Adjusted, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_ChangesAddress_WhenOneValidAddressIsReturned()
        {
            results.Remove(result2);
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
            results.Remove(result2);
            result1.IsValid = true;
            result1.Street1 = "Foo 1";
            result1.Street2 = "Foo 2";
            result1.Street3 = "Foo 3";
            result1.City = "Foo City";
            result1.StateProvCode = "AA";
            result1.CountryCode = "US";
            result1.PostalCode = "12345";
            result1.ResidentialStatus = ResidentialStatusType.Commercial;
            result1.POBox = POBoxType.POBox;

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(ResidentialStatusType.Commercial, (ResidentialStatusType)sampleOrder.ShipResidentialStatus);
            Assert.AreEqual(POBoxType.POBox, (POBoxType)sampleOrder.ShipPOBox);
            Assert.AreEqual(MilitaryAddressType.MilitaryAddress, (MilitaryAddressType)sampleOrder.ShipMilitaryAddress);
            Assert.AreEqual(InternationalTerritoryType.NotInternationalTerritory, (InternationalTerritoryType)sampleOrder.ShipInternationalTerritory);
        }

        [TestMethod]
        public void Validate_DoesNotChangeOriginalAddress_WhenOneValidAddressIsReturned()
        {
            results.Remove(result2);
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
            results.Remove(result2);
            result1.Street1 = "Foo";

            testObject.Validate(sampleOrder, "Ship", true, (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.NeedsAttention, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_DoesNotChangeAddress_WhenOneInvalidAddressIsReturned()
        {
            results.Remove(result2);
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

            Assert.AreEqual(AddressValidationStatusType.NeedsAttention, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
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
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, ( ValidatedAddressEntity, addressList) =>
            {
            });

            Assert.AreEqual(AddressValidationStatusType.Error, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_StoresValidationError_WhenWebClientThrowsAddressValidationException()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage))
                .Throws<AddressValidationException>();

            testObject.Validate(sampleOrder, "Ship", true, ( ValidatedAddressEntity, addressList) =>
            {
            });

            Assert.AreEqual("Error communicating with Address Validation Server.", sampleOrder.ShipAddressValidationError);
        }

        [TestMethod]
        public void Validate_StoresValidationError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage));

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(errorMessage, sampleOrder.ShipAddressValidationError);
        }

        [TestMethod]
        public void Validate_SetsStatusToError_WhenWebClientReturnsErrorMessage()
        {
            errorMessage = "blah";

            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage));

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(AddressValidationStatusType.NotValid, (AddressValidationStatusType)sampleOrder.ShipAddressValidationStatus);
        }

        [TestMethod]
        public void Validate_UnsetsValidationError_WhenWebClientReturnsErrorMessageThenReturnsNoErrorMessage()
        {
            errorMessage = "blah";
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage));
            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(errorMessage, sampleOrder.ShipAddressValidationError);

            errorMessage = string.Empty;
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), out errorMessage));
            sampleOrder.ShipAddressValidationStatus = (int) AddressValidationStatusType.Pending;

            testObject.Validate(sampleOrder, "Ship", true, (a, b) => { });

            Assert.AreEqual(string.Empty, sampleOrder.ShipAddressValidationError);
        }
    }
}
