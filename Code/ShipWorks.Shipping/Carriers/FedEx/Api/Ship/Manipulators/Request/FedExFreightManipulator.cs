using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExFreightManipulator : IFedExShipRequestManipulator
    {
        readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExFreightManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) =>
            FedExUtility.IsFreightExpressService((FedExServiceType) shipment.FedEx.Service);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            Initialize(request);
            CreateFedExExpressFreightDetailManipulations(request.RequestedShipment, shipment.FedEx);

            return request;
        }

        /// <summary>
        /// Add options for express freight
        /// </summary>
        private void CreateFedExExpressFreightDetailManipulations(RequestedShipment requestedShipment, IFedExShipmentEntity fedex)
        {
            ExpressFreightDetail expressFreightDetail = requestedShipment.ExpressFreightDetail;
            expressFreightDetail.BookingConfirmationNumber = fedex.FreightBookingNumber;

            if (fedex.FreightLoadAndCount > 0)
            {
                expressFreightDetail.ShippersLoadAndCount = fedex.FreightLoadAndCount.ToString();
            }

            // For certification, so far, everything is true.  
            if (settingsRepository.IsInterapptiveUser)
            {
                expressFreightDetail.PackingListEnclosed = true;
                expressFreightDetail.PackingListEnclosedSpecified = true;
            }

            AddSpecialServiceTypes(requestedShipment.SpecialServicesRequested, fedex);
            AddShippingDocumentTypes(requestedShipment.ShippingDocumentSpecification);
        }

        /// <summary>
        /// Add special service types
        /// </summary>
        private static void AddSpecialServiceTypes(ShipmentSpecialServicesRequested servicesRequested, IFedExShipmentEntity fedex)
        {
            // So far, this needs to stay US...  even if shipping CA => CA
            if (fedex.Shipment.AdjustedShipCountryCode() != "US")
            {
                return;
            }

            var serviceTypes = servicesRequested.SpecialServiceTypes.AsEnumerable();

            if (fedex.FreightInsideDelivery)
            {
                serviceTypes = serviceTypes.Union(new[] { ShipmentSpecialServiceType.INSIDE_DELIVERY });
            }

            if (fedex.FreightInsidePickup)
            {
                serviceTypes = serviceTypes.Union(new[] { ShipmentSpecialServiceType.INSIDE_PICKUP });
            }

            servicesRequested.SpecialServiceTypes = serviceTypes.ToArray();
        }

        /// <summary>
        /// Initialize the request properties needed for freight
        /// </summary>
        private static void Initialize(ProcessShipmentRequest request)
        {
            var requestedShipment = request.Ensure(x => x.RequestedShipment);
            requestedShipment.Ensure(x => x.ExpressFreightDetail);
            requestedShipment.Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
            requestedShipment.Ensure(x => x.ShippingDocumentSpecification)
                .Ensure(x => x.ShippingDocumentTypes);
        }

        /// <summary>
        /// Adds the label shipping document type if it's not already present.
        /// </summary>
        private static void AddShippingDocumentTypes(ShippingDocumentSpecification specification) =>
            specification.ShippingDocumentTypes = specification.ShippingDocumentTypes
                .Union(new[] { RequestedShippingDocumentType.LABEL })
                .ToArray();
    }
}
