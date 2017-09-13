using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Repository for AmazonServiceType
    /// </summary>
    [Component(SingleInstance = true)]
    public class AmazonServiceTypeRepository : IAmazonServiceTypeRepository
    {
        private readonly object lockObject = new object();
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private List<AmazonServiceTypeEntity> serviceTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceTypeRepository(ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> logFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = logFactory(typeof(IAmazonServiceTypeRepository));
        }

        /// <summary>
        /// Get the list of AmazonServiceTypes
        /// </summary>
        public List<AmazonServiceTypeEntity> Get()
        {
            lock (lockObject)
            {
                if (serviceTypes == null)
                {
                    RefreshServiceTypes();
                }

                return serviceTypes;
            }
        }

        /// <summary>
        /// Creates a new service type
        /// </summary>
        public AmazonServiceTypeEntity CreateNewService(string apiValue, string description)
        {
            lock (lockObject)
            {
                // We don't know about this type, create a new one and attempt to save it.
                AmazonServiceTypeEntity newType = new AmazonServiceTypeEntity
                {
                    ApiValue = apiValue,
                    Description = description
                };

                try
                {
                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                    {
                        sqlAdapter.SaveAndRefetch(newType);
                        return newType;
                    }
                }
                catch (ORMQueryExecutionException ex)
                {
                    // Saving failed. Refresh from database to see if the service is already in the DB. 
                    // If not, throw.
                    log.Error($"Error inserting into AmazonServiceTypeTable with apiValue {apiValue}", ex);

                    RefreshServiceTypes();
                    newType = serviceTypes.FirstOrDefault(service => service.ApiValue == apiValue);
                    if (newType != null)
                    {
                        log.Info($"apiValue found already in database. Using this new value.");
                        return newType;
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Refetch service types from the database
        /// </summary>
        private void RefreshServiceTypes()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<AmazonServiceTypeEntity> query = new QueryFactory().AmazonServiceType;

                IEntityCollection2 types = sqlAdapter.FetchQueryAsync(query).Result;
                serviceTypes = types.OfType<AmazonServiceTypeEntity>().ToList();
            }
        }
    }
}