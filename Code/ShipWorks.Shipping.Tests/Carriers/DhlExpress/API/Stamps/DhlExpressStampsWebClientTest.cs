using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl.API.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress.API.Stamps
{
    public class DhlExpressStampsWebClientTest
    {
        [Theory]
        [InlineData("US", "13102", "GB", "M38 9WL", false)]
        public async Task CreateInitialRate_SetsToPostal_WhenNotDomestic(
            string fromCountryCode, string fromZipPostalCode,
            string toCountryCode, string toZipPostalCode,
            bool isReturn)
        {
            var shipment = new ShipmentEntity()
            {
                RequestedLabelFormat = (int) ThermalLanguage.ZPL,
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Packages = { new DhlExpressPackageEntity() }
                },
                ShipCountryCode = toCountryCode,
                ShipPostalCode = toZipPostalCode,
                OriginCountryCode = fromCountryCode,
                OriginPostalCode = fromZipPostalCode,
                ReturnShipment = isReturn
            };

            var account = new UspsAccountEntity()
            {
                MailingPostalCode = fromZipPostalCode
            };

            var rate = DhlExpressStampsWebClient.CreateInitialRate(shipment, account);

            if (isReturn)
            {
                (toCountryCode, fromCountryCode) = (fromCountryCode, toCountryCode);
                (toZipPostalCode, fromZipPostalCode) = (fromZipPostalCode, toZipPostalCode);
            }

            Assert.Equal(toCountryCode, rate.To.Country);
            Assert.Equal(toZipPostalCode, rate.To.PostalCode);
            Assert.Null(rate.To.ZIPCode);
            Assert.Equal(fromCountryCode, rate.From.Country);
            Assert.Equal(fromZipPostalCode, rate.From.PostalCode);
            Assert.Null(rate.From.ZIPCode);
        }
    }
}
