using System;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom behavior for the EbayOrderItem entity
    /// </summary>
    public partial class EbayOrderItemEntity
    {
        static Func<EbayOrderItemEntity, int> effectivePaymentMethodAlgorithm = null;
        static Func<EbayOrderItemEntity, int> effectiveCheckoutStatusAltorithm = null;

        /// <summary>
        /// Unique identifier of a line item, as defined by eBay based on ItemID and TransactionID
        /// </summary>
        public string OrderLineItemID
        {
            get
            {
                return string.Format("{0}-{1}", EbayItemID, EbayTransactionID);
            }
        }

        /// <summary>
        /// Set the algorithm to use for calculating the effective paymeant status of the item
        /// </summary>
        public static void SetEffectivePaymentMethodAlgorithm(Func<EbayOrderItemEntity, int> algorithm)
        {
            effectivePaymentMethodAlgorithm = algorithm;
        }

        /// <summary>
        /// Set the algorithm to use for calculating the effective checkout staths of the item
        /// </summary>
        public static void SetEffectiveCheckoutStatusAlgorithm(Func<EbayOrderItemEntity, int> algorithm)
        {
            effectiveCheckoutStatusAltorithm = algorithm;
        }

        /// <summary>
        /// The value of a field has changed
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, int field)
        {
            EffectiveCheckoutStatus = effectiveCheckoutStatusAltorithm(this);
            EffectivePaymentMethod = effectivePaymentMethodAlgorithm(this);

            base.OnFieldValueChanged(originalValue, field);
        }

        /// <summary>
        /// Special procesing to ensure change tracking for entity hierarchy
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            // Keep the foreign key up to date
            this.LocalEbayOrderID = OrderID;

            base.OnBeforeEntitySave();
        }
    }
}
