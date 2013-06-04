using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Used to have a common way of accessing dimensions from any entity
    /// </summary>
    public class DimensionsAdapter
    {
        EntityBase2 entity;
        string fieldPrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsAdapter(EntityBase2 entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.entity = entity;
            this.fieldPrefix = "Dims";
        }

        /// <summary>
        /// The database id of the DimensionsProfile
        /// </summary>
        public long ProfileID
        {
            get { return (long) entity.Fields[fieldPrefix + "ProfileID"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "ProfileID", value); }
        }

        /// <summary>
        /// Length of the packaging
        /// </summary>
        public double Length
        {
            get { return (double) entity.Fields[fieldPrefix + "Length"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "Length", value); }
        }

        /// <summary>
        /// Width of the packaging
        /// </summary>
        public double Width
        {
            get { return (double) entity.Fields[fieldPrefix + "Width"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "Width", value); }
        }

        /// <summary>
        /// Height of the packaging
        /// </summary>
        public double Height
        {
            get { return (double) entity.Fields[fieldPrefix + "Height"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "Height", value); }
        }

        /// <summary>
        /// The Girth of the packaging
        /// </summary>
        public double Girth
        {
            get { return 2 * (Width + Height); }
        }

        /// <summary>
        /// Weight of the packaging
        /// </summary>
        public double Weight
        {
            get { return (double) entity.Fields[fieldPrefix + "Weight"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "Weight", value); }
        }

        /// <summary>
        /// Controls if the packaging weight should be added to the total weight before processing
        /// </summary>
        public bool AddWeight
        {
            get { return (bool) entity.Fields[fieldPrefix + "AddWeight"].CurrentValue; }
            set { entity.SetNewFieldValue(fieldPrefix + "AddWeight", value); }
        }
    }
}
