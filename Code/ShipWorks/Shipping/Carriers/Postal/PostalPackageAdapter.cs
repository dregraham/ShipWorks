﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class PostalPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private readonly PropertyChangedHandler handler;
        private readonly ShipmentEntity shipment;
        private PackageTypeBinding packagingType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostalPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public PostalPackageAdapter(ShipmentEntity shipment)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            this.shipment = shipment;

            packagingType = new PackageTypeBinding()
            {
                PackageTypeID = shipment.Postal.PackagingType,
                Name = EnumHelper.GetDescription((PostalPackagingType)shipment.Postal.PackagingType)
            };
        }

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index
        {
            get { return 1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
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
        public double AdditionalWeight
        {
            get { return shipment.Postal.DimsWeight; }
            set
            {
                handler.Set(nameof(AdditionalWeight), v => shipment.Postal.DimsWeight = value, shipment.Postal.DimsWeight, value, false);
            }

        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return shipment.Postal.DimsAddWeight; }
            set
            {
                handler.Set(nameof(ApplyAdditionalWeight), v => shipment.Postal.DimsAddWeight = value, shipment.Postal.DimsAddWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public PackageTypeBinding PackagingType
        {
            get
            {
                return packagingType;
            }
            set
            {
                packagingType = value;

                // value can be null when switching between shipments, so only update the underlying value
                // if we have a valid packagingType.
                if (packagingType != null)
                {
                    handler.Set(nameof(PackagingType), v => shipment.Postal.PackagingType = packagingType.PackageTypeID, shipment.Postal.PackagingType, packagingType.PackageTypeID, false);
                }
            }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsLength
        {
            get { return shipment.Postal.DimsLength; }
            set
            {
                handler.Set(nameof(DimsLength), v => shipment.Postal.DimsLength = value, shipment.Postal.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsWidth
        {
            get { return shipment.Postal.DimsWidth; }
            set
            {
                handler.Set(nameof(DimsWidth), v => shipment.Postal.DimsWidth = value, shipment.Postal.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsHeight
        {
            get { return shipment.Postal.DimsHeight; }
            set
            {
                handler.Set(nameof(DimsHeight), v => shipment.Postal.DimsHeight = value, shipment.Postal.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return shipment.Postal.DimsProfileID; }
            set
            {
                handler.Set(nameof(DimsProfileID), v => shipment.Postal.DimsProfileID = value, shipment.Postal.DimsProfileID, value, false);
            }
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
    }
}
