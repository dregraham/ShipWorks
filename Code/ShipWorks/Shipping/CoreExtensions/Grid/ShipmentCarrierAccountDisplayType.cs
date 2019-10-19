using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Display type for showing the Carrier Account used for a shipment
    /// </summary>
    class ShipmentCarrierAccountDisplayType : GridTextDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentCarrierAccountDisplayType()
        {
            PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the display text to use for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            var account = ShippingManager.GetCarrierAccount((ShipmentEntity) value);
            return account != null ? account.AccountDescription : string.Empty;
        }
    }
}
