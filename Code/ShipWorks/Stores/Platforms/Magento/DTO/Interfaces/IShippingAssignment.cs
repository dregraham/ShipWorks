using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IShippingAssignment
    {
        IList<IItem> Items { get; set; }
        IShipping Shipping { get; set; }
    }
}