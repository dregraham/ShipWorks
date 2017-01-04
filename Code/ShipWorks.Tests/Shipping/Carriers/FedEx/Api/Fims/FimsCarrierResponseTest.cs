using Autofac.Extras.Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    public class FimsCarrierResponseTest
    {
        [Fact]
        public void Process_SetsAcutalLabelFormatToNull_WhenResponseLabelFormatIsI()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IFimsShipResponse>()
                    .Setup(x => x.LabelFormat)
                    .Returns("I");

                var shipment = new ShipmentEntity()
                {
                    FedEx = new FedExShipmentEntity()
                };
                shipment.FedEx.Packages.Add(new FedExPackageEntity());

                mock.Provide<ShipmentEntity>(shipment);

                var testObject = mock.Create<FimsCarrierResponse>();
                testObject.Process();

                Assert.Null(shipment.ActualLabelFormat);
            }
        }

        [Fact]
        public void Process_SetsAcutalLabelFormatToZPL_WhenResponseLabelFormatIsZ()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IFimsShipResponse>()
                    .Setup(x => x.LabelFormat)
                    .Returns("Z");

                var shipment = new ShipmentEntity()
                {
                    FedEx = new FedExShipmentEntity()
                };
                shipment.FedEx.Packages.Add(new FedExPackageEntity());

                mock.Provide<ShipmentEntity>(shipment);

                var testObject = mock.Create<FimsCarrierResponse>();
                testObject.Process();

                Assert.Equal((int) ThermalLanguage.ZPL, shipment.ActualLabelFormat);
            }
        }
    }
}
