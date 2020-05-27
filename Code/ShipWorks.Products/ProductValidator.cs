using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Validator for Products
    /// </summary>
    [Component]
    public class ProductValidator : IProductValidator
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IMessageHelper messageHelper;

        public ProductValidator(ISqlAdapterFactory sqlAdapterFactory, IMessageHelper messageHelper)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Checks if product is valid
        /// </summary>
        public async Task<Result> Validate(ProductVariantEntity productVariant, IProductCatalog productCatalog)
        {
            if (productVariant.Aliases.Any(a => string.IsNullOrWhiteSpace(a.Sku)))
            {
                string message = $"The following field is required: {Environment.NewLine}SKU";
                return Result.FromError(message);
            }

            if (productVariant.Product.IsBundle)
            {
                // A Bundle can't have siblings
                IEnumerable<IProductVariantEntity> siblingVariants;
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    siblingVariants = await productCatalog.FetchSiblingVariants(productVariant, sqlAdapter);
                }

                if (siblingVariants.Any())
                {
                    return Result.FromError("A product with variants cannot be turned into a bundle");
                }

                // A Bundle can't be in another bundle
                int inHowManyBundles = productVariant.IncludedInBundles.Count();
                if (inHowManyBundles > 0)
                {
                    string plural = inHowManyBundles > 1 ? "s" : "";
                    string question = $"A bundle cannot contain another bundle.\r\n\r{productVariant.DefaultSku ?? "This Product"} is already a part of {inHowManyBundles} existing bundle{plural}.\r\n\r\nDo you want to remove {productVariant.DefaultSku ?? "this product"} from the existing bundle{plural}? ";

                    DialogResult answer = messageHelper.ShowQuestion(question);
                    if (answer != DialogResult.OK)
                    {
                        return Result.FromError("A Bundle cannot contain another bundle");
                    }
                }
            }

            return Result.FromSuccess();
        }
    }
}