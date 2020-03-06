﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for loading shipment data
    /// </summary>
    public static class ShipmentTypeDataService
    {
        static Dictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// Load an existing shipment data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed. "shipment" and "parent" could be different in the case of derived ShipmentTypes, like for USPS
        /// where "parent" is the PostalShipment, not the actual Shipment
        /// </summary>
        [NDependIgnoreTooManyParams]
        public static void LoadShipmentData(ShipmentType shipmentType, ShipmentEntity shipment, EntityBase2 parent, string childProperty, Type entityType, bool refreshIfPresent)
        {
            PropertyInfo property = GetChildProperty(parent, childProperty);

            // See if the profile data is already present
            if (refreshIfPresent)
            {
                property.SetValue(parent, null, null);
            }

            // If the specific shipment isn't there we have to fetch it
            if (property.GetValue(parent, null) == null)
            {
                EntityBase2 childEntity;

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ISqlAdapterFactory adapterFactory = lifetimeScope.Resolve<ISqlAdapterFactory>();

                    adapterFactory.WithPhysicalTransaction(adapter =>
                    {
                        // Try to fetch the existing profile data for the shipment
                        if (parent.Fields.State != EntityState.New)
                        {
                            childEntity = (EntityBase2) Activator.CreateInstance(entityType, parent.Fields["ShipmentID"].CurrentValue);
                            adapter.FetchEntity(childEntity);
                        }
                        // If the parent is new, just create a new child.
                        else
                        {
                            childEntity = (EntityBase2) Activator.CreateInstance(entityType);
                        }

                        // Apply the reference
                        property.SetValue(parent, childEntity, null);

                        // If it didn't exist, then we have to create to save it as new
                        if (childEntity.Fields.State != EntityState.Fetched)
                        {
                            // Reset the object to new and apply
                            childEntity.Fields.State = EntityState.New;

                            // Configure the newly created shipment
                            shipmentType.ConfigureNewShipment(shipment);

                            // Save the new entity.  Remove the reference to the parent first that it doesn't save recursively up to the shipment, only down to child packages
                            property.SetValue(parent, null, null);
                            adapter.SaveAndRefetch(childEntity);
                            property.SetValue(parent, childEntity, null);
                        }

                        adapter.Commit();
                    });
                }
            }
        }

        /// <summary>
        /// Load insurance data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed.
        /// </summary>
        public static void LoadInsuranceData(ShipmentEntity shipment)
        {
            InsurancePolicyEntity insurancePolicyEntity = new InsurancePolicyEntity(shipment.ShipmentID);

            SqlAdapter.Default.FetchEntity(insurancePolicyEntity);

            if (insurancePolicyEntity.Fields.State == EntityState.Fetched)
            {
                shipment.InsurancePolicy = insurancePolicyEntity;
            }
        }

        /// <summary>
        /// Get the reflected property represented the given property name
        /// </summary>
        private static PropertyInfo GetChildProperty(EntityBase2 parent, string childProperty)
        {
            Type type = parent.GetType();
            string identifier = type.FullName + "." + childProperty;

            PropertyInfo property;
            if (!propertyMap.TryGetValue(identifier, out property))
            {
                property = type.GetProperty(childProperty);

                propertyMap[identifier] = property;
            }

            return property;
        }

    }
}
