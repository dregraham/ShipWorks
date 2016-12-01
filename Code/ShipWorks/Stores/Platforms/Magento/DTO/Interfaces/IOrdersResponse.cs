using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IOrdersResponse
    {
        IEnumerable<IOrder> Orders { get; set; }
        int TotalCount { get; set; }
    }
}