﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the OrderEntity
    /// </summary>
    public partial class OrderEntity
    {
        string prefix = "";
        string postfix = "";

        bool settingOrderNumberComplete = false;

        // We cache this so we only have to look it up once
        static string baseObjectName = new OrderEntity().LLBLGenProEntityName;

        /// <summary>
        /// Set the Prefix for the ordernumber
        /// </summary>
        public void ApplyOrderNumberPrefix(string prefix)
        {
            this.prefix = prefix;

            UpdateOrderNumberComplete();
        }

        /// <summary>
        /// Set the Postfix for the ordernumber
        /// </summary>
        public void ApplyOrderNumberPostfix(string postfix)
        {
            this.postfix = postfix;

            UpdateOrderNumberComplete();
        }

        /// <summary>
        /// Trying to set a value of a field
        /// </summary>
        protected override void OnSetValue(int fieldIndex, object valueToSet, out bool cancel)
        {
            // User can't directly update OrderNumberComplete.
            if (fieldIndex == (int) OrderFieldIndex.OrderNumberComplete && !settingOrderNumberComplete)
            {
                throw new InvalidOperationException("Cannot set OrderNumberComplete directly.  Use OrderNumber and ApplyOrderNumber*Fix.");
            }

            base.OnSetValue(fieldIndex, valueToSet, out cancel);
        }

        /// <summary>
        /// Field value is changing
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, IEntityField2 field)
        {
            // If any parts of the order number are changing, we have to update the complete number
            if (field.FieldIndex == (int) OrderFieldIndex.OrderNumber)
            {
                UpdateOrderNumberComplete();
            }
            
            base.OnFieldValueChanged(originalValue, field);
        }

        /// <summary>
        /// Update the OrderNumberComplete field
        /// </summary>
        private void UpdateOrderNumberComplete()
        {
            settingOrderNumberComplete = true;

            OrderNumberComplete =
                string.Format("{0}{1}{2}",
                prefix,
                OrderNumber,
                postfix);

            settingOrderNumberComplete = false;
        }
        /// <summary>
        /// Special procesing to ensure change tracking for entity hierarchy
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            if (!HasBaseDirtyField(baseObjectName))
            {
                // Force the timestamp to update.  Update a column we don't filter on, so we don't falsely cause
                // filters to need to be recalc'd
                Fields[(int)OrderFieldIndex.BillFax].IsChanged = true;
                Fields.IsDirty = true;
            }

            base.OnBeforeEntitySave();
        }
    }
}
