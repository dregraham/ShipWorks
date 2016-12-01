using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IExtensionAttributes
    {
        IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }

        IEnumerable<ICustomOption> CustomOptions { get; set; }

        IEnumerable<IBundleOption> BundleOptions { get; set; }

        IDownloadableOption DownloadableOption { get; set; }

        IEnumerable<IConfigurableItemOption> ConfigurableItemOptions { get; set; }
    }
}