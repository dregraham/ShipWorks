using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExInternationalControlledExportManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExInternationalControlledExportManipulator" /> class.
        /// </summary>
        public FedExInternationalControlledExportManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExInternationalControlledExportManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExInternationalControlledExportManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        public override void Manipulate(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            // If the type is none, just return.
            if ((FedExInternationalControlledExportType) fedExShipment.IntlExportDetailType ==
                FedExInternationalControlledExportType.None)
            {
                return;
            }

            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            AddControlledExportOption(nativeRequest);
            
            // Get the current InternationalControlledExportDetail
            InternationalControlledExportDetail internationalControlledExportDetail =
                nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail;

            // Get the InternationalControlledExportDetail Type based on the FedEx shipment property
            internationalControlledExportDetail.Type =
                GetApiInternationalControlledExportType((FedExInternationalControlledExportType)fedExShipment.IntlExportDetailType);

            // Set the InternationalControlledExportDetail ForeignTradeZoneCode
            internationalControlledExportDetail.ForeignTradeZoneCode =
                fedExShipment.IntlExportDetailForeignTradeZoneCode;

            // Set the InternationalControlledExportDetail EntryNumber and LicenseOrPermitNumber
            internationalControlledExportDetail.EntryNumber = fedExShipment.IntlExportDetailEntryNumber;
            internationalControlledExportDetail.LicenseOrPermitNumber = fedExShipment.IntlExportDetailLicenseOrPermitNumber;

            // Set the InternationalControlledExportDetail LicenseOrPermitNumber expiration date
            if (fedExShipment.IntlExportDetailLicenseOrPermitExpirationDate.HasValue)
            {
                internationalControlledExportDetail.LicenseOrPermitExpirationDate =
                    fedExShipment.IntlExportDetailLicenseOrPermitExpirationDate.Value;
                internationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified = true;
            }
            else
            {
                internationalControlledExportDetail.LicenseOrPermitExpirationDateSpecified = false; 
            }
        }

        /// <summary>
        /// Adds the controlled export option to the request.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        private static void AddControlledExportOption(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Resize the special service type array so we can add the controlled export service type
            ShipmentSpecialServiceType[] services = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Array.Resize(ref services, services.Length + 1);

            // Add the controlled export option and update the native request
            services[services.Length - 1] = ShipmentSpecialServiceType.INTERNATIONAL_CONTROLLED_EXPORT_SERVICE;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = services;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void InitializeRequest(CarrierRequest request)
        {
            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
                
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // We'll be manipulating properties of the SpecialServicesRequested, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail == null)
            {
                // We'll be manipulating properties of the InternationalControlledExportDetail, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalControlledExportDetail = new InternationalControlledExportDetail();
            }
        }

        /// <summary>
        /// Get the API InternationalControlledExportType based on our internal value
        /// </summary>
        public static InternationalControlledExportType GetApiInternationalControlledExportType(FedExInternationalControlledExportType internationalControlledExportType)
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

            throw new InvalidOperationException("Invalid FedEx International Controlled Export Type " + internationalControlledExportType);
        }
    }
}
