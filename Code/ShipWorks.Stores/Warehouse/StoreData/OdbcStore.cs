using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class OdbcStore : Store
    {
        public string ImportMap { get; set; }
        public string UploadMap { get; set; }
        public int ImportStrategy { get; set; }
        public int ImportColumnSourceType { get; set; }
        public string ImportColumnSource { get; set; }
        public int ImportOrderItemStrategy { get; set; }
        public int UploadStrategy { get; set; }
        public int UploadColumnSourceType { get; set; }
        public string UploadColumnSource { get; set; }
    }
}
