using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Insurance;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPInsuranceChoiceTest
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPInsuranceChoice(null));
        }

        [Fact]
        public void Constructor_ThrowsInvalidArgumentException_WhenAmazonShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPInsuranceChoice(new ShipmentEntity()));
        }

        [Theory]
        [InlineData("STAMPS_DOT_COM")]
        [InlineData("USPS")]
        public void InsurancePennyOne_ReturnsTrue_WhenCarrierIsUsps(string carrierName)
        {
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { CarrierName = carrierName } });
            Assert.True(testObject.InsurancePennyOne);
        }

        [Theory]
        [InlineData("UPS")]
        [InlineData("FedEx")]
        public void InsurancePennyOne_ReturnsFalse_WhenCarrierIsNotUsps(string carrierName)
        {
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { CarrierName = carrierName } });
            Assert.False(testObject.InsurancePennyOne);
        }

        [Fact]
        public void InsuranceProvider_ReturnsShipWorks()
        {
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity() });
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.InsuranceProvider);
        }

        [Theory]
        [InlineData(33)]
        [InlineData(0)]
        [InlineData(123.45)]
        public void InsuranceValue_ReturnsAmazonInsuranceValue(decimal value)
        {
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { InsuranceValue = value } });
            Assert.Equal(value, testObject.InsuranceValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured)
        {
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { Insurance = insured }});
            Assert.Equal(insured, testObject.Insured);
        }

        [Fact]
        public void Shipment_ReturnsShipment_FromConstructor()
        {
            ShipmentEntity shipment = new ShipmentEntity { Amazon = new AmazonShipmentEntity() };
            AmazonSFPInsuranceChoice testObject = new AmazonSFPInsuranceChoice(shipment);
            Assert.Equal(shipment, testObject.Shipment);
        }
    }
}
