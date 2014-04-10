using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public interface ILinqCollections
    {
        /// <summary>
        /// Allow shipments to be queried
        /// </summary>
        IQueryable<ShipmentEntity> Shipment { get; }

        /// <summary>
        /// Allow validated addresses to be queried
        /// </summary>
        IQueryable<ValidatedAddressEntity> ValidatedAddress { get; }

        /// <summary>
        /// Allow orders to be queried
        /// </summary>
        IQueryable<OrderEntity> Order { get; }
    }
}
