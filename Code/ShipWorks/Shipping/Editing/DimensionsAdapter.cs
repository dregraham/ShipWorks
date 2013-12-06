using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Used to have a common way of accessing dimensions from any entity
    /// </summary>
    public class DimensionsAdapter : EntityAdapter
    {
        /// <summary>
        /// Creates a new instance of the adapter that maintains its own values, and has no backing entity.
        /// </summary>
        public DimensionsAdapter()
            : base()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsAdapter(EntityBase2 entity)
            : base(entity, "Dims")
        {

        }

        /// <summary>
        /// The database id of the DimensionsProfile
        /// </summary>
        public long ProfileID
        {
            get { return GetField<long>("ProfileID"); }
            set { SetField("ProfileID", value); }
        }

        /// <summary>
        /// Length of the packaging
        /// </summary>
        public double Length
        {
            get { return GetField<double>("Length"); }
            set { SetField("Length", value); }
        }

        /// <summary>
        /// Width of the packaging
        /// </summary>
        public double Width
        {
            get { return GetField<double>("Width"); }
            set { SetField("Width", value); }
        }

        /// <summary>
        /// Height of the packaging
        /// </summary>
        public double Height
        {
            get { return GetField<double>("Height"); }
            set { SetField("Height", value); }
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
            get { return GetField<double>("Weight"); }
            set { SetField("Weight", value); }
        }

        /// <summary>
        /// Controls if the packaging weight should be added to the total weight before processing
        /// </summary>
        public bool AddWeight
        {
            get { return GetField<bool>("AddWeight"); }
            set { SetField("AddWeight", value); }
        }
    }
}
