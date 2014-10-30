using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen OnTracShipmentEntity
    /// </summary>
    public partial class OnTracShipmentEntity
    {
        /// <summary>
        /// Field value is changing
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, IEntityField2 field)
        {
            // If the RequestedLabelFormat changed, update the ShipmentEntity's version
            if (field.FieldIndex == (int)OnTracShipmentFieldIndex.RequestedLabelFormat)
            {
                Shipment.RequestedLabelFormat = this.RequestedLabelFormat;
            }

            base.OnFieldValueChanged(originalValue, field);
        }
    }
}
