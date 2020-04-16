using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
    public class UspsTelemetryMutatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private readonly ShipmentEntity shipment;

        public UspsTelemetryMutatorTest()
        {
            shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        RequestedLabelFormat = (int)ThermalLanguage.None,
                        HidePostage = true,
                        RequireFullAddressValidation = true
                    }
                },
                ShipmentTypeCode = ShipmentTypeCode.Endicia
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
        }

        [Fact]
        public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
        {
            var testObject = new UspsTelemetryMutator();

            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Usps.RequestedLabelFormat", "Standard"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Usps.HidePostage", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Usps.RequireFullAddressValidation", "True"));
        }
    }
}
