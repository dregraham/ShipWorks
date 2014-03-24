using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelPackageAdapter : IPackageAdapter
    {
        private readonly IParcelPackageEntity packageEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelPackageAdapter"/> class.
        /// </summary>
        /// <param name="packageEntity">The package entity.</param>
        public iParcelPackageAdapter(IParcelPackageEntity packageEntity)
        {
            this.packageEntity = packageEntity;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return packageEntity.DimsLength; }
            set { packageEntity.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return packageEntity.DimsWidth; }
            set { packageEntity.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return packageEntity.DimsHeight; }
            set { packageEntity.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return packageEntity.Weight; }
            set { packageEntity.Weight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return packageEntity.DimsWeight; }
            set { packageEntity.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return packageEntity.DimsAddWeight; }
            set { packageEntity.DimsAddWeight = value; }
        }
    }
}
