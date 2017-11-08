using System;
using Autofac.Extras.Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    public class FedExLabelSpecificationResponseManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExLabelSpecificationResponseManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(ThermalLanguage.EPL, ThermalLanguage.EPL)]
        [InlineData(ThermalLanguage.ZPL, ThermalLanguage.ZPL)]
        [InlineData(ThermalLanguage.None, null)]
        public void Test_NonLtlFreight(ThermalLanguage language, ThermalLanguage? expected)
        {
            var shipment = Create.Shipment().AsFedEx().Set(x => x.RequestedLabelFormat, (int) language).Build();
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var testObject = mock.Create<FedExLabelSpecificationResponseManipulator>();

            var result = testObject.Manipulate(null, null, shipment);

            Assert.Equal(expected, (ThermalLanguage?) result.Value.ActualLabelFormat);
        }

        [Theory]
        [InlineData(ThermalLanguage.EPL, null)]
        [InlineData(ThermalLanguage.ZPL, null)]
        [InlineData(ThermalLanguage.None, null)]
        public void Test_LtlFreight(ThermalLanguage language, ThermalLanguage? expected)
        {
            var shipment = Create.Shipment().AsFedEx().Set(x => x.RequestedLabelFormat, (int)language).Build();
            shipment.FedEx.Service = (int)FedExServiceType.FedExFreightEconomy;

            var testObject = mock.Create<FedExLabelSpecificationResponseManipulator>();

            var result = testObject.Manipulate(null, shipment);

            Assert.Equal(expected, (ThermalLanguage?)result.Value.ActualLabelFormat);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
