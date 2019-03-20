using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interapptive.Shared.Business;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Stores;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom store data
    /// </summary>
    public partial interface IStoreEntity
    {
        /// <summary>
        /// Address as a person adapter
        /// </summary>
        PersonAdapter Address { get; }

        /// <summary>
        /// Strongly typed store type code
        /// </summary>
        StoreTypeCode StoreTypeCode { get; }
    }
}
