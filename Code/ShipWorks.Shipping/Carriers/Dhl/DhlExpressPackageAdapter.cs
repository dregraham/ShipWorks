using ShipWorks.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Shared.System.ComponentModel.DataAnnotations;
using Interapptive.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class DhlExpressPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly DhlExpressPackageEntity package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageAdapter" /> class.
        /// </summary>
        /// <param name="shipment">The shipment entity.</param>
        /// <param name="package">The package entity.</param>
        /// <param name="packageIndex">The index of this package adapter in a list of package adapters.</param>
        public DhlExpressPackageAdapter(ShipmentEntity shipment, DhlExpressPackageEntity package, int packageIndex)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));

            this.shipment = shipment;
            this.package = package;
            Index = packageIndex;
        }

        /// <summary>
        /// Id of the underlying package
        /// </summary>
        public long PackageId => package.DhlExpressPackageID;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index { get; set; }

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
                return shipment.DhlExpress.Packages.Count == 1 ? shipment.ContentWeight : package.Weight;
            }
            set
            {
                if (shipment.DhlExpress.Packages.Count == 1)
                {
                    // The shipment's content weight will need to be updated as well in the event
                    // that the weight change is a result of a customs item's weight changing
                    shipment.ContentWeight = value;
                }

                package.Weight = value;
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 999999, ErrorMessage = @"Please enter a valid additional weight.")]
        public double AdditionalWeight
        {
            get { return package.DimsWeight; }
            set { package.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return package.DimsAddWeight; }
            set { package.DimsAddWeight = value; }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [SuppressMessage("SonarQube", "S3237: \"value\" parameters should be used",
            Justification = "Package type isn't supported for DHL Express, so the setter is not necessary")]
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return -1; }
            set { /* Not supported by DHL */ }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Length must be greater than 1.")]
        public double DimsLength
        {
            get { return package.DimsLength; }
            set { package.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Width must be greater than 1.")]
        public double DimsWidth
        {
            get { return package.DimsWidth; }
            set { package.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(1, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Height must be greater than 1.")]
        public double DimsHeight
        {
            get { return package.DimsHeight; }
            set { package.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return package.DimsProfileID; }
            set { package.DimsProfileID = value; }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice{ get; set;}

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
                string errorMessage = InputValidation<DhlExpressPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<DhlExpressPackageAdapter>.Validate(this);
        }

        #endregion
    }
}

