using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class EtsyOrderEntity
    {
        static Action<EtsyOrderEntity> effectiveOnlineStatusAlgorithm = null;

        /// <summary>
        /// Set the algorithm to use for calculating the effective paymeant status of the item
        /// </summary>
        public static void SetEffectiveOnlineStatusAlgorithm(Action<EtsyOrderEntity> algorithm)
        {
            effectiveOnlineStatusAlgorithm = algorithm;
        }

        /// <summary>
        /// The value of a field has changed
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, IEntityField2 field)
        {
            if (field.FieldIndex == (int)EtsyOrderFieldIndex.WasPaid || field.FieldIndex == (int)EtsyOrderFieldIndex.WasShipped)
            {
                effectiveOnlineStatusAlgorithm(this);
            }

            base.OnFieldValueChanged(originalValue, field);
        }
    }
}
