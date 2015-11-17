using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonInsuranceChoiceTest
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonInsuranceChoice(null));
        }

        [Fact]
        public void Constructor_ThrowsInvalidArgumentException_WhenAmazonShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonInsuranceChoice(new ShipmentEntity()));
        }

        [Fact]
        public void InsurancePennyOne_ReturnsTrue_WhenCarrierIsUsps()
        {
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { CarrierName = "STAMPS_DOT_COM" } });
            Assert.True(testObject.InsurancePennyOne);
        }

        [Theory]
        [InlineData("UPS")]
        [InlineData("FedEx")]
        public void InsurancePennyOne_ReturnsFalse_WhenCarrierIsNotUsps(string carrierName)
        {
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { CarrierName = carrierName } });
            Assert.False(testObject.InsurancePennyOne);
        }

        [Fact]
        public void InsuranceProvider_ReturnsShipWorks()
        {
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity() });
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.InsuranceProvider);
        }

        [Theory]
        [InlineData(33)]
        [InlineData(0)]
        [InlineData(123.45)]
        public void InsuranceValue_ReturnsAmazonInsuranceValue(decimal value)
        {
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity { InsuranceValue = value } });
            Assert.Equal(value, testObject.InsuranceValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured)
        {
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(new ShipmentEntity { Amazon = new AmazonShipmentEntity(), Insurance = insured });
            Assert.Equal(insured, testObject.Insured);
        }

        [Fact]
        public void Shipment_ReturnsShipment_FromConstructor()
        {
            ShipmentEntity shipment = new ShipmentEntity { Amazon = new AmazonShipmentEntity() };
            AmazonInsuranceChoice testObject = new AmazonInsuranceChoice(shipment);
            Assert.Equal(shipment, testObject.Shipment);
        }
    }
}
