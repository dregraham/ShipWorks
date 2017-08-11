namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom BuyDotCom order item
    /// </summary>
    public partial class BuyDotComOrderItemEntity
    {
        /// <summary>
        /// Get the original order id before saving
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            base.OnBeforeEntitySave();

            if (IsNew && OriginalOrderID == default(long))
            {
                OriginalOrderID = OrderID;
            }
        }
    }
}
