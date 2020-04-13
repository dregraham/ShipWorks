using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Services.Telemetry;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Telemetry
{
    public class UpsTelemetryMutatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedDurationEvent> trackedDurationEventMock;
        private readonly ShipmentEntity shipment;

        public UpsTelemetryMutatorTest()
        {
            var package = new UpsPackageEntity
            {
                Weight = 1,
                DeclaredValue = 1.0m,
                DryIceEnabled = true,
                DryIceIsForMedicalUse = true,
                DryIceRegulationSet = (int) UpsDryIceRegulationSet.Cfr,
                DryIceWeight = 1,
                DimsHeight = 50,
                DimsLength = 50,
                DimsWidth = 75,
                Insurance = true,
                InsurancePennyOne = true,
                InsuranceValue = 1.0m,
                PackagingType = (int) UpsPackagingType.Box25Kg,
                VerbalConfirmationEnabled = true,
                VerbalConfirmationName = "Test",
                VerbalConfirmationPhone = "4445556666",
                VerbalConfirmationPhoneExtension = "ext1"

            };

            shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity
                {
                    CarbonNeutral = true,
                    Cn22Number = "12345",
                    CodAmount = 1.0m,
                    CodEnabled = true,
                    CodPaymentType = (int) UpsCodPaymentType.Cash,
                    CommercialInvoiceComments = "Comment",
                    CommercialInvoiceFreight = 1.0m,
                    CommercialInvoiceInsurance = 1.0m,
                    CommercialInvoiceOther = 1.0m,
                    CommercialInvoicePurpose = (int) UpsExportReason.Sale,
                    CommercialInvoiceTermsOfSale = (int) UpsTermsOfSale.CostFreight,
                    CommercialPaperlessInvoice = true,
                    CostCenter = "CostCenter",
                    CustomsDescription = "Description",
                    CustomsDocumentsOnly = true,
                    DeliveryConfirmation = (int) UpsDeliveryConfirmationType.AdultSignature,
                    EmailNotifyFrom = "From",
                    EmailNotifyMessage = "Message",
                    EmailNotifyOther = 1,
                    EmailNotifyOtherAddress = "OtherAddress",
                    EmailNotifyRecipient = 1,
                    EmailNotifySender = 1,
                    EmailNotifySubject = (int) UpsEmailNotificationSubject.TrackingNumber,
                    Endorsement = (int) UspsEndorsementType.AddressServiceRequested,
                    IrregularIndicator = 1,
                    NegotiatedRate = true,
                    PaperlessAdditionalDocumentation = true,
                    PayorAccount = "Account",
                    PayorCountryCode = "us",
                    PayorPostalCode = "63102",
                    PayorType = (int) UpsPayorType.Receiver,
                    PublishedCharges = 1.0m,
                    ReferenceNumber = "12345",
                    ReferenceNumber2 = "12345",
                    RequestedLabelFormat = (int) ThermalLanguage.EPL,
                    ReturnContents = "Return",
                    ReturnService = (int) UpsReturnServiceType.ElectronicReturnLabel,
                    ReturnUndeliverableEmail = "ReturnEmail",
                    SaturdayDelivery = true,
                    Service = (int) UpsServiceType.Ups2DayAir,
                    ShipmentChargeAccount = "12345",
                    ShipmentChargeCountryCode = "us",
                    ShipmentChargePostalCode = "63102",
                    ShipmentChargeType = (int) UpsShipmentChargeType.BillReceiver,
                    ShipperRelease = true,
                    Subclassification = (int) UpsPostalSubclassificationType.Machineable,
                    WorldShipStatus = (int) WorldShipStatusType.Completed,
                    Packages = {package}
                }
            };

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            trackedDurationEventMock = mock.MockRepository.Create<ITrackedDurationEvent>();
        }

        [Fact]
        public void MutateTelemetry_SetsTelemetryPropertiesFromShipment()
        {
            var testObject = new UpsTelemetryMutator();
            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);

            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CarbonNeutral", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Cn22Number", "12345"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodAmount", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CodPaymentType", "Any (No Cash)"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceComments", "Comment"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceFreight", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceInsurance", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceOther", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoicePurpose", "Sale"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialInvoiceTermsOfSale", "Cost and Freight"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CommercialPaperlessInvoice", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CostCenter", "CostCenter"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CustomsDescriptions", "Description"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.CustomsDocumentsOnly", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.DeliveryConfirmation", "Adult Signature Required"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyFrom", "From"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyMessage", "Message"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyOther", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyOtherAddress", "OtherAddress"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifyRecipient", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotifySender", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.EmailNotificationSubject", "Tracking Number"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Endorsement", "Address Service Requested"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.IrregularIndicator", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.NegotiatedRate", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PaperlessAdditionalDocumentation", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorAccount", "Account"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorCountryCode", "us"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "63102"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorType", "Receiver"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PublishedCharges", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReferenceNumber", "12345"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReferenceNumber2", "12345"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.RequestedLabelFormat", "Thermal - EPL"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReturnContents", "Return"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "Electronic Return Label"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ReturnUndeliverableEmail", "ReturnEmail"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.SaturdayDelivery", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Service", "UPS 2nd Day Air®"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeAccount", "12345"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeCountryCode", "us"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargePostalCode", "63102"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipmentChargeType", "Bill Receiver"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.ShipperRelease", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.PayorPostalCode", "Machineable"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.WorldShipStatus", "Completed"));
        }

        [Fact]
        public void MutateTelemetry_SetsTelemetryPropertiesFromPackage()
        {
            var testObject = new UpsTelemetryMutator();

            testObject.MutateTelemetry(trackedDurationEventMock.Object, shipment);
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.BillableWeight", "1349"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.DeclaredValue", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.DryIceEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.DryIceIsForMedicalUse", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.DryIceRegulationSet", "CFR"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.DryIceWeight", "1"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.Girth", "200"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.Insurance", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.InsurancePennyOne", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.InsuranceValue", "1.0"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.IsLargePackage", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.LongestSide", "75"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.PackagingType", "UPS 25 KG Box®"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.SecondLongestSize", "50"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.VerbalConfirmationEnabled", "True"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.VerbalConfirmationName", "Test"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.VerbalConfirmationPhone", "4445556666"));
            trackedDurationEventMock.Verify(x => x.AddProperty("Label.Ups.Package.1.VerbalConfirmationPhoneExtension", "ext1"));
        }
    }
}