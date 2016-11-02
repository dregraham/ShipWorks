using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx
{
    public class FimsWebClientTest
    {
        [Fact]
        public void Ship_MakesWebCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var apiLogEntry = mock.MockRepository.Create<IApiLogEntry>();
                mock.Provide(apiLogEntry.Object);

                var fedExShipment = new FedExShipmentEntity()
                {
                    CommercialInvoicePurpose = (int) FedExCommercialInvoicePurpose.Gift,
                    ReferenceFIMS = "1234",
                    FimsAirWaybill = "123456789012"
                };

                fedExShipment.Packages.Add(new FedExPackageEntity
                {
                    DimsLength = 10,
                    DimsWidth = 10,
                    DimsHeight = 10
                });

                var shipment = new ShipmentEntity()
                {
                    OriginFirstName = "Homer",
                    OriginMiddleName = "J.",
                    OriginLastName = "Simpson",
                    OriginCompany = "ShipWorks",
                    OriginStreet1 = "1 S Memorial Dr",
                    OriginStreet2 = "Suite 2000",
                    OriginCity = "St Louis",
                    OriginStateProvCode = "MO",
                    OriginCountryCode = "US",
                    OriginPhone = "314-555-1212",
                    OriginEmail = "support@shipworks.com",
                    ShipFirstName = "Marge",
                    ShipMiddleName = "B",
                    ShipLastName = "Simpson",
                    ShipCompany = "McDonalds",
                    ShipStreet1 = "2391 W 4th Ave",
                    ShipCity = "Vancouver",
                    ShipStateProvCode = "BC",
                    ShipPostalCode = "V6K1P2",
                    ShipCountryCode = "CA",
                    ShipPhone = "604-718-1185",
                    ShipEmail = "nobody@shipworks.com",
                    TotalWeight = 2,
                    CustomsValue = 100,
                    FedEx = fedExShipment,
                    RequestedLabelFormat = (int) ThermalLanguage.ZPL
                };

                shipment.CustomsItems.Add(new ShipmentCustomsItemEntity()
                {
                    Description = "desc",
                    UnitValue = 5,
                    Weight = 10,
                    HarmonizedCode = "123",
                    CountryOfOrigin = "CA"
                });

                var mockShipRequest = mock.MockRepository.Create<IFimsShipRequest>();
                mockShipRequest.SetupGet(r => r.Username).Returns("SWORKS");
                mockShipRequest.SetupGet(r => r.Password).Returns("CRESWA8WAHAFUFR");
                mockShipRequest.SetupGet(r => r.Shipment).Returns(shipment);

                var testObject = mock.Create<FimsWebClient>();
                var response = testObject.Ship(mockShipRequest.Object);
                Assert.NotNull(response.LabelData);
                Assert.Equal("Z", response.LabelFormat);
            }
        }
    }
}
