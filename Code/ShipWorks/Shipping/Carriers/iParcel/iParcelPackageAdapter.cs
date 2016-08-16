using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class iParcelPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipmentEntity;
        private readonly IParcelPackageEntity packageEntity;
        private int index;
        private IInsuranceChoice insuranceChoice;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelPackageAdapter" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="packageEntity">The package entity.</param>
        /// <param name="packageIndex">The index of this package adapter in a list of package adapters.</param>
        public iParcelPackageAdapter(ShipmentEntity shipmentEntity, IParcelPackageEntity packageEntity, int packageIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentEntity.IParcel, nameof(shipmentEntity.IParcel));

            this.shipmentEntity = shipmentEntity;
            this.packageEntity = packageEntity;
            this.Index = packageIndex;
            this.insuranceChoice = new InsuranceChoice(shipmentEntity, packageEntity, packageEntity, packageEntity);
        }

        /// <summary>
        /// Id of the underlying package
        /// </summary>
        public long PackageId => packageEntity.IParcelPackageID;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid weight.")]
        public double Weight
        {
            get
            {
                // The shipment's content weight is updated when one of the customs items' quantity or weight changes
                // when there is a only single package. When there are multiple packages, the weight differences are
                // distributed evenly across packages, so the content weight does not have to be modified via the package adapter.
                return shipmentEntity.IParcel.Packages.Count == 1 ? shipmentEntity.ContentWeight : packageEntity.Weight;
            }
            set
            {
                if (shipmentEntity.IParcel.Packages.Count == 1)
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
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid additional weight.")]
        public double AdditionalWeight
        {
            get { return packageEntity.DimsWeight; }
            set { packageEntity.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return packageEntity.DimsAddWeight; }
            set { packageEntity.DimsAddWeight = value; }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return -1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double DimsLength
        {
            get { return packageEntity.DimsLength; }
            set { packageEntity.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double DimsWidth
        {
            get { return packageEntity.DimsWidth; }
            set { packageEntity.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double DimsHeight
        {
            get { return packageEntity.DimsHeight; }
            set { packageEntity.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return packageEntity.DimsProfileID; }
            set { packageEntity.DimsProfileID = value; }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice
        {
            get { return insuranceChoice; }
            set { insuranceChoice = value; }
        }

        /// <summary>
        /// Update the insurance fields on the package
        /// </summary>
        public void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // If there is more than one package, only declared value is allowed, so just return.
            if (shipmentEntity.IParcel.Packages.Count > 1)
            {
                return;
            }

            if (shipmentEntity.InsuranceProvider != shippingSettings.IParcelInsuranceProvider)
            {
                shipmentEntity.InsuranceProvider = shippingSettings.IParcelInsuranceProvider;
            }

            if (packageEntity.InsurancePennyOne != shippingSettings.IParcelInsurancePennyOne)
            {
                packageEntity.InsurancePennyOne = shippingSettings.IParcelInsurancePennyOne;
            }

            InsuranceChoice = new InsuranceChoice(shipmentEntity, packageEntity, packageEntity, packageEntity);
        }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", DimsLength, DimsWidth, DimsHeight, Weight, AdditionalWeight, ApplyAdditionalWeight);

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
                string errorMessage = InputValidation<iParcelPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<iParcelPackageAdapter>.Validate(this);
        }

        #endregion
    }
}
