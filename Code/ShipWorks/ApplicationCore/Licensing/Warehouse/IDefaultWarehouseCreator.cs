using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Creates a default warehouse
    /// </summary>
    public interface IDefaultWarehouseCreator
    {
        /// <summary>
        /// Creates a default warehouse if needed
        /// </summary>
        Task<Result> Create(IStoreEntity store);

        /// <summary>
        /// Returns true if no linked warehouse and one needs to be created
        /// </summary>
        Task<GenericResult<bool>> NeedsDefaultWarehouse();
    }
}
