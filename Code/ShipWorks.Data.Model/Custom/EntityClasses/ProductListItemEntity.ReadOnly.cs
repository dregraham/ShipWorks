namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity 'ProductListItem'.
    /// </summary>
    public partial class ReadOnlyProductListItemEntity
    {
        /// <summary>
        /// Dimensions as a single field
        /// </summary>
        public string Dimensions => string.Join("x", Length, Width, Height);
    }
}
