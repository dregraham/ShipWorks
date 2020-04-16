using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
    public class EndiciaTelemetryMutatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private readonly ShipmentEntity shipment;

        public EndiciaTelemetryMutatorTest()
        {
            shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Endicia = new EndiciaShipmentEntity
                    {
                        ReferenceID = "1",
                        ReferenceID2 = "2",
                        ReferenceID3 = "3",
                        ReferenceID4 = "4",
                        RequestedLabelFormat = (int)ThermalLanguage.None,
                        ScanBasedReturn = true,
                        StealthPostage = true
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
            var testObject = new EndiciaTelemetryMutator();

            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.ReferenceID", "1"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.ReferenceID2", "2"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.ReferenceID3", "3"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.ReferenceID4", "4"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.RequestedLabelFormat", "Standard"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.ScanBasedReturn", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Endicia.StealthPostage", "True"));
        }
    }
}
