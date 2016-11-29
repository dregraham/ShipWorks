using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IExtensionAttributes
    {
        IList<IShippingAssignment> ShippingAssignments { get; set; }
    }
}