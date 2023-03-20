using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class iParcelTelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly ShipmentEntity shipment;

		public iParcelTelemetryMutatorTest()
		{
			var package = new IParcelPackageEntity
			{
				DeclaredValue = 794.8m,
				DimsAddWeight = true,
				DimsHeight = 3.5,
				DimsLength = 4.5,
				DimsWeight = 5.5,
				DimsWidth = 6.5,
				DimsProfileID = 12345,
				Insurance = true,
				InsuranceValue = 29756.7m,
				InsurancePennyOne = true,
				ParcelNumber = "ParcelNumber",
				SkuAndQuantities = "SkuAndQuantities",
				TrackingNumber = "TrackingNumber",
				Weight = 7.5
			};

			shipment = new ShipmentEntity()
			{
				IParcel = new IParcelShipmentEntity
				{
					Reference = "Reference",
					IsDeliveryDutyPaid = true,
					Service = (int) iParcelServiceType.Saver,
					RequestedLabelFormat = (int) ThermalLanguage.ZPL,
					TrackByEmail = true,
					TrackBySMS = true,
					Packages = { package }
				}
			};

			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new iParcelTelemetryMutator();
			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Reference", "Reference"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.IsDeliveryDutyPaid", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Service", "Saver"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.RequestedLabelFormat", "Thermal - ZPL"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.TrackByEmail", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.TrackBySMS", "True"));
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromPackage()
		{
			var testObject = new iParcelTelemetryMutator();
			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DeclaredValue", "794.8"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsAddWeight", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsHeight", "3.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsLength", "4.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsProfileID", "12345"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsWeight", "5.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.DimsWidth", "6.5"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.Insurance", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.InsuranceValue", "29756.7"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.InsurancePennyOne", "True"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.ParcelNumber", "ParcelNumber"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.SkuAndQuantities", "SkuAndQuantities"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.TrackingNumber", "TrackingNumber"));
			trackedDurationEventMock.Verify(x =>
				x.AddProperty("Label.iParcel.Package.1.Weight", "7.5"));
		}
	}
}