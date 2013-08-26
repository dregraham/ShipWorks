using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Class for managing ServiceStatusEntities
    /// </summary>
    public static class ServiceStatusManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceStatusManager));

        static TableSynchronizer<ServiceStatusEntity> tableSynchronizer;
        static bool needCheckForChanges = false;

        const string ormConcurrencyExceptionMessage = "Another user has recently made changes.\n\n" +
                              "Your changes cannot be saved since they would overwrite the other changes.";

        /// <summary>
        /// Returns the TimeSpan for amount a time allowed before a service is considered "not running"
        /// </summary>
        public static TimeSpan NotRunningTimeSpan
        {
            get
            {
                return new TimeSpan(0, 0, 10, 0);
            }
        }

        /// <summary>
        /// Returns the TimeSpan for how often a service should check in.
        /// </summary>
        public static TimeSpan CheckInTimeSpan
        {
            get
            {
                return new TimeSpan(0, 0, 1, 0);
            }
        }

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            ComputerManager.InitializeForCurrentUser();

            // Add any missing computers 
            AddMissingComputers();

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
                    tableSynchronizer.EntityCollection.Sort((int)ServiceStatusFieldIndex.ServiceDisplayName, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Adds any computers to the ServiceStatus table if they are missing from it.
        /// </summary>
        private static void AddMissingComputers()
        {
            int numberOfServiceTypes = Enum.GetNames(typeof(ShipWorksServiceType)).Length;

            // Find computers that are missing ServiceStatus entries
            foreach (ComputerEntity computer in ComputerManager.Computers.Where(c => c.ServiceStatus == null || c.ServiceStatus.Count != numberOfServiceTypes))
            {
                // For each ShipWorksServiceType, if the computer does have an entry for it, add it.
                foreach (ShipWorksServiceType serviceType in Enum.GetValues(typeof(ShipWorksServiceType)))
                {
                    if (computer.ServiceStatus == null ||
                        computer.ServiceStatus.Count == 0 ||
                        computer.ServiceStatus.All(ws => ws.ServiceType != (int)serviceType))
                    {
                        ServiceStatusEntity serviceStatus = new ServiceStatusEntity
                            {
                                ServiceType = (int) serviceType,
                                ServiceFullName = string.Empty,
                                ServiceDisplayName = string.Empty,
                                ComputerID = computer.ComputerID
                            };

                        SaveServiceStatus(serviceStatus);
                    }
                }
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
            return ServicesStatuses.SingleOrDefault(a => a.ComputerID == computerID && a.ServiceType == (int)serviceType);
        }

        /// <summary>
        /// Delete the given ServiceStatusEntity
        /// </summary>
        public static void DeleteServiceStatus(ServiceStatusEntity serviceStatus)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Finally delete the entity itself
                adapter.DeleteEntity(serviceStatus);

                // Commit transaction
                adapter.Commit();
            }
        }

        /// <summary>
        /// Saves the given ServiceStatusEntity. 
        /// </summary>
        public static void SaveServiceStatus(ServiceStatusEntity serviceStatus, SqlAdapter adapter)
        {
            try
            {
                // Save and refetch.
                adapter.SaveAndRefetch(serviceStatus);
            }
            catch (ORMConcurrencyException ex)
            {
                log.Error(ormConcurrencyExceptionMessage, ex);

                throw new ShipWorksServiceException(ormConcurrencyExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Saves the given ServiceStatusEntity. 
        /// </summary>
        public static void SaveServiceStatus(ServiceStatusEntity serviceStatus)
        {
            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Save and refetch.
                    SaveServiceStatus(serviceStatus, adapter);

                    adapter.Commit();
                }
            }
            catch (ORMConcurrencyException ex)
            {
                log.Error(ormConcurrencyExceptionMessage, ex);

                throw new ShipWorksServiceException(ormConcurrencyExceptionMessage, ex);
            }
        }

        /// <summary>
        /// Update and persist the last check-in for the service
        /// </summary>
        /// <param name="serviceStatus">The service for which to check-in.</param>
        public static void CheckIn(ServiceStatusEntity serviceStatus)
        {
            serviceStatus.LastCheckInDateTime = DateTime.UtcNow;

            log.InfoFormat("Service '{0}' checking in at {1}", serviceStatus.ServiceDisplayName, serviceStatus.LastCheckInDateTime);

            SaveServiceStatus(serviceStatus);
        }
    }
}
