using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Api
{
    /// <summary>
    /// Volusion integration type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Api)]
    [Component(RegistrationType.Self)]
    public class ApiStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStoreType(StoreEntity store)
            : base(store)
        {
            if (store != null && !(store is PlatformStoreEntity))
            {
                throw new ArgumentException("ApiStoretype - StoreEntity is not instance of PlatformStoreEntity.");
            }
        }

        /// <summary>
        /// Api StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Api;

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                PlatformStoreEntity store = (PlatformStoreEntity) Store;
                return store.OrderSourceID;
            }
        }

        /// <summary>
        /// Creates the OrderIdentifier for locating Api orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            PlatformStoreEntity store = new PlatformStoreEntity();

            InitializeStoreDefaults(store);

            store.OrderSourceID = "";

            return store;
        }
    }
}
