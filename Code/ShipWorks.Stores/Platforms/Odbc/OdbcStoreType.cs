using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// The Odbc Store Type
    /// </summary>
    public class OdbcStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Returns the OdbcStoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Odbc;

        /// <summary>
        /// Creates the license identifier for the OdbcStoreType
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create a downloader for the OdbcStoreType
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an order identifier
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an empty OdbcStoreType
        /// </summary>
        /// <returns></returns>
        public override StoreEntity CreateStoreInstance()
        {
            throw new NotImplementedException();
        }
    }
}
