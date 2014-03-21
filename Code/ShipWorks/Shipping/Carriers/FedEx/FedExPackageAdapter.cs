using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExPackageAdapter : IPackageAdapter
    {
        private readonly FedExPackageEntity packageEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageAdapter"/> class.
        /// </summary>
        /// <param name="packageEntity">The package entity.</param>
        public FedExPackageAdapter(FedExPackageEntity packageEntity)
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
            // This is working like you'd expect with single package shipments, but 
            // will probably need to change once we do multiple packages.
            get { return packageEntity.FedExShipment.Shipment.ContentWeight; }
            set { packageEntity.FedExShipment.Shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return packageEntity.DimsWeight; }
            set { packageEntity.DimsWeight = value; }
        }
    }
}
