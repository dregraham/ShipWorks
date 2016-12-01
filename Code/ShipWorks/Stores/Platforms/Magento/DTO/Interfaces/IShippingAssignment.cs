using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IShippingAssignment
    {
        IEnumerable<IItem> Items { get; set; }
        IShipping Shipping { get; set; }
    }
}