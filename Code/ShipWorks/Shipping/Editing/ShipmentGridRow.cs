using System;
using Divelements.SandGrid;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Custom SandGrid row class for shipments
    /// </summary>
    public class ShipmentGridRow : EntityGridRow
    {
        ShipmentEntity shipment;
        int sortIndex;

        PersonAdapter addressAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentGridRow(ShipmentEntity shipment, int sortIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            this.shipment = shipment;
            this.sortIndex = sortIndex;
        }

        /// <summary>
        /// The shipment this row represents
        /// </summary>
        public ShipmentEntity Shipment
        {
            get { return shipment; }
            set { shipment = value; }
        }

        /// <summary>
        /// The original index this shipment, or it's order, was originally in when selected for shipping.  This is what helps
        /// us preserve the original order grid sort within the shipping window.
        /// </summary>
        public int SortIndex
        {
            get { return sortIndex; }
        }

        /// <summary>
        /// The adapter to use for easy access to the ship address
        /// </summary>
        public PersonAdapter AddressAdapter
        {
            get
            {
                if (addressAdapter == null)
                {
                    addressAdapter = new PersonAdapter(shipment, "Ship");
                }

                return addressAdapter;
            }
        }

        /// <summary>
        /// Abstract implementation from EntityGridRow to provide the entity this row represents.
        /// </summary>
        public override EntityBase2 Entity
        {
            get
            {
                return shipment;
            }
        }

        /// <summary>
        /// Get the value to use for the given column
        /// </summary>
        public override object GetCellValue(GridColumn column)
        {
            ShipmentGridHiddenSortColumn sorter = column as ShipmentGridHiddenSortColumn;

            // Special case for providing unbound sort values
            if (sorter != null)
            {
                return sorter.GetSortValue(this);
            }

            return base.GetCellValue(column);
        }
    }
}
