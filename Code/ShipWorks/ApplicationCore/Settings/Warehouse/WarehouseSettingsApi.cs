using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// Api for managing interaction with the warehouse for settings
    /// </summary>
    [Component]
    public class WarehouseSettingsApi : IWarehouseSettingsApi
    {
        private readonly IWarehouseList warehouseListRequest;
        private readonly IWarehouseAssociation warehouseAssociation;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsApi(IWarehouseList warehouseListRequest, IWarehouseAssociation warehouseAssociation)
        {
            this.warehouseAssociation = warehouseAssociation;
            this.warehouseListRequest = warehouseListRequest;
        }

        /// <summary>
        /// Ge all warehouses
        /// </summary>
        public Task<GenericResult<WarehouseListDto>> GetAllWarehouses() => warehouseListRequest.GetList();

        /// <summary>
        /// Associate the warehouse with this instance of ShipWorks
        /// </summary>
        public Task<Result> Associate(string id) => warehouseAssociation.Associate(id);
    }
}
