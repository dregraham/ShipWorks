﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRatePackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRatePackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public BestRatePackageAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return shipment.BestRate.DimsLength; }
            set { shipment.BestRate.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return shipment.BestRate.DimsWidth; }
            set { shipment.BestRate.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return shipment.BestRate.DimsHeight; }
            set { shipment.BestRate.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return shipment.ContentWeight; }
            set { shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return shipment.BestRate.DimsWeight; }
            set { shipment.BestRate.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return shipment.BestRate.DimsAddWeight; }
            set { shipment.BestRate.DimsAddWeight = value; }
        }
    }
}
