﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Repository for excluded packages
    /// </summary>
    public class ExcludedPackageTypeRepository : IExcludedPackageTypeRepository
    {
        /// <summary>
        /// Saves the list of excluded Package types.
        /// </summary>
        public void Save(ShipmentTypeCode shipmentType, IEnumerable<int> excludedPackageTypes)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(ExcludedPackageTypeFields.ShipmentType == (int) shipmentType);

            EntityCollection<ExcludedPackageTypeEntity> collection = excludedPackageTypes
                .Select(x => new ExcludedPackageTypeEntity { ShipmentType = (int) shipmentType, PackageType = x })
                .ToEntityCollection();

            using (collection)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntitiesDirectly(typeof(ExcludedPackageTypeEntity), bucket);

                    adapter.SaveEntityCollection(collection);
                }
            }
        }

        /// <summary>
        /// Gets the excluded Package types for the given shipment type.
        /// </summary>
        public List<ExcludedPackageTypeEntity> GetExcludedPackageTypes(ShipmentType shipmentType)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentType, "shipmentType");
            RelationPredicateBucket bucket = new RelationPredicateBucket(ExcludedPackageTypeFields.ShipmentType == (int) shipmentType.ShipmentTypeCode);

            using (ExcludedPackageTypeCollection excludedPackages = new ExcludedPackageTypeCollection())
            {
                using (SqlAdapter adapter = SqlAdapter.Default)
                {
                    adapter.FetchEntityCollection(excludedPackages, bucket);
                    return excludedPackages.ToList();
                }
            }
        }
    }
}