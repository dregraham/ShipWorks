using System;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class EtsyOrderEntity
    {
        static Action<EtsyOrderEntity> effectiveOnlineStatusAlgorithm = null;

        /// <summary>
        /// Set the algorithm to use for calculating the effective payment status of the item
        /// </summary>
        public static void SetEffectiveOnlineStatusAlgorithm(Action<EtsyOrderEntity> algorithm)
        {
            effectiveOnlineStatusAlgorithm = algorithm;
        }

        /// <summary>
        /// The value of a field has changed
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, int fieldIndex)
        {
            if (fieldIndex == (int) EtsyOrderFieldIndex.WasPaid || fieldIndex == (int) EtsyOrderFieldIndex.WasShipped)
            {
                effectiveOnlineStatusAlgorithm(this);
            }

            base.OnFieldValueChanged(originalValue, fieldIndex);
        }
    }
}
