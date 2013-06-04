using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExTotalInsuredValueManipulatorTest
    {
        private FedExTotalInsuredValueManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private object nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        [TestInitialize]
        public void Initialize()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            };

            fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };
            
            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            SetUpNewCarrierRequest(); 

            testObject = new FedExTotalInsuredValueManipulator(new FedExSettings(settingsRepository.Object));
        }

        /// <summary>
        /// Create a new carrier request with defaults
        /// </summary>
        private void SetUpNewCarrierRequest()
        {
            // Add some packages for the sum test
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.02m });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.03m });

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            nativeRequest = null;
            SetUpNewCarrierRequest();

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            nativeRequest = new CancelPendingShipmentRequest();
            SetUpNewCarrierRequest();

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            ((ProcessShipmentRequest)nativeRequest).RequestedShipment = null;
            SetUpNewCarrierRequest();

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(((ProcessShipmentRequest)nativeRequest).RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_SetsCurrencyToUSD_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.AreEqual("USD", totalInsuredValue.Currency);
        }

        [TestMethod]
        public void Manipulate_SetsAmount_Test()
        {
            const decimal expectedTotal = 300.06m;

            testObject.Manipulate(carrierRequest.Object);

            Money totalInsuredValue = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.TotalInsuredValue;
            Assert.AreEqual(expectedTotal, totalInsuredValue.Amount);
        }
    }
}
