using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Settings
{
    public class ExcludedServiceTypeRepository : IExcludedServiceTypeRepository
    {
        /// <summary>
        /// Saves the list of excluded service types.
        /// </summary>
        public void Save(List<ExcludedServiceTypeEntity> excludedServiceTypes)
        {
            // This will be a wipe and replace strategy based on the shipment types in the list
            // excludes service types

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the excluded service types for the given shipment type.
        /// </summary>
        public List<ExcludedServiceTypeEntity> GetExcludedServiceTypes(ShipmentType shipmentType)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(ExcludedServiceTypeFields.ShipmentType == (int) shipmentType.ShipmentTypeCode);

            using (ExcludedServiceTypeCollection excludedServices = new ExcludedServiceTypeCollection())
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                adapter.FetchEntityCollection(excludedServices, bucket);
                return excludedServices.ToList();
            }
        }
    }
}
