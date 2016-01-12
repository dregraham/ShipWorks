using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// A package adapter that amounts to an implementation of the null object
    /// pattern. 
    /// </summary>
    public class NullPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
#pragma warning disable CS0067
        public virtual event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        public int Index
        {
            get { return 1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        public PackageTypeBinding PackagingType
        {
            get
            {
                return new PackageTypeBinding()
                {
                    PackageTypeID = 0,
                    Name = "None"
                };
            }
            set { }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        public double DimsLength
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        public double DimsWidth
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        public double DimsHeight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        public long DimsProfileID
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the insurance choice.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice
        {
            get { return null; }
            set { /*  Not relevant to a NullPackageAdapter */ }
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
                string errorMessage = InputValidation<NullPackageAdapter>.Validate(this, columnName);

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
            return InputValidation<NullPackageAdapter>.Validate(this);
        }

        #endregion
    }
}
