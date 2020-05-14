using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Data returned from the upload product endpoint
    /// </summary>
    [Obfuscation]
    public class UploadResponseData
    {
        /// <summary>
        /// Results of the import
        /// </summary>
        public IEnumerable<UploadResponseDataResult> Results { get; set; }
    }
}
