using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.CoreExtensions.Grid
{
    /// <summary>
    /// Display type for showing if a shipment is insured
    /// </summary>
    public class ShipmentInsuredDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Get the value to use for the given entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the display text to use
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            ShipmentEntity shipment = value as ShipmentEntity;
            if (shipment == null)
            {
                return null;
            }

            if (!shipment.Insurance)
            {
                return EnumHelper.GetDescription(ShipmentInsuredType.None);
            }
            else
            {
                if (shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
                {
                    return EnumHelper.GetDescription(ShipmentInsuredType.Carrier);
                }
                else
                {
                    return EnumHelper.GetDescription(ShipmentInsuredType.ShipWorks);
                }
            }
        }
    }
}
