using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Api
{
    /// <summary>
    /// API Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Api)]
    [Component(RegistrationType.Self)]
    public class ApiStoreType : PlatformStoreType
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
        }

        /// <summary>
        /// Api StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Api;
    }
}
