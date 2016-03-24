using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.PlatforInterfaces;
using System;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Generates a string to identify the spark pay store
    /// </summary>
    public class SparkPayInternalLicenseIdentifierFactory : IInternalLicenseIdentifierFactory
    {
        public string Create(StoreEntity store)
        {
            SparkPayStoreEntity sparkPayStore = (SparkPayStoreEntity)store;

            if (sparkPayStore == null)
            {
                throw new NullReferenceException("Non SparkPay store passed to SparkPay license identifier");
            }

            return sparkPayStore.StoreUrl;
        }
    }
}
