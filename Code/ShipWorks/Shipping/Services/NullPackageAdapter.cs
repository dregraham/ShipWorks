﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// A package adapter that amounts to an implementation of the null object
    /// pattern. 
    /// </summary>
    public class NullPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        public int Index
        {
            get { return 1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return 0; }
            set{}
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return 0; }
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
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Length, Width, Height, Weight, AdditionalWeight, ApplyAdditionalWeight);

            return stringHash.Hash(rawValue, string.Empty);
        }
    }
}
