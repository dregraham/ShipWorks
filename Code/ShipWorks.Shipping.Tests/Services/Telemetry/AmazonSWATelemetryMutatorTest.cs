using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class AmazonSWATelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly ShipmentEntity shipment;

		public AmazonSWATelemetryMutatorTest()
		{
			shipment = new ShipmentEntity
			{
				AmazonSWA = new AmazonSWAShipmentEntity
				{
					Insurance = true,
					InsuranceValue = 1.0m,
					RequestedLabelFormat = (int) ThermalLanguage.EPL,
					Service = (int) AmazonSWAServiceType.Ground
				}
			};

			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new AmazonSWATelemetryMutator();

			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x => x.AddProperty("Label.AmazonSWA.Insurance", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.AmazonSWA.InsuranceValue", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.AmazonSWA.RequestedLabelFormat", "Thermal - EPL"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.AmazonSWA.Service", "Ground"));
		}
	}
}
