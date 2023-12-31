﻿using System;
using System.Data;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelLabelServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public iParcelLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = Create.Shipment(new OrderEntity()).AsIParcel(x => x.WithPackage()).Build();
        }

        [Fact]
        public async Task ProcessShipment_ThrowsArgumentNullException_WhenShipmentEntityIsNull()
        {
            var labelService = mock.Create<iParcelLabelService>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => labelService.Create(null));
        }

        [Fact]
        public async Task ProcessShipment_ThermalTypeIsZPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsZPL()
        {
            shipment.RequestedLabelFormat = (int) ThermalLanguage.ZPL;
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            Assert.Equal((int) ThermalLanguage.ZPL, shipment.ActualLabelFormat);
        }

        [Fact]
        public async Task ProcessShipment_ThermalTypeIsEPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsEPL()
        {
            shipment.RequestedLabelFormat = (int) ThermalLanguage.EPL;
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            Assert.Equal((int) ThermalLanguage.EPL, shipment.ActualLabelFormat);
        }

        [Fact]
        public async Task ProcessShipment_ThermalTypeIsNull_WhenThermalTypeSettingIsFalse()
        {
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            Assert.Null(shipment.ActualLabelFormat);
        }

        [Fact]
        public async Task ProcessShipment_DelegatesToRepositoryForAccount()
        {
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            mock.Mock<ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity>>()
                .Verify(x => x.GetAccountReadOnly(shipment.IParcel.IParcelAccountID), Times.Once);
        }

        [Fact]
        public async Task ProcessShipment_DelegatesToRepositoryForOrderDetails()
        {
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            mock.Mock<IOrderManager>().Verify(x => x.PopulateOrderDetails(shipment), Times.Once);
        }

        [Fact]
        public async Task ProcessShipment_DelegatesToServiceGateway()
        {
            var labelService = mock.Create<iParcelLabelService>();

            await labelService.Create(shipment);

            mock.Mock<IiParcelServiceGateway>()
                .Verify(g => g.SubmitShipment(It.IsAny<iParcelCredentials>(), shipment, It.IsAny<TelemetricResult<IDownloadedLabelData>>()), Times.Once);
        }

        [Fact]
        public async Task ProcessShipment_CreatesValidDownloadedLabelData()
        {
            DataSet dataSet = new DataSet();

            mock.Mock<IiParcelServiceGateway>()
                .Setup(x => x.SubmitShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>(), It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Returns(dataSet);

            var response = mock.Create<iParcelDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(dataSet));
            mock.Provide<Func<ShipmentEntity, DataSet, iParcelDownloadedLabelData>>(
                (s, d) => s == shipment && d == dataSet ? response : null);

            var labelService = mock.Create<iParcelLabelService>();

            var downloadedLabel = await labelService.Create(shipment);

            Assert.Equal(response, downloadedLabel.Value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}