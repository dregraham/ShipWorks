using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    internal class GridProviderDisplayType : GridEnumDisplayType<ShipmentTypeCode>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GridProviderDisplayType(EnumSortMethod sortMethod) 
            : base(sortMethod)
        {
            GridHyperlinkDecorator gridHyperlinkDecorator = new GridHyperlinkDecorator();
            gridHyperlinkDecorator.QueryEnabled += (sender, args) => args.Enabled = LinkEnabled(args.Entity);
            
            Decorate(gridHyperlinkDecorator);
        }

        bool LinkEnabled(EntityBase2 argShipment)
        {
            ShipmentEntity shipment = argShipment as ShipmentEntity;

            if (shipment == null)
            {
                return false;
            }

            return !shipment.Processed;
        }
    }
}
