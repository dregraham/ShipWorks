using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers
{
    /// <summary>
    /// Order DTO
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [Serializable]
    public class MarketplaceAdvisorOrderDto
    {
        /// <summary>
        /// Constructor for serialization
        /// </summary>
        public MarketplaceAdvisorOrderDto()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOrderDto(OrderEntity order)
        {
            MarketplaceAdvisorOrderEntity orderEntity = order as MarketplaceAdvisorOrderEntity;
            if (orderEntity == null)
            {
                throw new MarketplaceAdvisorException("Could not cast order correctly");
            }

            IsManual = orderEntity.IsManual;
            OrderID = orderEntity.OrderID;
            OrderNumber = orderEntity.OrderNumber;
            ParcelID = orderEntity.ParcelID;
        }

        public bool IsManual { get; set; }
        public long OrderID { get; set; }
        public long OrderNumber { get; set; }
        public long ParcelID { get; set; }
    }
}