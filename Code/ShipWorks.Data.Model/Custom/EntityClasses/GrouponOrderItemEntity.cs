namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom Groupon order item
    /// </summary>
    public partial class GrouponOrderItemEntity
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
