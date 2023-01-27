using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class PostalTelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly ShipmentEntity shipment;

		public PostalTelemetryMutatorTest()
		{
			shipment = new ShipmentEntity
			{
				Postal = new PostalShipmentEntity
				{
					Confirmation = (int) PostalConfirmationType.Delivery,
					CustomsContentDescription = "Description",
					CustomsContentType = (int) PostalCustomsContentType.Merchandise,
					Insurance = true,
					InsuranceValue = 1.0m,
					Memo1 = "Memo1",
					Memo2 = "Memo2",
					Memo3 = "Memo3",
					NonMachinable = true,
					NonRectangular = true,
					NoPostage = true,
					PackagingType = (int) PostalPackagingType.FlatRateMediumBox,
					Service = (int) PostalServiceType.ExpressMail
				},
				ShipmentTypeCode = ShipmentTypeCode.Express1Endicia
			};

			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new PostalTelemetryMutator();

			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Confirmation", "Delivery Confirmation"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.CustomsContentDescription", "Description"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.PostalCustomsContentType", "Merchandise"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Insurance", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.InsuranceValue", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Memo1", "Memo1"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Memo2", "Memo2"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Memo3", "Memo3"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.NonMachinable", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.NonRectangular", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.NoPostage", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.PackagingType", "Flat Rate Medium Box"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Express1Endicia.Service", "Priority Mail Express"));
		}
	}
}
