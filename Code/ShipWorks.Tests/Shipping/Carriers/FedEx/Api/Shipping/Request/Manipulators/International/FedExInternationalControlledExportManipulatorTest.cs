using System;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExInternationalControlledExportManipulatorTest
    {
        private FedExInternationalControlledExportManipulator testObject;

        private ShipmentEntity shipmentEntity;

        private FedExShipRequest shipRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExInternationalControlledExportManipulatorTest()
        {
            settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            // All dry ice shipments need to use custom packaging
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            shipRequest = new FedExShipRequest(
                null,
                shipmentEntity,
                null,
                null,
                settingsRepository.Object,
                new ProcessShipmentRequest());

            testObject = new FedExInternationalControlledExportManipulator();

            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
        }

        [Fact]
        public void Manipulate_InternationalControlledExportIsNull_WhenInternationalControlledExportDetailTypeIsNone_Test()
        {
            shipRequest.ShipmentEntity.FedEx.IntlExportDetailType =
                (int)FedExInternationalControlledExportType.None;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Null(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ICEDTypeIsCorrect_WhenConverted_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea036;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DEA_036,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea236;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DEA_236,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DEA_486,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp05;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_05,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp61;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_61,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp73;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_73,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp85;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_85,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp94;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_94,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.DspLicenseAgreement;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.DSP_LICENSE_AGREEMENT,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);

            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.FromForeignTradeZone;
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal(InternationalControlledExportType.FROM_FOREIGN_TRADE_ZONE,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);
        }

        [Fact]
        public void Manipulate_ForeignTradeZoneCodeMatches_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;
            fedExEntity.IntlExportDetailForeignTradeZoneCode = "ftzc";
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal("ftzc",
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.ForeignTradeZoneCode);
        }

        [Fact]
        public void Manipulate_EntryNumberMatches_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;
            fedExEntity.IntlExportDetailEntryNumber = "123456";
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal("123456",
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.EntryNumber);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitNumberMatches_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;
            fedExEntity.IntlExportDetailLicenseOrPermitNumber = "abcde123456";
            testObject.Manipulate(shipRequest);
            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;
            Assert.Equal("abcde123456",
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitNumber);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateMatches_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(licenseOrPermitExpirationDate,
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDate);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateSpecifiedIsFalse_WhenNoExpirationDateProvided_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;

            testObject.Manipulate(shipRequest);

            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(false,
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateSpecifiedIsTrue_WhenNoExpirationDateProvided_Test()
        {
            ProcessShipmentRequest processShipmentRequest;
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(true,
                            processShipmentRequest.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDEA236_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea236;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDEA036_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea036;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDEA486_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea486;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSP05_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp05;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSP61_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp61;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSP73_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp73;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSP85_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp85;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSP94_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dsp94;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsDSPLicenseAgreement_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.DspLicenseAgreement;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsForeignTradeZone_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.FromForeignTradeZone;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsControlledExportOption_WhenExportTypeIsWarehouseWithdrawal_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.WarehouseWithdrawal;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Equal(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_DoesNotAddControlledExportOption_WhenExportTypeIsNone_Test()
        {
            FedExShipmentEntity fedExEntity = shipRequest.ShipmentEntity.FedEx;
            fedExEntity.IntlExportDetailType = (int)FedExInternationalControlledExportType.None;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            fedExEntity.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            testObject.Manipulate(shipRequest);

            ProcessShipmentRequest processShipmentRequest = (ProcessShipmentRequest)shipRequest.NativeRequest;

            Assert.Null(processShipmentRequest.RequestedShipment);
        }
    }
}
