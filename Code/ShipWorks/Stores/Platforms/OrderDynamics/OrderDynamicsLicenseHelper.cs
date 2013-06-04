using System;
using ShipWorks.Stores.Platforms.OrderDynamics.WebServices;
using System.Net;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.OrderDynamics
{
    /// <summary>
    /// Valides a License ID with OrderDyanamics
    /// </summary>
    public static class OrderDynamicsLicenseHelper
    {
        /// <summary>
        /// Determines if the license provided is a valid OrderDynamics license identifier.
        /// </summary>
        public static bool IsValidLicense(string license)
        {
            Guid licenseGuid;
            try
            {
                licenseGuid = new Guid(license);
            }
            catch (FormatException)
            {
                throw new FormatException("Invalid License ID format.");
            }

            try
            {
                // per OrderDynamics, if we can get a shipment queue for the store, the Guid is good.
                ShipmentQueueService service = new ShipmentQueueService();
                service.GetShipmentQueue(licenseGuid);

                return true;
            }
            catch (Exception ex)
            {
                // if it blows up, it's an invalid store guid
                if (WebHelper.IsWebException(ex))
                {
                    return false;
                }

                throw;
            }
        }
    }
}
