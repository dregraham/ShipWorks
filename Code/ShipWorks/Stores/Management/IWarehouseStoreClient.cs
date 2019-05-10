using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    public interface IWarehouseStoreClient
    {
        Task<Result> UploadStoreToWarehouse(StoreEntity store);
    }
}