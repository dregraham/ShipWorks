using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Inteface for ShipmentLoaderService
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IShipmentLoaderService<TResult>
    {
        /// <summary>
        /// Loads the order and sends a message that it has been loaded.
        /// </summary>
        void LoadAndNotify(IEnumerable<long>entityIDs);
    }
}
