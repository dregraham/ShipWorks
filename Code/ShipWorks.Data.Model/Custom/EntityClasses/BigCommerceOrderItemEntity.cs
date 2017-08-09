namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom BigCommerce order item
    /// </summary>
    public partial class BigCommerceOrderItemEntity
    {
        /// <summary>
        /// Get the original order id before saving
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            base.OnBeforeEntitySave();

            if (IsNew)
            {
                OriginalOrderID = OrderID;
            }
        }
    }
}
