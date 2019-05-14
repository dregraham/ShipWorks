using System.Threading.Tasks;

namespace ShipWorks.Warehouse
{
    public interface IWarehouseDownloader
    {
        Task Download(string warehouseID);
    }
}