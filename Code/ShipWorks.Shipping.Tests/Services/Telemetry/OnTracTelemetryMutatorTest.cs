using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class OnTracTelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly ShipmentEntity shipment;

		public OnTracTelemetryMutatorTest()
		{
			shipment = new ShipmentEntity
			{
				OnTrac = new OnTracShipmentEntity
				{
					CodAmount = 1.0m,
					CodType = (int) OnTracCodType.SecuredFunds,
					DeclaredValue = 1.0m,
					Instructions = "Instructions",
					Insurance = true,
					InsurancePennyOne = true,
					InsuranceValue = 1.0m,
					IsCod = true,
					PackagingType = (int) OnTracPackagingType.Package,
					Reference1 = "Reference1",
					Reference2 = "Reference2",
					RequestedLabelFormat = (int) ThermalLanguage.EPL,
					SaturdayDelivery = true,
					Service = (int) OnTracServiceType.Ground,
					SignatureRequired = true
				}
			};

			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new OnTracTelemetryMutator();

			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.CodAmount", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.CodType", "Secured"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.DeclaredValue", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.Instructions", "Instructions"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.Insurance", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.InsurancePennyOne", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.InsuranceValue", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.IsCod", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.PackagingType", "Package"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.Reference1", "Reference1"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.Reference2", "Reference2"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.RequestedLabelFormat", "Thermal - EPL"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.SaturdayDelivery", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.Service", "OnTrac Ground"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.OnTrac.SignatureRequired", "True"));
		}
	}
}
