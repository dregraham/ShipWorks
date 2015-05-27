using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using System.Drawing.Imaging;
using ShipWorks.Filters.Content.Conditions.Shipments;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Represents label data for a single label output for a shipment to be generated into template xml output
    /// </summary>
    public class TemplateLabelData
    {
        long? packageID;
        string name;
        TemplateLabelCategory category;
        DataResourceReference resource;
        bool canPrintThermal;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateLabelData(long? packageID, string name, TemplateLabelCategory category, DataResourceReference resource) :
            this(packageID, name, category, resource, true)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateLabelData(long? packageID, string name, TemplateLabelCategory category, DataResourceReference resource, bool canPrintThermal)
        {
            this.packageID = packageID;
            this.name = name;
            this.category = category;
            this.resource = resource;
            this.canPrintThermal = canPrintThermal;
        }

        /// <summary>
        /// The ID of the shipment's package that generated this label, or null if the shipment does not support packages
        /// </summary>
        public long? PackageID
        {
            get { return packageID; }
        }

        /// <summary>
        /// The name\description of the label
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The category of the label
        /// </summary>
        public TemplateLabelCategory Category
        {
            get { return category; }
        }

        /// <summary>
        /// The resource reference that refers to the actual label data
        /// </summary>
        public DataResourceReference Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// Checks whether this label can be printed with a thermal printer
        /// </summary>
        public bool CanPrintThermal
        {
            get
            {
                return canPrintThermal;   
            }
        }
    }
}
