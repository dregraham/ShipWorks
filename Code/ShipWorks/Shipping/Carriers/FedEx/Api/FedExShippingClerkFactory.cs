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
        /// <param name="settingsRepository">The ICarrierSettingsRepository.</param>
        public static IFedExShippingClerk CreateShippingClerk(ShipmentEntity shipment, ICarrierSettingsRepository settingsRepository)
        {
            IFedExShippingClerk fedExShippingClerk = null;

            if (!IsFimsShipment(shipment))
            {
                FedExShippingClerkParameters parameters = new FedExShippingClerkParameters()
                {
                    Inspector = new FedExShipmentType().CertificateInspector,
                    ForceVersionCapture = false,
                    LabelRepository = new FedExLabelRepository(),
                    RequestFactory = new FedExRequestFactory(new FedExSettingsRepository()),
                    SettingsRepository = new FedExSettingsRepository(),
                    Log = LogManager.GetLogger(typeof(FedExShippingClerk))
                };

                fedExShippingClerk = new FedExShippingClerk(parameters);
            }
            else
            {
                // TODO: Switch to use FIMS test server check when it's implemented.
                if (settingsRepository.UseTestServer)
                {
                    fedExShippingClerk = new FimsShippingClerk(new FimsFakeWebClient(), new FimsLabelRepository());
                }
                else
                {
                    fedExShippingClerk = new FimsShippingClerk(new FimsWebClient(), new FimsLabelRepository());
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
