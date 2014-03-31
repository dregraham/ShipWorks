using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.AddressValidation
{
    [TestClass]
    public class AddressValidatorTest
    {
        private OrderEntity sampleOrder;
        private Mock<IAddressValidationWebClient> webClient;
        private AddressValidator testObject;

        [TestInitialize]
        public void Initialize()
        {
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

            webClient = new Mock<IAddressValidationWebClient>();
            testObject = new AddressValidator(webClient.Object);
        }

        [TestMethod]
        public void Validate_CallsWebClient_IfValidationIsNeeded()
        {
            webClient.Setup(x => x.ValidateAddress("Street 1", "Street 2", "City", "MO", "63102"))
                .Returns(new List<AddressValidationResult>());
            testObject.Validate(sampleOrder, "Ship", (x, y) => { });
            webClient.Verify(x => x.ValidateAddress("Street 1", "Street 2", "City", "MO", "63102"));
        }

        [TestMethod]
        public void Validate_DoesNotCallSaveAction_WhenValidationIsNotNeeded()
        {
            bool called = false;
            sampleOrder.ShipAddressValidationStatus = (int) AddressValidationStatusType.NotValid;
            testObject.Validate(sampleOrder, "Ship", (x, y) => { called = true; });
            Assert.IsFalse(called);
        }
        
        [TestMethod]
        public void Validate_DoesNotCallWebClient_WhenValidationIsNotNeeded()
        {
            sampleOrder.ShipAddressValidationStatus = (int)AddressValidationStatusType.NotValid;
            testObject.Validate(sampleOrder, "Ship", (x, y) => { });
            webClient.Verify(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Validate_CallsSave_WithOriginalAddress()
        {
            AddressEntity originalAddress = null;
            testObject.Validate(sampleOrder, "Ship", (x, y) => originalAddress = x);

            Assert.AreEqual("Street 1", originalAddress.Street1);
            Assert.AreEqual("Street 2", originalAddress.Street2);
            Assert.AreEqual("Street 3", originalAddress.Street3);
            Assert.AreEqual("City", originalAddress.City);
            Assert.AreEqual("MO", originalAddress.StateProvCode);
            Assert.AreEqual("63102", originalAddress.PostalCode);
            Assert.AreEqual("US", originalAddress.CountryCode);
        }

        [TestMethod]
        public void Validate_CallsSave_WithSuggestedAddresses()
        {
            AddressValidationResult result1 = new AddressValidationResult
            {
                Street1 = "Street 1",
                Street2 = "Street 2",
                Street3 = "Street 3",
                City = "City",
                StateProvCode = "MO",
                CountryCode = "US",
                PostalCode = "63102"
            };

            AddressValidationResult result2 = new AddressValidationResult
            {
                Street1 = "Street 4",
                Street2 = "Street 5",
                Street3 = "Street 6",
                City = "City",
                StateProvCode = "IL",
                CountryCode = "US",
                PostalCode = "63102"
            };

            List<AddressEntity> suggestedAddresses = null;
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<AddressValidationResult> { result1, result2 });
            testObject.Validate(sampleOrder, "Ship", (x, y) => suggestedAddresses = y.OrderBy(z => z.Street1).ToList());

            Assert.AreEqual(result1.Street1, suggestedAddresses[0].Street1);
            Assert.AreEqual(result1.Street2, suggestedAddresses[0].Street2);
            Assert.AreEqual(result1.Street3, suggestedAddresses[0].Street3);
            Assert.AreEqual(result1.City, suggestedAddresses[0].City);
            Assert.AreEqual(result1.StateProvCode, suggestedAddresses[0].StateProvCode);
            Assert.AreEqual(result1.PostalCode, suggestedAddresses[0].PostalCode);
            Assert.AreEqual(result1.CountryCode, suggestedAddresses[0].CountryCode);

            Assert.AreEqual(result2.Street1, suggestedAddresses[1].Street1);
            Assert.AreEqual(result2.Street2, suggestedAddresses[1].Street2);
            Assert.AreEqual(result2.Street3, suggestedAddresses[1].Street3);
            Assert.AreEqual(result2.City, suggestedAddresses[1].City);
            Assert.AreEqual(result2.StateProvCode, suggestedAddresses[1].StateProvCode);
            Assert.AreEqual(result2.PostalCode, suggestedAddresses[1].PostalCode);
            Assert.AreEqual(result2.CountryCode, suggestedAddresses[1].CountryCode);
        }

        [TestMethod]
        public void Validate_SetsValidationStatusToNotValid_WhenNoResultsAreReturned()
        {
            webClient.Setup(x => x.ValidateAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((List<AddressValidationResult>)null);
            testObject.Validate(sampleOrder, "Ship", (x, y) => { });

            Assert.AreEqual(AddressValidationStatusType.NotValid, (AddressValidationStatusType) sampleOrder.ShipAddressValidationStatus);
        }
    }
}
