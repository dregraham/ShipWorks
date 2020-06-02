using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    public interface IDefaultWarehouseCreator
    {
        Task<Result> CreateIfNeeded(IStoreEntity store);
    }
}
