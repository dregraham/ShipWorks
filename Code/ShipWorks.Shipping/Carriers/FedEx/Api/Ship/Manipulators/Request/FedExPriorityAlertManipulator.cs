using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that will manipulate the Priority Alert settings
    /// of a FedEx IFedExNativeShipmentRequest.
    /// </summary>
    public class FedExPriorityAlertManipulator : IFedExShipRequestManipulator
    {
        private int currentPackageIndex;

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return sequenceNumber < shipment.FedEx.Packages.Count();
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            // Pull out the sequence number so we know which package in the shipment to use
            currentPackageIndex = sequenceNumber;

            IFedExPackageEntity[] packages = shipment.FedEx.Packages.ToArray();
            if (currentPackageIndex < packages.Length)
            {
                // Extract any API enhancement type to a FedEx enhancement type array for the current package
                IFedExPackageEntity package = packages[currentPackageIndex];

                if (package.PriorityAlert)
                {
                    List<PackageSpecialServiceType> specialServiceTypes =
                        request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.ToList();

                    specialServiceTypes.Add(PackageSpecialServiceType.PRIORITY_ALERT);
                    request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
                }

                if (!string.IsNullOrWhiteSpace(package.PriorityAlertDetailContent))
                {
                    // Each package should be in it's own request, so we always use the first item in the line item aray
                    request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail = new PriorityAlertDetail();

                    // Update the package line item in the native request with the API enhancement type array and the content
                    request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content = new string[] { package.PriorityAlertDetailContent };

                    PriorityAlertEnhancementType[] apiEnhancementTypes = GetApiEnhancementType(package);

                    if (apiEnhancementTypes.Length > 0)
                    {

                        request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes = apiEnhancementTypes;
                    }
                }
            }

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            request.Ensure(r => r.RequestedShipment)
                .EnsureAtLeastOne(rs => rs.RequestedPackageLineItems)
                .Ensure(rp => rp.SpecialServicesRequested)
                .Ensure(s => s.SpecialServiceTypes);

            PackageSpecialServiceType[] specialServiceTypes = request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes;
            if (specialServiceTypes == null)
            {
                request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[0];
            }
        }

        /// <summary>
        /// Translates the ShipWorks specific value to the native API enhancement type value.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>An array of PriorityAlertEnhancementType values.</returns>
        private PriorityAlertEnhancementType[] GetApiEnhancementType(IFedExPackageEntity package)
        {
            List<PriorityAlertEnhancementType> apiEnhancementTypes = new List<PriorityAlertEnhancementType>();

            if (package.PriorityAlertEnhancementType == (int) FedExPriorityAlertEnhancementType.PriorityAlertPlus)
            {
                apiEnhancementTypes.Add(PriorityAlertEnhancementType.PRIORITY_ALERT_PLUS);
            }

            return apiEnhancementTypes.ToArray();
        }
    }
}
