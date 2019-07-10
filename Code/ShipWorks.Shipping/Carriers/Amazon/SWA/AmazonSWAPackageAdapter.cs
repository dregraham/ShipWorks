﻿using ShipWorks.Shipping.Services;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Shared.System.ComponentModel.DataAnnotations;
using Interapptive.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class AmazonSWAPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSWAPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public AmazonSWAPackageAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSWA, nameof(shipment.AmazonSWA));

            this.shipment = shipment;
            this.InsuranceChoice = null;
        }

        /// <summary>
        /// Id of the underlying package
        /// </summary>
        public long PackageId => -1;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [SuppressMessage("SonarQube", "S3237: \"value\" parameters should be used",
            Justification = "Package type isn't supported for AmazonSWA, so the setter is not necessary")]
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return 1; }
            set { /* Not supported by AmazonSWA */ }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid weight.")]
        public double Weight
        {
            get { return shipment.ContentWeight; }
            set { shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid additional weight.")]
        public double AdditionalWeight
        {
            get { return shipment.AmazonSWA.DimsWeight; }
            set { shipment.AmazonSWA.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return shipment.AmazonSWA.DimsAddWeight; }
            set { shipment.AmazonSWA.DimsAddWeight = value; }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [SuppressMessage("SonarQube", "S3237: \"value\" parameters should be used",
            Justification = "Package type isn't supported for AmazonSWA, so the setter is not necessary")]
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return -1; }
            set { /* Not supported by AmazonSWA */ }
        }

        /// <summary>
        /// Gets the packaging type name.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PackagingTypeName
        {
            get => string.Empty;
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double DimsLength
        {
            get { return shipment.AmazonSWA.DimsLength; }
            set { shipment.AmazonSWA.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double DimsWidth
        {
            get { return shipment.AmazonSWA.DimsWidth; }
            set { shipment.AmazonSWA.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double DimsHeight
        {
            get { return shipment.AmazonSWA.DimsHeight; }
            set { shipment.AmazonSWA.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return shipment.AmazonSWA.DimsProfileID; }
            set { shipment.AmazonSWA.DimsProfileID = value; }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice { get; set; }

        /// <summary>
        /// Update the insurance fields on the package
        /// </summary>
        public void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // Nothing to do here since they only have ShipWorks insurance as an option
        }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = $"{DimsLength}-{DimsWidth}-{DimsHeight}-{Weight}-{AdditionalWeight}-{ApplyAdditionalWeight}";

            return stringHash.Hash(rawValue, string.Empty);
        }

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                string errorMessage = InputValidation<AmazonSWAPackageAdapter>.Validate(this, columnName);

                return errorMessage;
            }
        }

        /// <summary>
        /// IDataErrorInfo Error implementation
        /// </summary>
        public string Error => null;

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public ICollection<string> AllErrors()
        {
            return InputValidation<AmazonSWAPackageAdapter>.Validate(this);
        }
        #endregion
    }
}
