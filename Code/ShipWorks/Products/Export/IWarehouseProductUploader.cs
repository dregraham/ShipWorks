using System.Threading.Tasks;
using Interapptive.Shared.Threading;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Upload products to an associated warehouse
    /// </summary>
    public interface IWarehouseProductUploader
    {
        /// <summary>
        /// Upload changed products to the warehouse
        /// </summary>
        Task Upload(ISingleItemProgressDialog progressItem);
    }
}
