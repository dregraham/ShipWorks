using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Tests.Shared
{
    public class TestPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("CSharp", "CS0067: ", Justification = "For now, we're fine with fire and forget here")]
        public event PropertyChangedEventHandler PropertyChanged;

        public long PackageId => -1;
        public int Index { get; set; }
        public double Weight { get; set; }
        public double AdditionalWeight { get; set; }
        public bool ApplyAdditionalWeight { get; set; }
        public double DimsLength { get; set; }
        public double DimsWidth { get; set; }
        public double DimsHeight { get; set; }
        public long DimsProfileID { get; set; }
        public PackageTypeBinding PackagingType { get; set; }
        public IInsuranceChoice InsuranceChoice { get; set; }

        public string HashCode()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HashCode)));
            }

            return this.HashCode();
        }

        /// <summary>
        /// Update the insurance fields on the package
        /// </summary>
        public void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            // Nothing to do here
        }

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                string errorMessage = InputValidation<TestPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<TestPackageAdapter>.Validate(this);
        }

        #endregion
    }
}
