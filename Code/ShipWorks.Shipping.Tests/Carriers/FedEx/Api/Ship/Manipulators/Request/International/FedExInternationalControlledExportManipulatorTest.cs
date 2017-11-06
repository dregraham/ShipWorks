using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExInternationalControlledExportManipulatorTest
    {
        private FedExInternationalControlledExportManipulator testObject;
        private ShipmentEntity shipment;

        public FedExInternationalControlledExportManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            testObject = new FedExInternationalControlledExportManipulator();
        }

        [Theory]
        [InlineData(FedExInternationalControlledExportType.None, false)]
        [InlineData(FedExInternationalControlledExportType.Dea036, true)]
        [InlineData(FedExInternationalControlledExportType.Dea236, true)]
        [InlineData(FedExInternationalControlledExportType.Dea486, true)]
        [InlineData(FedExInternationalControlledExportType.Dsp05, true)]
        [InlineData(FedExInternationalControlledExportType.Dsp61, true)]
        [InlineData(FedExInternationalControlledExportType.Dsp73, true)]
        [InlineData(FedExInternationalControlledExportType.Dsp85, true)]
        [InlineData(FedExInternationalControlledExportType.Dsp94, true)]
        [InlineData(FedExInternationalControlledExportType.DspLicenseAgreement, true)]
        [InlineData(FedExInternationalControlledExportType.FromForeignTradeZone, true)]
        [InlineData(FedExInternationalControlledExportType.WarehouseWithdrawal, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(FedExInternationalControlledExportType type, bool expected)
        {
            shipment.FedEx.IntlExportDetailType = (int) type;

            var result = testObject.ShouldApply(shipment, 0);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(FedExInternationalControlledExportType.Dea036, InternationalControlledExportType.DEA_036)]
        [InlineData(FedExInternationalControlledExportType.Dea236, InternationalControlledExportType.DEA_236)]
        [InlineData(FedExInternationalControlledExportType.Dea486, InternationalControlledExportType.DEA_486)]
        [InlineData(FedExInternationalControlledExportType.Dsp05, InternationalControlledExportType.DSP_05)]
        [InlineData(FedExInternationalControlledExportType.Dsp61, InternationalControlledExportType.DSP_61)]
        [InlineData(FedExInternationalControlledExportType.Dsp73, InternationalControlledExportType.DSP_73)]
        [InlineData(FedExInternationalControlledExportType.Dsp85, InternationalControlledExportType.DSP_85)]
        [InlineData(FedExInternationalControlledExportType.Dsp94, InternationalControlledExportType.DSP_94)]
        [InlineData(FedExInternationalControlledExportType.DspLicenseAgreement, InternationalControlledExportType.DSP_LICENSE_AGREEMENT)]
        [InlineData(FedExInternationalControlledExportType.FromForeignTradeZone, InternationalControlledExportType.FROM_FOREIGN_TRADE_ZONE)]
        [InlineData(FedExInternationalControlledExportType.WarehouseWithdrawal, InternationalControlledExportType.WAREHOUSE_WITHDRAWAL)]
        public void Manipulate_ICEDTypeIsCorrect_WhenConverted(FedExInternationalControlledExportType type, InternationalControlledExportType expected)
        {
            shipment.FedEx.IntlExportDetailType = (int) type;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.Equal(expected,
                result.Value.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail.Type);
        }

        [Fact]
        public void Manipulate_ReturnsFailure_WhenTypeIsUnknown()
        {
            shipment.FedEx.IntlExportDetailType = int.MaxValue;
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_ForeignTradeZoneCodeMatches()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;
            shipment.FedEx.IntlExportDetailForeignTradeZoneCode = "ftzc";
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.Equal("ftzc",
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.ForeignTradeZoneCode);
        }

        [Fact]
        public void Manipulate_EntryNumberMatches()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;
            shipment.FedEx.IntlExportDetailEntryNumber = "123456";
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.Equal("123456",
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.EntryNumber);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitNumberMatches()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;
            shipment.FedEx.IntlExportDetailLicenseOrPermitNumber = "abcde123456";
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);
            Assert.Equal("abcde123456",
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitNumber);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateMatches()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            shipment.FedEx.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(licenseOrPermitExpirationDate,
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDate);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateSpecifiedIsFalse_WhenNoExpirationDateProvided()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);


            Assert.Equal(false,
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified);
        }

        [Fact]
        public void Manipulate_LicenseOrPermitExpirationDateSpecifiedIsTrue_WhenNoExpirationDateProvided()
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea486;

            DateTime licenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");
            shipment.FedEx.IntlExportDetailLicenseOrPermitExpirationDate = licenseOrPermitExpirationDate;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(true,
                            result.Value.RequestedShipment.SpecialServicesRequested
                                                  .InternationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified);
        }

        [Theory]
        [InlineData(FedExInternationalControlledExportType.Dea036, InternationalControlledExportType.DEA_036)]
        [InlineData(FedExInternationalControlledExportType.Dea236, InternationalControlledExportType.DEA_236)]
        [InlineData(FedExInternationalControlledExportType.Dea486, InternationalControlledExportType.DEA_486)]
        [InlineData(FedExInternationalControlledExportType.Dsp05, InternationalControlledExportType.DSP_05)]
        [InlineData(FedExInternationalControlledExportType.Dsp61, InternationalControlledExportType.DSP_61)]
        [InlineData(FedExInternationalControlledExportType.Dsp73, InternationalControlledExportType.DSP_73)]
        [InlineData(FedExInternationalControlledExportType.Dsp85, InternationalControlledExportType.DSP_85)]
        [InlineData(FedExInternationalControlledExportType.Dsp94, InternationalControlledExportType.DSP_94)]
        [InlineData(FedExInternationalControlledExportType.DspLicenseAgreement, InternationalControlledExportType.DSP_LICENSE_AGREEMENT)]
        [InlineData(FedExInternationalControlledExportType.FromForeignTradeZone, InternationalControlledExportType.FROM_FOREIGN_TRADE_ZONE)]
        [InlineData(FedExInternationalControlledExportType.WarehouseWithdrawal, InternationalControlledExportType.WAREHOUSE_WITHDRAWAL)]
        public void Manipulate_AddsControlledExportOption_WhenExportIsValidType(FedExInternationalControlledExportType type, InternationalControlledExportType expected)
        {
            shipment.FedEx.IntlExportDetailType = (int) FedExInternationalControlledExportType.Dea236;
            shipment.FedEx.IntlExportDetailLicenseOrPermitExpirationDate = DateTime.Parse("3/15/2015");

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.Contains(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE,
                result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }
    }
}
