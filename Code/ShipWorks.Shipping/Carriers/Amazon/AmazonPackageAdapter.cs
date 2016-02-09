using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Implementation of IPackageAdapter for Amazon and ShipSense.
    /// </summary>
    public class AmazonPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        [SuppressMessage("SonarQube", "CS0067:The event is never used", Justification = "It is being used, but this message is still being shown.")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private readonly PropertyChangedHandler handler;
        private IInsuranceChoice insuranceChoice;

        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public AmazonPackageAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            this.shipment = shipment;
            this.insuranceChoice = new AmazonInsuranceChoice(shipment);
        }

        /// <summary>
        /// Id of the underlying package
        /// </summary>
        public long PackageId => -1;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return 1; }
#pragma warning disable S3237 // "value" parameters should be used
            set { /* Not applicable */ }
#pragma warning restore S3237 // "value" parameters should be used
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double Length
        {
            get { return shipment.Amazon.DimsLength; }
            set
            {
                handler.Set(nameof(Length), v => shipment.Amazon.DimsLength = value, shipment.Amazon.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double Width
        {
            get { return shipment.Amazon.DimsWidth; }
            set
            {
                handler.Set(nameof(Width), v => shipment.Amazon.DimsWidth = value, shipment.Amazon.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double Height
        {
            get { return shipment.Amazon.DimsHeight; }
            set
            {
                handler.Set(nameof(Height), v => shipment.Amazon.DimsHeight = value, shipment.Amazon.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid weight.")]
        public double Weight
        {
            get { return shipment.ContentWeight; }
            set
            {
                handler.Set(nameof(Weight), v => shipment.ContentWeight = value, shipment.ContentWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid additional weight.")]
        public double AdditionalWeight
        {
            get { return shipment.Amazon.DimsWeight; }
            set
            {
                handler.Set(nameof(AdditionalWeight), v => shipment.Amazon.DimsWeight = value, shipment.Amazon.DimsWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return shipment.Amazon.DimsAddWeight; }
            set
            {
                handler.Set(nameof(ApplyAdditionalWeight), v => shipment.Amazon.DimsAddWeight = value, shipment.Amazon.DimsAddWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public PackageTypeBinding PackagingType
        {
            get { return null; }
#pragma warning disable S3237 // "value" parameters should be used
            set { /* Not applicable */ }
#pragma warning restore S3237 // "value" parameters should be used
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double DimsLength
        {
            get { return shipment.Amazon.DimsLength; }
            set
            {
                handler.Set(nameof(DimsLength), v => shipment.Amazon.DimsLength = value, shipment.Amazon.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double DimsWidth
        {
            get { return shipment.Amazon.DimsWidth; }
            set
            {
                handler.Set(nameof(DimsWidth), v => shipment.Amazon.DimsWidth = value, shipment.Amazon.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double DimsHeight
        {
            get { return shipment.Amazon.DimsHeight; }
            set
            {
                handler.Set(nameof(DimsHeight), v => shipment.Amazon.DimsHeight = value, shipment.Amazon.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return shipment.Amazon.DimsProfileID; }
            set
            {
                handler.Set(nameof(DimsProfileID), v => shipment.Amazon.DimsProfileID = value, shipment.Amazon.DimsProfileID, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice
        {
            get { return insuranceChoice; }
            set
            {
                handler.Set(nameof(InsuranceChoice), ref insuranceChoice, value);
            }
        }

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

            string rawValue = $"{Length}-{Width}-{Height}-{Weight}-{AdditionalWeight}-{ApplyAdditionalWeight}";

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
                string errorMessage = InputValidation<AmazonPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<AmazonPackageAdapter>.Validate(this);
        }

        #endregion
    }
}
