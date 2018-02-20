using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Repository for saving and loading profile data
    /// </summary>
    public class ShippingProfileLoader : IShippingProfileLoader
    {
        private readonly Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileLoader(ISqlAdapterFactory sqlAdapterFactory, IShipmentTypeManager shipmentTypeManager)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }
        
        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                // If this is the first time loading it, or we are supposed to refresh, do it now
                if (!profile.IsNew && refreshIfPresent)
                {
                    LoadPackages(profile, sqlAdapter);
                }

                if (profile.Packages.None() &&
                    (profile.ShipmentType == null || !shipmentTypeManager.Get(profile.ShipmentTypeCode).SupportsMultiplePackages))
                {
                    profile.Packages.Add(new PackageProfileEntity());
                }

                if (profile.ShipmentType != null && profile.ShipmentTypeCode != ShipmentTypeCode.None)
                {
                    LoadChildProfiles(profile, refreshIfPresent, sqlAdapter);
                }
            }
        }

        /// <summary>
        /// Load the profiles children
        /// </summary>
        private void LoadChildProfiles(ShippingProfileEntity profile, bool refreshIfPresent, ISqlAdapter sqlAdapter)
        {
            (string propertyName, Type type) = GetChildPropertyNameAndType(profile.ShipmentTypeCode);
            LoadProfileData(profile, propertyName, type, refreshIfPresent, sqlAdapter);

            if (PostalUtility.IsPostalShipmentType(profile.ShipmentTypeCode) && profile.ShipmentTypeCode != ShipmentTypeCode.PostalWebTools)
            {
                (string postalChildPropertyName, Type postalChildType) = GetPostalChildPropertyNameAndType(profile.ShipmentTypeCode);
                LoadProfileData(profile.Postal, postalChildPropertyName, postalChildType, refreshIfPresent, sqlAdapter);
            }
        }

        /// <summary>
        /// Load the profiles packages
        /// </summary>
        private void LoadPackages(ShippingProfileEntity profile, ISqlAdapter sqlAdapter)
        {
            profile.Packages.Clear();
            
            sqlAdapter.FetchEntityCollection(profile.Packages,
                new RelationPredicateBucket(PackageProfileFields.ShippingProfileID == profile.ShippingProfileID));
            profile.Packages.Sort((int) PackageProfileFieldIndex.PackageProfileID, ListSortDirection.Ascending);
        }

        /// <summary>
        /// Get the profiles child property name and type for the ShipmentTypeCode
        /// </summary>
        private static (string, Type) GetPostalChildPropertyNameAndType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Endicia:
                    return ("Endicia", typeof(EndiciaProfileEntity));                
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return ("Usps", typeof(UspsProfileEntity));
                default:
                    throw new InvalidOperationException("Unknown child property for ShipmentTypeCode");
            }
        }

        /// <summary>
        /// Get the profiles child property name and type for the ShipmentTypeCode
        /// </summary>
        private static (string, Type) GetChildPropertyNameAndType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return ("Ups", typeof(UpsProfileEntity));
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return ("Postal", typeof(PostalProfileEntity));
                case ShipmentTypeCode.FedEx:
                    return ("FedEx", typeof(FedExProfileEntity));
                case ShipmentTypeCode.OnTrac:
                    return ("OnTrac", typeof(OnTracProfileEntity));
                case ShipmentTypeCode.iParcel:
                    return ("IParcel", typeof(IParcelProfileEntity));
                case ShipmentTypeCode.Other:
                    return ("Other", typeof(OtherProfileEntity));
                case ShipmentTypeCode.BestRate:
                    return ("BestRate", typeof(BestRateProfileEntity));
                case ShipmentTypeCode.Amazon:
                    return ("Amazon", typeof(AmazonProfileEntity));
                case ShipmentTypeCode.DhlExpress:
                    return ("DhlExpress", typeof(DhlExpressProfileEntity));
                case ShipmentTypeCode.Asendia:
                    return ("Asendia", typeof(AsendiaProfileEntity));
                default:
                    throw new InvalidOperationException($"Unknown child property for ShipmentTypeCode {shipmentTypeCode}");
            }
        }

        /// <summary>
        /// Load an existing profile data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed.
        /// </summary>
        private void LoadProfileData(EntityBase2 parent, string childProperty, Type profileType, bool refreshIfPresent, ISqlAdapter sqlAdapter)
        {
            PropertyInfo property = GetChildProperty(parent, childProperty);

            // See if the profile data is already present
            if (refreshIfPresent)
            {
                property.SetValue(parent, null, null);
            }

            // If the profile isn't there we have to fetch it
            if (property.GetValue(parent, null) == null)
            {
                EntityBase2 childEntity;

                // Try to fetch the existing profile data for the shipment
                if (parent.Fields.State != EntityState.New)
                {
                    childEntity = (EntityBase2) Activator.CreateInstance(profileType, parent.Fields["ShippingProfileID"].CurrentValue);
                    sqlAdapter.FetchEntity(childEntity);
                }
                // If the parent is new, just create a new child.
                else
                {
                    childEntity = (EntityBase2) Activator.CreateInstance(profileType);
                }

                // Apply the reference
                property.SetValue(parent, childEntity, null);

                // If it doesn't exist, then we have to create to save it as new
                if (childEntity.Fields.State != EntityState.Fetched)
                {
                    // Reset the object to new and apply
                    childEntity.Fields.State = EntityState.New;
                }
            }
        }

        /// <summary>
        /// Get the reflected property represented the given property name
        /// </summary>
        private PropertyInfo GetChildProperty(EntityBase2 parent, string childProperty)
        {
            Type type = parent.GetType();
            string identifier = type.FullName + "." + childProperty;

            if (!propertyMap.TryGetValue(identifier, out PropertyInfo property))
            {
                property = type.GetProperty(childProperty);

                propertyMap[identifier] = property;
            }

            return property;
        }
    }
}
