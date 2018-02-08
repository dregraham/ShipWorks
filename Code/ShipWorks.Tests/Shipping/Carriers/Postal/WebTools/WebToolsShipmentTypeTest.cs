using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.WebTools
{
    public class WebToolsShipmentTypeTest
    {
        private readonly PostalWebShipmentType testObject;

        public WebToolsShipmentTypeTest()
        {
            testObject = new PostalWebShipmentType();
        }

        [Theory]
        [InlineData(true, 9.99)]
        [InlineData(false, 6.66)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured, decimal insuranceValue)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = !insured,
                Postal = new PostalShipmentEntity
                {
                    Insurance = insured,
                    InsuranceValue = insuranceValue,
                }
            };

            ShipmentParcel parcel = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }
    }
}
