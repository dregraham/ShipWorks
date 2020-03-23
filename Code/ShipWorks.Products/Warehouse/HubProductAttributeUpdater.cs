using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update product attributes
    /// </summary>
    public class HubProductAttributeUpdater : IHubProductItemUpdater
    {
        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        public void UpdateProductVariant(ProductVariantEntity productVariant, WarehouseProduct warehouseProduct)
        {
            productVariant.AttributeValues.RemovedEntitiesTracker = new ProductVariantAttributeValueCollection();
            var attributesToRemove = productVariant.AttributeValues
                .Where(x => warehouseProduct.Attributes.None(attribute => attribute.Name == x.ProductAttribute.AttributeName))
                .ToList();

            foreach (var attribute in attributesToRemove)
            {
                productVariant.AttributeValues.Remove(attribute);
            }

            foreach (var hubAttribute in warehouseProduct.Attributes)
            {
                var attributeValue = productVariant.AttributeValues.First(x => x.ProductAttribute.AttributeName == hubAttribute.Name);
                if (attributeValue == null)
                {
                    var attribute = productVariant.Product.Attributes.FirstOrDefault(x => x.AttributeName == hubAttribute.Name) ??
                        productVariant.Product.Attributes.AddNew();
                    attribute.AttributeName = hubAttribute.Name;

                    attributeValue = productVariant.AttributeValues.AddNew();
                    attributeValue.ProductAttribute = attribute;
                }
                attributeValue.AttributeValue = hubAttribute.Value;
            }
        }
    }
}
