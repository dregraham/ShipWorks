using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IExtensionAttributes
    {
        IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }
    }
}