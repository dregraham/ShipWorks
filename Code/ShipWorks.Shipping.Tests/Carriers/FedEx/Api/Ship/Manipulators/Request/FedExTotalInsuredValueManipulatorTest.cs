using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExTotalInsuredValueManipulatorTest
    {
        private readonly FedExTotalInsuredValueManipulator testObject;
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;

        public FedExTotalInsuredValueManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            processShipmentRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
            };

            shipment = Create.Shipment().AsFedEx().Build();

            IFedExAccountEntity fedExAccount = new FedExAccountEntity { AccountNumber = "123", CountryCode = "US", LastName = "Doe", FirstName = "John" };

            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(fedExAccount);

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            SetUpNewCarrierRequest();

            testObject = mock.Create<FedExTotalInsuredValueManipulator>();
        }

        /// <summary>
        /// Create a new carrier request with defaults
        /// </summary>
        private void SetUpNewCarrierRequest()
        {
            // Add some packages for the sum test
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.01m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.02m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity() { DeclaredValue = 100.03m, DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
        }

        [Theory]
        [InlineData(FedExServiceType.FedExGround, 0, false)]
        [InlineData(FedExServiceType.FedExGround, 1, true)]
        [InlineData(FedExServiceType.SmartPost, 0, false)]
        [InlineData(FedExServiceType.SmartPost, 1, false)]
        public void ShouldApply_ReturnsCorrectValue(FedExServiceType serviceType, decimal declaredValue, bool expectedValue)
        {
            shipment.FedEx.Service = (int) serviceType;
            shipment.FedEx.Packages.ForEach(p => p.DeclaredValue = declaredValue);

            Assert.Equal(expectedValue, testObject.ShouldApply(shipment));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;
            SetUpNewCarrierRequest();

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsCurrencyToUSD()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Money totalInsuredValue = processShipmentRequest.RequestedShipment.TotalInsuredValue;
            Assert.Equal("USD", totalInsuredValue.Currency);
        }

        [Fact]
        public void Manipulate_SetsAmount()
        {
            const decimal expectedTotal = 300.06m;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Money totalInsuredValue = processShipmentRequest.RequestedShipment.TotalInsuredValue;
            Assert.Equal(expectedTotal, totalInsuredValue.Amount);
        }
    }
}
