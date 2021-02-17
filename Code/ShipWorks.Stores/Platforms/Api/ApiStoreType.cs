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
    public class ApiStoreType : StoreType
    {
        /// <summary>
        /// Api StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Api;

        /// <summary>
        /// Gets the license identifier for this store
        /// </summary>
        protected override string InternalLicenseIdentifier => throw new NotImplementedException();

        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            throw new NotImplementedException();
        }

        public override StoreEntity CreateStoreInstance()
        {
            throw new NotImplementedException();
        }
    }
}
