using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IDownloadableOption
    {
        IEnumerable<int> DownloadableLinks { get; set; }
    }
}