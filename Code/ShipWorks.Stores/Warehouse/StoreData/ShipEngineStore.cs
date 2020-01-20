using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    public class ShipEngineStore : Store
    {
        /// <summary>
        /// The Order Source Id of the ShipEngine Store
        /// </summary>
        public string OrderSourceId { get; set; }

        /// <summary>
        /// The Account Id of the ShipEngine Store
        /// </summary>
        public string AccountId { get; set; }
    }
}
