using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Fims
{
    public class FimsCarrierResponseTest
    {
        private const string TrackingNumber = "12345";
        private const string ParcelID = "1";

        [Fact]
        public void Process_SetsShipmentTrackingNumber()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns(TrackingNumber);
                fimsResponse.SetupGet(x => x.ParcelID).Returns(ParcelID);

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal(TrackingNumber, shipmentEntity.TrackingNumber);
            }
        }

        [Fact]
        public void Process_SetsPackageTrackingNumber()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal(ParcelID, shipmentEntity.FedEx.Packages[0].TrackingNumber);
            }
        }

        [Fact]
        public void Process_SetsMasterFormIDToEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal(string.Empty, shipmentEntity.FedEx.MasterFormID);
            }
        }

        [Fact]
        public void Process_SetsShipmentCostToZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal(0, shipmentEntity.ShipmentCost);
            }
        }

        [Fact]
        public void Process_SetsShipmentRequestedLabelFormatToNone()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal((int)ThermalLanguage.None, shipmentEntity.RequestedLabelFormat);
            }
        }

        [Fact]
        public void Process_SetsActualLabelFormat()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");
                fimsResponse.SetupGet(x => x.LabelFormat).Returns("Z");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal((int)ThermalLanguage.ZPL, shipmentEntity.ActualLabelFormat);
            }
        }

        [Fact]
        public void Process_SetsFedExShipmentRequestedLabelFormatToNone()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                Assert.Equal((int)ThermalLanguage.None, shipmentEntity.FedEx.RequestedLabelFormat);
            }
        }

        [Fact]
        public void Process_SavesLabelToRepository()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var labelRepository = mock.Mock<IFimsLabelRepository>();
                var fimsResponse = mock.Mock<IFimsShipResponse>();
                var shipmentEntity = new ShipmentEntity();
                shipmentEntity.FedEx = new FedExShipmentEntity();
                shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity());
                fimsResponse.SetupGet(x => x.TrackingNumber).Returns("12345");
                fimsResponse.SetupGet(x => x.ParcelID).Returns("1");

                var testObject = new FimsCarrierResponse(shipmentEntity, fimsResponse.Object, labelRepository.Object);

                testObject.Process();

                labelRepository.Verify(r => r.SaveLabel(It.IsAny<IFimsShipResponse>(), It.IsAny<long>()));
            }
        }
    }
}