﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    public class SkusToUploadDto
    {
        public SkusToUploadDto(IEnumerable<IProductVariantEntity> productVariants, string databaseId)
        {
            this.databaseId = databaseId;
            skus = productVariants.Select(x => new Sku(x.IsActive, x.Aliases)).ToList();
        }

        public string databaseId { get; set; }
        public IEnumerable<Sku> skus { get; set; }
    }

    public class Sku
    {
        public Sku(bool enabled, IEnumerable<IProductVariantAliasEntity> aliases)
        {
            sku = aliases.First(a => a.IsDefault).Sku;
            this.aliases = aliases.Where(a => !a.IsDefault).Select(a => a.Sku);
            this.enabled = enabled;
        }
        public string sku { get; set; }
        public IEnumerable<string> aliases { get; set; }
        public bool enabled { get; set; }
    }
}
