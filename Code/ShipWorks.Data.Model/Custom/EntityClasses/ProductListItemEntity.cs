namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Entity 'ProductListItem'.
    /// </summary>
    public partial class ProductListItemEntity
    {
        /// <summary>
        /// Dimensions as a single field
        /// </summary>
        public string Dimensions => string.Join("x", Length, Width, Height);
    }
}
