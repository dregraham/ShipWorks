using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateRecipientManipulatorTest
    {
        private FedExRateRecipientManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRateRecipientManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRateRecipientManipulator();
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_FedExShipperManipulator_ReturnsValidStreetLines()
        {
            testObject.Manipulate(carrierRequest.Object);
            
            // Make sure Address fields match
            string[] addressLines = nativeRequest.RequestedShipment.Recipient.Address.StreetLines;

            Assert.Equal(shipmentEntity.ShipStreet1, addressLines[0]);

            if (addressLines.Length > 1)
            {
                // Check address line 2
                Assert.Equal(shipmentEntity.ShipStreet2, addressLines[1]);
            }

            if (addressLines.Length > 2)
            {
                // Check address line 3
                Assert.Equal(shipmentEntity.ShipStreet3, addressLines[2]);
            }
        }

        [Fact]
        public void Manipulate_FedExRecipientManipulator_ReturnsValidRequestedShipmentRecipient()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a Recipient back
            Assert.IsAssignableFrom<Party>(nativeRequest.RequestedShipment.Recipient);

            // Make sure the Address matches what we input
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.City, shipmentEntity.ShipCity);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.CountryCode, shipmentEntity.ShipCountryCode);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.PostalCode, shipmentEntity.ShipPostalCode);
            Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode, shipmentEntity.ShipStateProvCode);
            
            // Make sure residential info matches
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipmentEntity))
            {
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.Residential, shipmentEntity.ResidentialResult);
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, true);
            }
            else
            {
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.Residential, false);
                Assert.Equal(nativeRequest.RequestedShipment.Recipient.Address.ResidentialSpecified, false);
            }
        }
    }
}
