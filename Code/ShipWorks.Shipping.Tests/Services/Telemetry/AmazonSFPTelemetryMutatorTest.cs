using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class AmazonSFPTelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly ShipmentEntity shipment;

		public AmazonSFPTelemetryMutatorTest()
		{
			shipment = new ShipmentEntity
			{
				AmazonSFP = new AmazonSFPShipmentEntity
				{
					AmazonUniqueShipmentID = "AmazonUniqueShipmentID",
					CarrierName = "CarrierName",
					DeclaredValue = 27.0m,
					DeliveryExperience = (int) AmazonSFPDeliveryExperienceType.DeliveryConfirmationWithoutSignature,
					DimsAddWeight = true,
					DimsHeight = 3.5,
					DimsLength = 4.5,
					DimsWidth = 5.5,
					DimsWeight = 6.5,
					DimsProfileID = 1234,
					Insurance = true,
					InsuranceValue = 12.0m,
					Reference1 = "Reference",
					RequestedLabelFormat = (int) ThermalLanguage.ZPL,
					ShippingServiceID = "ShippingServiceID",
					ShippingServiceName = "ShippingServiceName"
				}
			};

			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new AmazonSFPTelemetryMutator();
			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.AmazonUniqueShipmentID", "AmazonUniqueShipmentID"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.CarrierName", "CarrierName"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DeclaredValue", "27.0"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DeliveryExperience", "Delivery Confirmation Without Signature"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsAddWeight", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsHeight", "3.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsLength", "4.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsWidth", "5.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsWeight", "6.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.DimsProfileID", "1234"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.Insurance", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.InsuranceValue", "12.0"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.Reference", "Reference"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.RequestedLabelFormat", "Thermal - ZPL"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.ShippingServiceID", "ShippingServiceID"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.AmazonSFP.ShippingServiceName", "ShippingServiceName"));
		}
	}
}