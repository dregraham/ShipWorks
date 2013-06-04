using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Error provider for processed shpments
    /// </summary>
    class ShipmentGridRowProcessingErrorProvider : EntityGridRowErrorProvider
    {
        /// <summary>
        /// Get the error to display for the given shipment entity
        /// </summary>
        public override string GetError(EntityBase2 entity)
        {
            Exception error;
            if (ShippingDlg.ProcessingErrors.TryGetValue(((ShipmentEntity) entity).ShipmentID, out error))
            {
                return error.Message;
            }

            return null;
        }
    }
}
