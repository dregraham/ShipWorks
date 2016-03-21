using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Wrap a package adapter to get certain fields to notify property changes
    /// </summary>
    public class PackageAdapterWrapper : IPackageAdapter, INotifyPropertyChanged
    {
        readonly IPackageAdapter packageAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packageAdapter"></param>
        public PackageAdapterWrapper(IPackageAdapter packageAdapter)
        {
            this.packageAdapter = packageAdapter;
        }

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return packageAdapter.Index; }
            set
            {
                if (packageAdapter.Index != value)
                {
                    packageAdapter.Index = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Index)));
                }
            }
        }

        /// <summary>
        /// Id of the package to which this adapter applies
        /// </summary>
        public long PackageId
        {
            get { return packageAdapter.PackageId; }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return packageAdapter.Weight; }
            set { packageAdapter.Weight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return packageAdapter.AdditionalWeight; }
            set { packageAdapter.AdditionalWeight = value; }
        }

        /// <summary>
        /// Gets or sets the whether the additional weight should be applied.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return packageAdapter.ApplyAdditionalWeight; }
            set { packageAdapter.ApplyAdditionalWeight = value; }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        public double DimsLength
        {
            get { return packageAdapter.DimsLength; }
            set { packageAdapter.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        public double DimsWidth
        {
            get { return packageAdapter.DimsWidth; }
            set { packageAdapter.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        public double DimsHeight
        {
            get { return packageAdapter.DimsHeight; }
            set { packageAdapter.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        public long DimsProfileID
        {
            get { return packageAdapter.DimsProfileID; }
            set { packageAdapter.DimsProfileID = value; }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        public int PackagingType
        {
            get { return packageAdapter.PackagingType; }
            set { packageAdapter.PackagingType = value; }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        public IInsuranceChoice InsuranceChoice
        {
            get { return packageAdapter.InsuranceChoice; }
            set { packageAdapter.InsuranceChoice = value; }
        }

        /// <summary>
        /// Gets an error, if any
        /// </summary>
        public string Error
        {
            get { return packageAdapter.Error; }
        }

        /// <summary>
        /// Gets an error for the given column name
        /// </summary>
        public string this[string columnName]
        {
            get { return this[columnName]; }
        }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            return packageAdapter.HashCode();
        }

        /// <summary>
        /// Update the insurance fields on the package
        /// </summary>
        public void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings)
        {
            packageAdapter.UpdateInsuranceFields(shippingSettings);
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}