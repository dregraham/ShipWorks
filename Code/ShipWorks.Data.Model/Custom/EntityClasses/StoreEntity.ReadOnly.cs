using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Stores;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom store data
    /// </summary>
    public partial class ReadOnlyStoreEntity
    {
        /// <summary>
        /// Address as a person adapter
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Strongly typed store type code
        /// </summary>
        public StoreTypeCode StoreTypeCode => (StoreTypeCode) TypeCode;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomStoreData(IStoreEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
