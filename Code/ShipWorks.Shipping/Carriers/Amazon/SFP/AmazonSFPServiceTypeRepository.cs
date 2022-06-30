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
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Repository for AmazonServiceType
    /// </summary>
    [Component(SingleInstance = true)]
    public class AmazonSFPServiceTypeRepository : IAmazonSFPServiceTypeRepository
    {
        private readonly object lockObject = new object();
        private readonly ILog log;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private List<AmazonSFPServiceTypeEntity> serviceTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPServiceTypeRepository(ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> logFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = logFactory(typeof(AmazonSFPServiceTypeRepository));
        }

        /// <summary>
        /// Get the list of AmazonServiceTypes
        /// </summary>
        public List<AmazonSFPServiceTypeEntity> Get()
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
        public void SaveNewService(string apiValue, string description)
        {
            // if for some reason the api value is greater than what we can store in the database ignore it
            if (apiValue.Length > AmazonSFPServiceTypeFields.ApiValue.MaxLength)
            {
                return;
            }

            lock (lockObject)
            {
                if (serviceTypes == null)
                {
                    RefreshServiceTypes();
                }

                // We don't know about this type, create a new one and attempt to save it.
                AmazonSFPServiceTypeEntity newType = new AmazonSFPServiceTypeEntity
                {
                    ApiValue = apiValue,
                    //keep the descripton truncated to whatever length we support in the database
                    Description = CleanupServiceName(description.Truncate(AmazonSFPServiceTypeFields.Description.MaxLength))
                };

                try
                {
                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                    {
                        sqlAdapter.SaveAndRefetch(newType);
                        serviceTypes.Add(newType);
                    }
                }
                catch (ORMQueryExecutionException ex)
                {
                    // Saving failed. Refresh from database to see if the service is already in the DB.
                    // If not, throw.
                    log.Error($"Error inserting into AmazonServiceTypeTable with apiValue {apiValue}", ex);

                    RefreshServiceTypes();
                    newType = serviceTypes.FirstOrDefault(service => service.ApiValue == apiValue);
                    if (newType == null)
                    {
                        log.Error($"apiValue not found after refresh. Rethrowing.");
                        throw;
                    }
                    log.Info($"apiValue found already in database. Using this new value.");
                }
            }
        }

        /// <summary>
        /// Clean up the description for service types with known issues
        /// </summary>
        private string CleanupServiceName(string serviceFromAmazon)
        {
            return serviceFromAmazon == "ONTRAC_MFN_GROUND" ? "OnTrac Ground" : serviceFromAmazon;
        }

        /// <summary>
        /// Refetch service types from the database
        /// </summary>
        private void RefreshServiceTypes()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<AmazonSFPServiceTypeEntity> query = new QueryFactory().AmazonSFPServiceType;

                IEntityCollection2 types = sqlAdapter.FetchQueryAsync(query).Result;
                serviceTypes = types.OfType<AmazonSFPServiceTypeEntity>().ToList();

                serviceTypes.Insert(0, new AmazonSFPServiceTypeEntity()
                {
                    AmazonSFPServiceTypeID = 0,
                    ApiValue = string.Empty,
                    Description = "Least Expensive Rate"
                });
            }
        }

        /// <summary>
        /// Get carrier name
        /// </summary>
        public string GetCarrierName(AmazonSFPServiceTypeEntity amazonSfpServiceType)
        {
            var parts = amazonSfpServiceType.ApiValue.Split('_');

            if (parts.Any())
            {
                return parts[0];
            }

            return amazonSfpServiceType.ApiValue;
        }

        /// <summary>
        /// Get carrier name
        /// </summary>
        public string GetCarrierName(string id)
        {
            var amazonSfpServiceType = Find(id);

            return GetCarrierName(amazonSfpServiceType);
        }

        /// <summary>
        /// Get carrier name
        /// </summary>
        public AmazonSFPServiceTypeEntity Find(string valueToSearch)
        {
            var toCheck = Get();

            var result = toCheck
                .FirstOrDefault(t => t.PlatformApiCode?.Equals(valueToSearch, StringComparison.InvariantCultureIgnoreCase) == true ||
                                     t.ApiValue?.Equals(valueToSearch, StringComparison.InvariantCultureIgnoreCase) == true);

            return result;
        }
    }
}