using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
    public class AsendiaTelemetryMutatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private readonly ShipmentEntity shipment;

        public AsendiaTelemetryMutatorTest()
        {
            shipment = new ShipmentEntity
            {
                Asendia = new AsendiaShipmentEntity
                {
                    AsendiaAccountID = 123,
                    Contents = (int) ShipEngineContentsType.Gift,
                    DimsAddWeight = true,
                    DimsHeight = 1,
                    DimsLength = 2,
                    DimsWidth = 3,
                    DimsWeight = 4,
                    Insurance = true,
                    InsuranceValue = 5,
                    NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender,
                    NonMachinable = true,
                    RequestedLabelFormat = (int) ThermalLanguage.ZPL,
                    Service = Interapptive.Shared.Enums.AsendiaServiceType.AsendiaOther
                }
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
        }

        [Fact]
        public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
        {
            var testObject = new AsendiaTelemetryMutator();

            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.AsendiaAccountID", "123"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.Contents", "Gift"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.DimsAddWeight", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.DimsHeight", "1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.DimsLength", "2"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.DimsWeight", "4"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.DimsWidth", "3"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.Insurance", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.InsuranceValue", "5"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.NonDelivery", "Return To Sender"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.NonMachinable", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.RequestedLabelFormat", "Thermal - ZPL"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Asendia.Service", "Asendia Other"));
        }
    }
}
