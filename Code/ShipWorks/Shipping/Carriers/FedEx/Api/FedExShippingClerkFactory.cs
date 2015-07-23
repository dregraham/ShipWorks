using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory responsible for creating the correct IFedExShippingClerk for a FedEx shipment.
    /// </summary>
    public static class FedExShippingClerkFactory
    {
        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="certificateInspector">The ICertificateInspector</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICertificateInspector certificateInspector) 
        {
            return CreateShippingClerk(shipment, new FedExSettingsRepository(), certificateInspector);
        }

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        /// <param name="certificateInspector">The ICertificateInspector</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
        {
            return CreateShippingClerk(shipment, settingsRepository, certificateInspector, new FedExRequestFactory(settingsRepository), LogManager.GetLogger(typeof(FedExShippingClerk)), false, new FedExLabelRepository());
        }

        /// <summary>
        /// Creates an IFedExShippingClerk with the specified shipment and ICertificateInspector.
        /// </summary>
        /// <param name="shipment">The shipment.  If this clerk should not need a shipment, like when doing a Close, just pass null.</param>
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        /// <param name="certificateInspector">The ICertificateInspector</param>
        /// <param name="requestFactory">The IFedExRequestFactory.  </param>
        /// <param name="log">The ILog.</param>
        /// <param name="forceVersionCapture">if set to <c>true</c> [force version capture] to occur rather than only performing the version capture once.</param>
        /// <param name="labelRepository">The ILabelRepository.</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector, IFedExRequestFactory requestFactory, ILog log, bool forceVersionCapture, ILabelRepository labelRepository)
        {
            IFedExShippingClerk fedExShippingClerk = null;

            if (!IsFimsShipment(shipment))
            {
                fedExShippingClerk = new FedExShippingClerk(settingsRepository, certificateInspector, requestFactory, log, forceVersionCapture, labelRepository);
            }
            else
            {
                // TODO: Switch to use FIMS test server check when it's implemented.
                if (settingsRepository.UseTestServer)
                {
                    fedExShippingClerk = new FimsShippingClerk(new FimsFakeWebClient(), new FimsLabelRepository());
                }
            }

            return fedExShippingClerk;
        }

        /// <summary>
        /// Determines if the shipment is a FIMS shipment.
        /// </summary>
        private static bool IsFimsShipment(ShipmentEntity shipment)
        {
            return shipment != null &&
                   (shipment.FedEx.Service == (int) FedExServiceType.FedExFimsUnder4Lbs ||
                    shipment.FedEx.Service == (int) FedExServiceType.FedExFims4LbsAndOver);
        }
    }
}
