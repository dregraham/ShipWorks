using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Class for managing ServiceStatusEntities
    /// </summary>
    public static class ServiceStatusManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceStatusManager));
        private static TableSynchronizer<ServiceStatusEntity> tableSynchronizer;
        private static bool needCheckForChanges = false;
        private const string ormConcurrencyExceptionMessage =
            "Another user has recently made changes.\n\n" +
            "Your changes cannot be saved since they would overwrite the other changes.";

        /// <summary>
        /// Returns the TimeSpan for amount a time allowed before a service is considered "not running"
        /// </summary>
        public static TimeSpan NotRunningTimeSpan
        {
            get
            {
                return TimeSpan.FromMinutes(10);
            }
        }

        /// <summary>
        /// Returns the TimeSpan for how often a service should check in.
        /// </summary>
        public static TimeSpan CheckInTimeSpan
        {
            get
            {
                return TimeSpan.FromMinutes(1);
            }
        }

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            ComputerManager.InitializeForCurrentSession();

            // Add any missing computers 
            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "Attempting to SaveServiceStatus.");
            sqlAdapterRetry.ExecuteWithRetry(AddMissingComputers);

            tableSynchronizer = new TableSynchronizer<ServiceStatusEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (tableSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (tableSynchronizer)
            {
                if (tableSynchronizer.Synchronize())
                {
                    tableSynchronizer.EntityCollection.Sort((int) ServiceStatusFieldIndex.ServiceDisplayName, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Adds any computers to the ServiceStatus table if they are missing from it.
        /// </summary>
        private static void AddMissingComputers()
        {
            var serviceTypeValues = EnumHelper.GetEnumList<ShipWorksServiceType>().Select(e => (int) e.Value).ToArray();
            List<ComputerEntity> computers = ComputerManager.Computers;

            foreach (ComputerEntity computer in computers)
            {
                computer.ServiceStatus.Clear();
                computer.ServiceStatus.AddRange(DataProvider.GetRelatedEntities(computer.ComputerID, EntityType.ServiceStatusEntity).Cast<ServiceStatusEntity>());
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (ComputerEntity computer in computers)
                {
                    // For each ShipWorksServiceType value, if the computer does have an entry for it, add it.
                    var missingServiceTypes = serviceTypeValues.Except(computer.ServiceStatus.Select(x => x.ServiceType));

                    foreach (int serviceType in missingServiceTypes)
                    {
                        ServiceStatusEntity serviceStatus = new ServiceStatusEntity
                        {
                            ServiceType = serviceType,
                            ServiceFullName = string.Empty,
                            ServiceDisplayName = string.Empty,
                            ComputerID = computer.ComputerID
                        };

                        SaveServiceStatus(adapter, serviceStatus);
                    }
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Get the current list of all ServiceStatusEntities
        /// </summary>
        public static IList<ServiceStatusEntity> ServicesStatuses
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(tableSynchronizer.EntityCollection);
                }
            }
        }

        /// <summary>
        /// Get the ServiceStatusEntity with the given ID, or null of no such entity exists.
        /// </summary>
        public static ServiceStatusEntity GetServiceStatus(long serviceStatusID)
        {
            return ServicesStatuses.SingleOrDefault(a => a.ServiceStatusID == serviceStatusID);
        }

        /// <summary>
        /// Get the ServiceStatusEntity with the given Computer ID and ShipWorksServiceType, or null of no such entity exists.
        /// </summary>
        public static ServiceStatusEntity GetServiceStatus(long computerID, ShipWorksServiceType serviceType)
        {
            return ServicesStatuses.SingleOrDefault(a => a.ComputerID == computerID && a.ServiceType == (int) serviceType);
        }

        /// <summary>
        /// Saves the given ServiceStatusEntity. 
        /// </summary>
        private static void SaveServiceStatus(SqlAdapter adapter, ServiceStatusEntity serviceStatus)
        {
            try
            {
                adapter.SaveAndRefetch(serviceStatus);
            }
            catch (ORMConcurrencyException ex)
            {
                // Just eat it - if someone else just updated us, that's no big deal.  
                log.Error(ormConcurrencyExceptionMessage, ex);

                // Just fetch the most current
                SqlAdapter.Default.FetchEntity(serviceStatus);
            }
        }

        /// <summary>
        /// Update and persist the last check-in for the service
        /// </summary>
        /// <param name="serviceStatus">The service for which to check-in.</param>
        public static void CheckIn(ServiceStatusEntity serviceStatus)
        {
            try
            {
                CheckInInternal(serviceStatus);
            }
            catch (ORMEntityOutOfSyncException ex)
            {
                log.Warn("ServiceStatusEntity was out of sync when checking in.  Trying again.", ex);

                // Refresh the status and try to run the check in again
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntity(serviceStatus);
                }

                CheckInInternal(serviceStatus);
            }

            SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "Attempting to SaveServiceStatus.");
            sqlAdapterRetry.ExecuteWithRetry(() => SaveServiceStatus(SqlAdapter.Default, serviceStatus));
        }

        /// <summary>
        /// Perform the actual check in and logging here so it can be retried if necessary
        /// </summary>
        private static void CheckInInternal(ServiceStatusEntity serviceStatus)
        {
            serviceStatus.LastCheckInDateTime = DateTime.UtcNow;

            log.InfoFormat("Service '{0}' checking in at {1}", serviceStatus.ServiceDisplayName, serviceStatus.LastCheckInDateTime);
        }

        /// <summary>
        /// Gets a list of the minimum required computers that need to have ShipWorks running 
        /// as a service based on the actions currently configured in the system.
        /// </summary>
        /// <returns>A list of ServiceStatusEntity objects representing the computers that
        /// need to have the ShipWorks service running.</returns>
        public static List<ServiceStatusEntity> GetComputersRequiringShipWorksService()
        {
            List<ActionEntity> allScheduledActions = ActionManager.Actions.Where(a => a.TriggerType == (int) ActionTriggerType.Scheduled).ToList();
            List<ServiceStatusEntity> allServices = new List<ServiceStatusEntity>(ServiceStatusManager.ServicesStatuses);

            List<long> requiredComputerIDs = new List<long>();

            if (allScheduledActions.Any())
            {
                // Find the list of required Computers for actions that specify a named list of computers
                foreach (ActionEntity action in allScheduledActions.Where(a => a.Enabled && a.ComputerLimitedType == (int) ComputerLimitedType.List))
                {
                    if (allServices.Where(s => action.ComputerLimitedList.Contains(s.ComputerID)).All(s => s.GetStatus() != ServiceStatus.Running))
                    {
                        // Only add computers if none of the named computers are running the service
                        requiredComputerIDs.AddRange(action.ComputerLimitedList);
                    }
                }

                if (allScheduledActions.Any(a => a.Enabled && a.ComputerLimitedType == (int) ComputerLimitedType.None) && allServices.All(s => s.GetStatus() != ServiceStatus.Running))
                {
                    // When there are scheduled action configured to run on any computer, but there aren't any computers 
                    // running the service, only add the user's computer to the list
                    requiredComputerIDs.Add(UserSession.Computer.ComputerID);
                }
            }

            allServices.ForEach(s => s.Computer = ComputerManager.Computers.FirstOrDefault(c => c.ComputerID == s.ComputerID));
            return allServices.Where(s => requiredComputerIDs.Contains(s.ComputerID)).ToList();
        }
    }
}
