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
    /// Partial class extention of the LLBLGen FedExShipmentEntity
    /// </summary>
    public partial class FedExShipmentEntity
    {
        /// <summary>
        /// Field value is changing
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, IEntityField2 field)
        {
            // If the FedEx RequestedLabelFormat changed, update the ShipmentEntity's version
            if (field.FieldIndex == (int)FedExShipmentFieldIndex.RequestedLabelFormat)
            {
                Shipment.RequestedLabelFormat = this.RequestedLabelFormat;
            }

            base.OnFieldValueChanged(originalValue, field);
        }
    }
}
