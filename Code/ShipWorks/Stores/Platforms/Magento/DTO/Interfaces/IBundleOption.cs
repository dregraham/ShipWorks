using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IBundleOption
    {
        int OptionId { get; set; }
        int OptionQty { get; set; }
        IEnumerable<int> OptionSelections { get; set; }
        IExtensionAttributes ExtensionAttributes { get; set; }
    }
}