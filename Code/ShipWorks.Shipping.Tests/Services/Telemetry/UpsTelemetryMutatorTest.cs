using Autofac.Extras.Moq;
using CultureAttribute;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
	[UseCulture("en-US")]
	public class UpsTelemetryMutatorTest
	{
		private readonly AutoMock mock;
		private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
		private readonly Mock<IShipmentEntity> shipmentMock;
		private readonly Mock<IUpsShipmentEntity> upsShipment;
		private readonly Mock<IUpsPackageEntity> package;

		public UpsTelemetryMutatorTest()
		{
			mock = AutoMockExtensions.GetLooseThatReturnsMocks();
			trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();

			package = mock.Mock<IUpsPackageEntity>();
			package.SetupGet(x => x.Weight).Returns(1);
			package.SetupGet(x => x.DeclaredValue).Returns(1.0m);
			package.SetupGet(x => x.DryIceEnabled).Returns(true);
			package.SetupGet(x => x.DryIceIsForMedicalUse).Returns(true);
			package.SetupGet(x => x.DryIceRegulationSet).Returns((int) UpsDryIceRegulationSet.Cfr);
			package.SetupGet(x => x.DryIceWeight).Returns(2);
			package.SetupGet(x => x.DimsHeight).Returns(25);
			package.SetupGet(x => x.DimsLength).Returns(50);
			package.SetupGet(x => x.DimsWidth).Returns(75);
			package.SetupGet(x => x.Insurance).Returns(true);
			package.SetupGet(x => x.InsurancePennyOne).Returns(true);
			package.SetupGet(x => x.InsuranceValue).Returns(2.0m);
			package.SetupGet(x => x.PackagingType).Returns((int) UpsPackagingType.Box25Kg);
			package.SetupGet(x => x.VerbalConfirmationEnabled).Returns(true);
			package.SetupGet(x => x.VerbalConfirmationName).Returns("Test");
			package.SetupGet(x => x.VerbalConfirmationPhone).Returns("4445556666");
			package.SetupGet(x => x.VerbalConfirmationPhoneExtension).Returns("ext1");

			upsShipment = mock.MockRepository.Create<IUpsShipmentEntity>();

			upsShipment.SetupGet(x => x.CarbonNeutral).Returns(true);
			upsShipment.SetupGet(x => x.Cn22Number).Returns("12345");
			upsShipment.SetupGet(x => x.CodAmount).Returns(1.0m);
			upsShipment.SetupGet(x => x.CodEnabled).Returns(true);
			upsShipment.SetupGet(x => x.CodPaymentType).Returns((int) UpsCodPaymentType.Cash);
			upsShipment.SetupGet(x => x.CommercialInvoiceComments).Returns("Comment");
			upsShipment.SetupGet(x => x.CommercialInvoiceFreight).Returns(2.0m);
			upsShipment.SetupGet(x => x.CommercialInvoiceInsurance).Returns(3.0m);
			upsShipment.SetupGet(x => x.CommercialInvoiceOther).Returns(4.0m);
			upsShipment.SetupGet(x => x.CommercialInvoicePurpose).Returns((int) UpsExportReason.Sale);
			upsShipment.SetupGet(x => x.CommercialInvoiceTermsOfSale).Returns((int) UpsTermsOfSale.CostFreight);
			upsShipment.SetupGet(x => x.CommercialPaperlessInvoice).Returns(true);
			upsShipment.SetupGet(x => x.CostCenter).Returns("CostCenter");
			upsShipment.SetupGet(x => x.CustomsDescription).Returns("Description");
			upsShipment.SetupGet(x => x.CustomsDocumentsOnly).Returns(true);
			upsShipment.SetupGet(x => x.DeliveryConfirmation).Returns((int) UpsDeliveryConfirmationType.AdultSignature);
			upsShipment.SetupGet(x => x.EmailNotifyFrom).Returns("From");
			upsShipment.SetupGet(x => x.EmailNotifyMessage).Returns("Message");
			upsShipment.SetupGet(x => x.EmailNotifyOther).Returns(1);
			upsShipment.SetupGet(x => x.EmailNotifyOtherAddress).Returns("OtherAddress");
			upsShipment.SetupGet(x => x.EmailNotifyRecipient).Returns(1);
			upsShipment.SetupGet(x => x.EmailNotifySender).Returns(1);
			upsShipment.SetupGet(x => x.EmailNotifySubject).Returns((int) UpsEmailNotificationSubject.TrackingNumber);
			upsShipment.SetupGet(x => x.Endorsement).Returns((int) UspsEndorsementType.AddressServiceRequested);
			upsShipment.SetupGet(x => x.IrregularIndicator).Returns(1);
			upsShipment.SetupGet(x => x.NegotiatedRate).Returns(true);
			upsShipment.SetupGet(x => x.PaperlessAdditionalDocumentation).Returns(true);
			upsShipment.SetupGet(x => x.PayorAccount).Returns("Account");
			upsShipment.SetupGet(x => x.PayorCountryCode).Returns("us");
			upsShipment.SetupGet(x => x.PayorPostalCode).Returns("63102");
			upsShipment.SetupGet(x => x.PayorType).Returns((int) UpsPayorType.Receiver);
			upsShipment.SetupGet(x => x.PublishedCharges).Returns(5.0m);
			upsShipment.SetupGet(x => x.ReferenceNumber).Returns("00002");
			upsShipment.SetupGet(x => x.ReferenceNumber2).Returns("54321");
			upsShipment.SetupGet(x => x.RequestedLabelFormat).Returns((int) ThermalLanguage.EPL);
			upsShipment.SetupGet(x => x.ReturnContents).Returns("Return");
			upsShipment.SetupGet(x => x.ReturnService).Returns((int) UpsReturnServiceType.ElectronicReturnLabel);
			upsShipment.SetupGet(x => x.ReturnUndeliverableEmail).Returns("ReturnEmail");
			upsShipment.SetupGet(x => x.SaturdayDelivery).Returns(true);
			upsShipment.SetupGet(x => x.Service).Returns((int) UpsServiceType.Ups2DayAir);
			upsShipment.SetupGet(x => x.ShipmentChargeAccount).Returns("00001");
			upsShipment.SetupGet(x => x.ShipmentChargeCountryCode).Returns("us");
			upsShipment.SetupGet(x => x.ShipmentChargePostalCode).Returns("20134");
			upsShipment.SetupGet(x => x.ShipmentChargeType).Returns((int) UpsShipmentChargeType.BillReceiver);
			upsShipment.SetupGet(x => x.ShipperRelease).Returns(true);
			upsShipment.SetupGet(x => x.Subclassification).Returns((int) UpsPostalSubclassificationType.Machineable);
			upsShipment.SetupGet(x => x.WorldShipStatus).Returns((int) WorldShipStatusType.Completed);
			upsShipment.SetupGet(x => x.Packages).Returns(new[] { package.Object });

			shipmentMock = mock.MockRepository.Create<IShipmentEntity>();
			shipmentMock.SetupGet(x => x.Ups).Returns(upsShipment.Object);
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
		{
			var testObject = new UpsTelemetryMutator();
			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipmentMock.Object);

			upsShipment.VerifyGet(x => x.CarbonNeutral, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CarbonNeutral", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Cn22Number", "12345"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodAmount", "1.0"));
			upsShipment.VerifyGet(x => x.CodEnabled, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodEnabled", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodPaymentType", "Any (No Cash)"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceComments", "Comment"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceFreight", "2.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceInsurance", "3.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceOther", "4.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoicePurpose", "Sale"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceTermsOfSale", "Cost and Freight"));
			upsShipment.VerifyGet(x => x.CommercialPaperlessInvoice, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialPaperlessInvoice", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CostCenter", "CostCenter"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CustomsDescriptions", "Description"));
			upsShipment.VerifyGet(x => x.CustomsDocumentsOnly, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CustomsDocumentsOnly", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.DeliveryConfirmation", "Adult Signature Required"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyFrom", "From"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyMessage", "Message"));
			upsShipment.VerifyGet(x => x.EmailNotifyOther, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyOther", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyOtherAddress", "OtherAddress"));
			upsShipment.VerifyGet(x => x.EmailNotifyRecipient, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyRecipient", "True"));
			upsShipment.VerifyGet(x => x.EmailNotifySender, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifySender", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotificationSubject", "Tracking Number"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Endorsement", "Address Service Requested"));
			upsShipment.VerifyGet(x => x.IrregularIndicator, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.IrregularIndicator", "True"));
			upsShipment.VerifyGet(x => x.NegotiatedRate, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.NegotiatedRate", "True"));
			upsShipment.VerifyGet(x => x.PaperlessAdditionalDocumentation, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PaperlessAdditionalDocumentation", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorAccount", "Account"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorCountryCode", "us"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "63102"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorType", "Receiver"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PublishedCharges", "5.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReferenceNumber", "00002"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReferenceNumber2", "54321"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.RequestedLabelFormat", "Thermal - EPL"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReturnContents", "Return"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "Electronic Return Label"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReturnUndeliverableEmail", "ReturnEmail"));
			upsShipment.VerifyGet(x => x.SaturdayDelivery, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.SaturdayDelivery", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Service", "UPS 2nd Day Air®"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeAccount", "00001"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeCountryCode", "us"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargePostalCode", "20134"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeType", "Bill Receiver"));
			upsShipment.VerifyGet(x => x.ShipperRelease, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipperRelease", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "Machineable"));
			trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.WorldShipStatus", "Completed"));
		}

		[Fact]
		public void MutateTelemetry_SetsTelemetryPropertiesFromPackage()
		{
			var testObject = new UpsTelemetryMutator();
			testObject.MutateTelemetry(trackedDurationEventMock.Object, shipmentMock.Object);

			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.DeclaredValue", "1.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.DryIceEnabled", "True"));
			package.VerifyGet(x => x.DryIceEnabled, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.DryIceIsForMedicalUse", "True"));
			package.VerifyGet(x => x.DryIceIsForMedicalUse, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.DryIceRegulationSet", "CFR"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.DryIceWeight", "2"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.Insurance", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.InsurancePennyOne", "True"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.InsuranceValue", "2.0"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.PackagingType", "UPS 25 KG Box®"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.VerbalConfirmationEnabled", "True"));
			package.VerifyGet(x => x.VerbalConfirmationEnabled, Times.Once);
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.VerbalConfirmationName", "Test"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.VerbalConfirmationPhone", "4445556666"));
			trackedDurationEventMock.Verify(x => x.AddProperty($"Label.Ups.Package.1.VerbalConfirmationPhoneExtension", "ext1"));
		}
	}
}