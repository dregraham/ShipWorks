using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// Add international controlled export information
    /// </summary>
    public class FedExInternationalControlledExportManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.FedEx.IntlExportDetailType != (int) FedExInternationalControlledExportType.None;

        /// <summary>
        /// Manipulate the request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeRequest(request);
            var fedExShipment = shipment.FedEx;

            AddControlledExportOption(request);

            return GetApiInternationalControlledExportType((FedExInternationalControlledExportType) fedExShipment.IntlExportDetailType)
                .Map(x => BuildExportDetail(request, fedExShipment, x));
        }

        /// <summary>
        /// Build export detail
        /// </summary>
        private ProcessShipmentRequest BuildExportDetail(
            ProcessShipmentRequest request,
            IFedExShipmentEntity fedExShipment,
            InternationalControlledExportType exportType)
        {
            var detail = request.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail;

            detail.Type = exportType;
            detail.ForeignTradeZoneCode = fedExShipment.IntlExportDetailForeignTradeZoneCode;
            detail.EntryNumber = fedExShipment.IntlExportDetailEntryNumber;
            detail.LicenseOrPermitNumber = fedExShipment.IntlExportDetailLicenseOrPermitNumber;

            // Set the InternationalControlledExportDetail LicenseOrPermitNumber expiration date
            if (fedExShipment.IntlExportDetailLicenseOrPermitExpirationDate.HasValue)
            {
                detail.LicenseOrPermitExpirationDate =
                    fedExShipment.IntlExportDetailLicenseOrPermitExpirationDate.Value;
                detail.LicenseOrPermitExpirationDateSpecified = true;
            }
            else
            {
                detail.LicenseOrPermitExpirationDateSpecified = false;
            }

            return request;
        }

        /// <summary>
        /// Adds the controlled export option to the request.
        /// </summary>
        /// <param name="request">The native request.</param>
        private static void AddControlledExportOption(IFedExNativeShipmentRequest request)
        {
            var specialServices = request.RequestedShipment.SpecialServicesRequested;
            specialServices.SpecialServiceTypes = specialServices.SpecialServiceTypes
                .Append(ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE)
                .ToArray();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private static void InitializeRequest(ProcessShipmentRequest request)
        {
            var services = request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);

            services.Ensure(x => x.SpecialServiceTypes);
            services.Ensure(x => x.InternationalControlledExportDetail);
        }

        /// <summary>
        /// Get the API InternationalControlledExportType based on our internal value
        /// </summary>
        public static GenericResult<InternationalControlledExportType> GetApiInternationalControlledExportType(FedExInternationalControlledExportType internationalControlledExportType)
        {
            switch (internationalControlledExportType)
            {
                case FedExInternationalControlledExportType.Dea036: return InternationalControlledExportType.DEA_036;
                case FedExInternationalControlledExportType.Dea236: return InternationalControlledExportType.DEA_236;
                case FedExInternationalControlledExportType.Dea486: return InternationalControlledExportType.DEA_486;
                case FedExInternationalControlledExportType.Dsp05: return InternationalControlledExportType.DSP_05;
                case FedExInternationalControlledExportType.Dsp61: return InternationalControlledExportType.DSP_61;
                case FedExInternationalControlledExportType.Dsp73: return InternationalControlledExportType.DSP_73;
                case FedExInternationalControlledExportType.Dsp85: return InternationalControlledExportType.DSP_85;
                case FedExInternationalControlledExportType.Dsp94: return InternationalControlledExportType.DSP_94;
                case FedExInternationalControlledExportType.DspLicenseAgreement: return InternationalControlledExportType.DSP_LICENSE_AGREEMENT;
                case FedExInternationalControlledExportType.FromForeignTradeZone: return InternationalControlledExportType.FROM_FOREIGN_TRADE_ZONE;
                case FedExInternationalControlledExportType.WarehouseWithdrawal: return InternationalControlledExportType.WAREHOUSE_WITHDRAWAL;
            }

            return new InvalidOperationException("Invalid FedEx International Controlled Export Type " + internationalControlledExportType);
        }
    }
}
