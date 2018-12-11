namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ProductVariant'.
    /// </summary>
    public partial interface IProductVariantEntity
    {
        /// <summary>
        /// The default sku for the product variant
        /// </summary>
        string DefaultSku { get; }
    }
}