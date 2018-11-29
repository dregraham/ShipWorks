namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ProductListItem'.
    /// </summary>
    public partial interface IProductListItemEntity
    {
        /// <summary>
        /// Dimensions as a single field
        /// </summary>
        string Dimensions { get; }
    }
}
