﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public class UpsPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipmentEntity;
        private readonly UpsPackageEntity packageEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPackageAdapter" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="packageEntity">The package entity.</param>
        public UpsPackageAdapter(ShipmentEntity shipmentEntity, UpsPackageEntity packageEntity)
        {
            this.shipmentEntity = shipmentEntity;
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
            get
            {
                // The shipment's content weight is updated when one of the customs items' quantity or weight changes 
                // when there is a only single package. When there are multiple packages, the weight differences are 
                // distributed evenly across packages, so the content weight does not have to be modified via the package adapter.
                return shipmentEntity.Ups.Packages.Count == 1 ? shipmentEntity.ContentWeight : packageEntity.Weight;
            }
            set
            {
                if (shipmentEntity.Ups.Packages.Count == 1)
                {
                    // The shipment's content weight will need to be updated as well in the event
                    // that the weight change is a result of a customs item's weight changing
                    shipmentEntity.ContentWeight = value;
                }

                packageEntity.Weight = value;
            }
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

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Length, Width, Height, Weight, AdditionalWeight, ApplyAdditionalWeight);

            return stringHash.Hash(rawValue, string.Empty);
        }
    }
}
