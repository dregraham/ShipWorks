using System;
using Divelements.SandGrid;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Custom SandGrid row class for serviceStatuses
    /// </summary>
    public class ServiceStatusEntityGridRow : EntityGridRow
    {
        ServiceStatusEntity serviceStatus;
        int sortIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceStatusEntityGridRow(ServiceStatusEntity serviceStatus, int sortIndex)
        {
            if (serviceStatus == null)
            {
                throw new ArgumentNullException("serviceStatus");
            }

            this.serviceStatus = serviceStatus;
            this.sortIndex = sortIndex;
        }

        /// <summary>
        /// The serviceStatus this row represents
        /// </summary>
        public ServiceStatusEntity ServiceStatus
        {
            get { return serviceStatus; }
            set { serviceStatus = value; }
        }

        /// <summary>
        /// The original index this ServiceStatus, or it's order, was originally in when selected for shipping.  This is what helps
        /// us preserve the original order grid sort within the shipping window.
        /// </summary>
        public int SortIndex
        {
            get { return sortIndex; }
        }

        /// <summary>
        /// Abstract implementation from EntityGridRow to provide the entity this row represents.
        /// </summary>
        public override EntityBase2 Entity
        {
            get
            {
                return serviceStatus;
            }
        }

        /// <summary>
        /// Get the value to use for the given column
        /// </summary>
        public override object GetCellValue(GridColumn column)
        {
            var sorter = column as GridHiddenSortColumn<ServiceStatusEntityGridRow>;

            // Special case for providing unbound sort values
            if (sorter != null)
            {
                return sorter.GetSortValue(this);
            }

            return base.GetCellValue(column);
        }
    }
}
