using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Services;

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
        /// Create a dimensions adapter from a package adapter
        /// </summary>
        public static DimensionsAdapter CreateFrom(IPackageAdapter packageAdapter) =>
            new DimensionsAdapter
            {
                ProfileID = packageAdapter.DimsProfileID,
                Length = packageAdapter.DimsLength,
                Width = packageAdapter.DimsWidth,
                Height = packageAdapter.DimsHeight,
                Weight = packageAdapter.DimsWidth
            };

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
