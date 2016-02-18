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

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class FedExPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private readonly PropertyChangedHandler handler;

        private readonly ShipmentEntity shipmentEntity;
        private readonly FedExPackageEntity packageEntity;
        private int index;
        private IInsuranceChoice insuranceChoice;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageAdapter" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="packageEntity">The package entity.</param>
        /// <param name="packageIndex">The index of this package adapter in a list of package adapters.</param>
        public FedExPackageAdapter(ShipmentEntity shipmentEntity, FedExPackageEntity packageEntity, int packageIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentEntity.FedEx, nameof(shipmentEntity.FedEx));

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            this.shipmentEntity = shipmentEntity;
            this.packageEntity = packageEntity;
            this.Index = packageIndex;
            this.insuranceChoice = new InsuranceChoice(shipmentEntity, packageEntity, packageEntity, packageEntity);
        }

        /// <summary>
        /// Id of the underlying package
        /// </summary>
        public long PackageId => packageEntity.FedExPackageID;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return index; }
            set
            {
                handler.Set(nameof(Index), ref index, value);
            }
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
                return shipmentEntity.FedEx.Packages.Count == 1 ? shipmentEntity.ContentWeight : packageEntity.Weight;
            }
            set
            {
                if (shipmentEntity.FedEx.Packages.Count == 1)
                {
                    // The shipment's content weight will need to be updated as well in the event
                    // that the weight change is a result of a customs item's weight changing
                    shipmentEntity.ContentWeight = value;
                }

                handler.Set(nameof(Weight), v => packageEntity.Weight = value, packageEntity.Weight, value, false);
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
            set
            {
                handler.Set(nameof(AdditionalWeight), v => packageEntity.DimsWeight = value, packageEntity.DimsWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return packageEntity.DimsAddWeight; }
            set
            {
                handler.Set(nameof(ApplyAdditionalWeight), v => packageEntity.DimsAddWeight = value, packageEntity.DimsAddWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return shipmentEntity.FedEx.PackagingType; }
            set { shipmentEntity.FedEx.PackagingType = value; }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double DimsLength
        {
            get { return packageEntity.DimsLength; }
            set
            {
                handler.Set(nameof(DimsLength), v => packageEntity.DimsLength = value, packageEntity.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double DimsWidth
        {
            get { return packageEntity.DimsWidth; }
            set
            {
                handler.Set(nameof(DimsWidth), v => packageEntity.DimsWidth = value, packageEntity.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double DimsHeight
        {
            get { return packageEntity.DimsHeight; }
            set
            {
                handler.Set(nameof(DimsHeight), v => packageEntity.DimsHeight = value, packageEntity.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return packageEntity.DimsProfileID; }
            set
            {
                handler.Set(nameof(DimsProfileID), v => packageEntity.DimsProfileID = value, packageEntity.DimsProfileID, value, false);
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
            // If there is more than one package, only declared value is allowed, so just return.
            if (shipmentEntity.FedEx.Packages.Count > 1)
            {
                return;
            }

            if (shipmentEntity.InsuranceProvider != shippingSettings.FedExInsuranceProvider)
            {
                shipmentEntity.InsuranceProvider = shippingSettings.FedExInsuranceProvider;
            }

            if (packageEntity.InsurancePennyOne != shippingSettings.FedExInsurancePennyOne)
            {
                packageEntity.InsurancePennyOne = shippingSettings.FedExInsurancePennyOne;
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
                string errorMessage = InputValidation<FedExPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<FedExPackageAdapter>.Validate(this);
        }

        #endregion
    }
}
