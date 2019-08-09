using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    public class OdbcStore : Store
    {
        public IOdbcFieldMap ImportMap { get; set; }
        public IOdbcFieldMap UploadMap { get; set; }
        public int ImportStrategy { get; set; }
        public int ImportColumnSourceType { get; set; }
        public string ImportColumnSource { get; set; }
        public int ImportOrderItemStrategy { get; set; }
        public int UploadStrategy { get; set; }
        public int UploadColumnSourceType { get; set; }
        public string UploadColumnSource { get; set; }
    }
}
