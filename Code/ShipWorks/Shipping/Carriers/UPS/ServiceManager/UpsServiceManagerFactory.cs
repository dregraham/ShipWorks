using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager.Countries;

namespace ShipWorks.Shipping.Carriers.UPS.ServiceManager
{
    /// <summary>
    /// An implementation of the ICarrierServiceManagerFactory interface that is specific to UPS.
    /// </summary>
    public class UpsServiceManagerFactory : IUpsServiceManagerFactory
    {
        private readonly List<IUpsServiceManager> serviceManagers;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsServiceManagerFactory"/> class.
        /// </summary>
        /// <param name="shipment"></param>
        public UpsServiceManagerFactory(ShipmentEntity shipment)
        {
            serviceManagers = new List<IUpsServiceManager>
            {
                new UpsCanadaServiceManager(),
                new UpsUsServiceManager(shipment),
                new UpsPuertoRicoServiceManager()
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsServiceManagerFactory"/> class. This
        /// version of the constructor is primarily for testing purposes.
        /// </summary>
        /// <param name="serviceManagers">The service managers.</param>
        private UpsServiceManagerFactory(IEnumerable<IUpsServiceManager> serviceManagers)
        {
            this.serviceManagers = new List<IUpsServiceManager>(serviceManagers);
        }

        /// <summary>
        /// Creates the an ICarrierServiceManager appropriate for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An ICarrierServiceManager object.</returns>
        public IUpsServiceManager Create(ShipmentEntity shipment)
        {
            IUpsServiceManager serviceManager = serviceManagers.FirstOrDefault(manager => manager.CountryCode == shipment.AdjustedOriginCountryCode());
            
            if (serviceManager == null)
            {
                // Default, return US.
                return serviceManagers.FirstOrDefault(manager => manager.CountryCode == "US");
            }

            return serviceManager;
        }

        /// <summary>
        /// Create a factory for testing
        /// </summary>
        public static UpsServiceManagerFactory CreateForTesting(IEnumerable<IUpsServiceManager> serviceManagers)
        {
            return new UpsServiceManagerFactory(serviceManagers);
        }
    }
}
