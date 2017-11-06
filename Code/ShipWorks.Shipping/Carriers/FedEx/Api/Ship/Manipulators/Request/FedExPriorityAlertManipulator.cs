using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that will manipulate the Priority Alert settings
    /// of a FedEx IFedExNativeShipmentRequest.
    /// </summary>
    public class FedExPriorityAlertManipulator : FedExShippingRequestManipulatorBase
    {
        private int currentPackageIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPriorityAlertManipulator" /> class.
        /// </summary>
        public FedExPriorityAlertManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPriorityAlertManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExPriorityAlertManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // Pull out the sequence number so we know which package in the shipment to use
            this.currentPackageIndex = request.SequenceNumber;

            if (currentPackageIndex < request.ShipmentEntity.FedEx.Packages.Count)
            {
                // We can safely cast this since we've passed initialization
                IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

                // Extract any API enhancement type to a FedEx enhancement type array for the current package
                FedExPackageEntity package = request.ShipmentEntity.FedEx.Packages[currentPackageIndex];

                if (package.PriorityAlert)
                {
                    List<PackageSpecialServiceType> specialServiceTypes =
                        nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.ToList();

                    specialServiceTypes.Add(PackageSpecialServiceType.PRIORITY_ALERT);
                    nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
                }

                if (!string.IsNullOrWhiteSpace(package.PriorityAlertDetailContent))
                {
                    // Each package should be in it's own request, so we always use the first item in the line item aray
                    nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail = new PriorityAlertDetail();

                    // Update the package line item in the native request with the API enhancement type array and the content
                    nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.Content = new string[] { package.PriorityAlertDetailContent };

                    PriorityAlertEnhancementType[] apiEnhancementTypes = GetApiEnhancementType(package);

                    if (apiEnhancementTypes.Length > 0)
                    {

                        nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.PriorityAlertDetail.EnhancementTypes = apiEnhancementTypes;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

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

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null || nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                // We'll be inspecting/manipulating properties of the package line items, so make sure it's been created
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            PackageSpecialServiceType[] specialServiceTypes = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes;
            if (specialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[0];
            }
        }

        /// <summary>
        /// Translates the ShipWorks specific value to the native API enhancement type value.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>An array of PriorityAlertEnhancementType values.</returns>
        private PriorityAlertEnhancementType[] GetApiEnhancementType(FedExPackageEntity package)
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
