
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// An interface intended to be used for shuffling package data between classes.
    /// </summary>
    public interface IPackageAdapter : INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        double Weight { get; set; }
        
        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        double AdditionalWeight { get; set; }

        /// <summary>
        /// Gets or sets the whether the additional weight should be applied.
        /// </summary>
        bool ApplyAdditionalWeight { get; set; }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        double DimsLength { get; set; }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        double DimsWidth { get; set; }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        double DimsHeight { get; set; }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        long DimsProfileID { get; set; }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        PackageTypeBinding PackagingType { get; set; }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        IInsuranceChoice InsuranceChoice { get; set; }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        string HashCode();
    }
}
