using System.Reflection;
using Interapptive.Shared.IO.Zip;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class OdbcStore : Store
    {
        [JsonIgnore]
        public string ImportMap
        {
            get => GZipUtility.Decompress(CompressedImportMap);
            set => CompressedImportMap = GZipUtility.Compress(value);
        }

        [JsonProperty("importMap")]
        public string CompressedImportMap { get; set; } = string.Empty;

        [JsonIgnore]
        public string UploadMap
        {
            get => GZipUtility.Decompress(CompressedUploadMap);
            set => CompressedUploadMap = GZipUtility.Compress(value);
        }

        [JsonProperty("uploadMap")]
        public string CompressedUploadMap { get; set; } = string.Empty;

        public int ImportStrategy { get; set; }
        public int ImportColumnSourceType { get; set; }
        public string ImportColumnSource { get; set; }
        public int ImportOrderItemStrategy { get; set; }
        public int UploadStrategy { get; set; }
        public int UploadColumnSourceType { get; set; }
        public string UploadColumnSource { get; set; }
    }
}
