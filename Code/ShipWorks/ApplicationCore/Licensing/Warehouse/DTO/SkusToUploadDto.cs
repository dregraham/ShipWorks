using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// Represents the data for a single upload request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class SkusToUploadDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SkusToUploadDto(IEnumerable<IProductVariantEntity> productVariants, string databaseId)
        {
            this.DatabaseId = databaseId;
            SKUs = productVariants.Select(x => new SkuEntry(x.IsActive, x.Aliases)).ToList();
        }

        /// <summary>
        /// Id of the ShipWorks database
        /// </summary>
        [JsonProperty("databaseId")]
        public string DatabaseId { get; set; }

        /// <summary>
        /// Collection of SKUs to upload
        /// </summary>
        [JsonProperty("skus")]
        public IEnumerable<SkuEntry> SKUs { get; set; }
    }

    /// <summary>
    /// Individual SKU entry to upload
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class SkuEntry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SkuEntry(bool enabled, IEnumerable<IProductVariantAliasEntity> aliases)
        {
            Sku = aliases.First(a => a.IsDefault).Sku;
            this.Aliases = aliases.Where(a => !a.IsDefault).Select(a => a.Sku);
            this.Enabled = enabled;
        }

        /// <summary>
        /// Primary SKU
        /// </summary>
        [JsonProperty("sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Alias SKUs for the product
        /// </summary>
        [JsonProperty("aliases")]
        public IEnumerable<string> Aliases { get; set; }

        /// <summary>
        /// Is the product enabled
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
