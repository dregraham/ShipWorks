using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;
using System.Reflection;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Implementation of the IPackageAdapter interface intended to be used for shuffling package data between classes.
    /// </summary>
    public class UpsPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipmentEntity;
        private readonly UpsPackageEntity packageEntity;
        private PackageTypeBinding packagingType;
        private int index;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private readonly PropertyChangedHandler handler;
        private IInsuranceChoice insuranceChoice;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPackageAdapter" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="packageEntity">The package entity.</param>
        /// <param name="packageIndex">The index of this package adapter in a list of package adapters.</param>
        public UpsPackageAdapter(ShipmentEntity shipmentEntity, UpsPackageEntity packageEntity, int packageIndex)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            this.shipmentEntity = shipmentEntity;
            this.packageEntity = packageEntity;
            this.Index = packageIndex;
            this.insuranceChoice = new InsuranceChoice(shipmentEntity, packageEntity, packageEntity, packageEntity);

            packagingType = new PackageTypeBinding()
            {
                PackageTypeID = packageEntity.PackagingType,
                Name = EnumHelper.GetDescription((UpsPackagingType)packageEntity.PackagingType)
            };
        }

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
        public double Weight
        {
            get
            {
                // The shipment's content weight is updated when one of the customs items' quantity or weight changes 
                // when there is a only single package. When there are multiple packages, the weight differences are 
                // distributed evenly across packages, so the content weight does not have to be modified via the package adapter.
                return shipmentEntity.Ups.Packages.Count == 1 ? shipmentEntity.ContentWeight : packageEntity.Weight;
            }
            set
            {
                if (shipmentEntity.Ups.Packages.Count == 1)
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
                    handler.Set(nameof(PackagingType), v => packageEntity.PackagingType = packagingType.PackageTypeID, packageEntity.PackagingType, packagingType.PackageTypeID);
                }
            }
        }

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsLength
        {
            get { return packageEntity.DimsLength; }
            set
            {
                handler.Set(nameof(DimsLength), v => packageEntity.DimsLength = value, packageEntity.DimsLength, value);
            }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
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
