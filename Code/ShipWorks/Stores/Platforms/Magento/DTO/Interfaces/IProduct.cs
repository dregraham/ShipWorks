using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IProduct
    {
        IEnumerable<CustomAttribute> CustomAttributes { get; set; }
        long ID { get; set; }
        IEnumerable<ProductOptionDetail> Options { get; set; }
    }
}