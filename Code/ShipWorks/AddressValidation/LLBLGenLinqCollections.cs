using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Provides concrete implementations of the linq collections
    /// </summary>
    public class LLBLGenLinqCollections : ILinqCollections
    {
        private readonly LinqMetaData metaData;

        /// <summary>
        /// Instantiates the object
        /// </summary>
        public LLBLGenLinqCollections(LinqMetaData metaData)
        {
            this.metaData = metaData;
        }

        /// <summary>
        /// Allow shipments to be queried
        /// </summary>
        public IQueryable<ShipmentEntity> Shipment
        {
            get
            {
                return metaData.Shipment;
            }
        }

        /// <summary>
        /// Allow validated addresses to be queried
        /// </summary>
        public IQueryable<ValidatedAddressEntity> ValidatedAddress
        {
            get
            {
                return metaData.ValidatedAddress;
            }
        }
    }
}
