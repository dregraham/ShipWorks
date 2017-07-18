using System;
using System.Linq;
using Interapptive.Shared.Business;
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

        // We cache this so we only have to look it up once
        static string baseObjectName = ((IEntityCore) new OrderEntity()).LLBLGenProEntityName;

        /// <summary>
        /// Set the Prefix for the order number
        /// </summary>
        public void ApplyOrderNumberPrefix(string prefix)
        {
            this.prefix = prefix;

            UpdateOrderNumberComplete();
        }

        /// <summary>
        /// Set the Postfix for the order number
        /// </summary>
        public void ApplyOrderNumberPostfix(string postfix)
        {
            this.postfix = postfix;

            UpdateOrderNumberComplete();
        }

        /// <summary>
        /// Field value is changing
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, int fieldIndex)
        {
            // If any parts of the order number are changing, we have to update the complete number
            if (fieldIndex == (int) OrderFieldIndex.OrderNumber)
            {
                UpdateOrderNumberComplete();
            }

            base.OnFieldValueChanged(originalValue, fieldIndex);
        }

        /// <summary>
        /// Update the OrderNumberComplete field
        /// </summary>
        private void UpdateOrderNumberComplete()
        {
            OrderNumberComplete =
                string.Format("{0}{1}{2}",
                prefix,
                OrderNumber,
                postfix);
        }
        /// <summary>
        /// Special processing to ensure change tracking for entity hierarchy
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            if (!HasBaseDirtyField(baseObjectName))
            {
                // Force the timestamp to update.  Update a column we don't filter on, so we don't falsely cause
                // filters to need to be recalculated
                Fields[(int) OrderFieldIndex.BillFax].IsChanged = true;
                Fields.IsDirty = true;
            }

            base.OnBeforeEntitySave();
        }

        /// <summary>
        /// Gets the billing address as a person adapter
        /// </summary>
        public PersonAdapter BillPerson => new PersonAdapter(this, "Bill");

        /// <summary>
        /// Shipping address as a person adapter
        /// </summary>
        public PersonAdapter ShipPerson
        {
            get { return new PersonAdapter(this, "Ship"); }
            set { PersonAdapter.Copy(value, ShipPerson); }
        }

        /// <summary>
        /// Get the shipment with the given id, if it exists
        /// </summary>
        public ShipmentEntity Shipment(long shipmentId) =>
            Shipments.FirstOrDefault(x => x.ShipmentID == shipmentId);
    }
}
