using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Transactions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Users;
using log4net;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Class for managing WindowsServiceEntities
    /// </summary>
    public static class WindowsServiceManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceManager));

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
        /// Adds any computers to the WindowsService table if they are missing from it.
        /// </summary>
        private static void AddMissingComputers()
        {
            int numberOfServiceTypes = Enum.GetNames(typeof(ShipWorksServiceType)).Length;

            // Find computers that are missing WindowsService entries
            foreach (ComputerEntity computer in ComputerManager.Computers.Where(c => c.ServiceStatuses == null || c.ServiceStatuses.Count != numberOfServiceTypes))
            {
                // For each ShipWorksServiceType, if the computer does have an entry for it, add it.
                foreach (ShipWorksServiceType serviceType in Enum.GetValues(typeof(ShipWorksServiceType)))
                {
                    if (computer.ServiceStatuses == null ||
                        computer.ServiceStatuses.Count == 0 ||
                        computer.ServiceStatuses.All(ws => ws.ServiceType != (int)serviceType))
                    {
                        ServiceStatusEntity windowsService = new ServiceStatusEntity
                            {
                                ServiceType = (int) serviceType,
                                ServiceFullName = string.Empty,
                                ServiceDisplayName = string.Empty,
                                ComputerID = computer.ComputerID
                            };

                        SaveWindowsService(windowsService);
                    }
                }
            }
        }

        /// <summary>
        /// Get the current list of all WindowsServiceEntities
        /// </summary>
        public static IList<ServiceStatusEntity> WindowsServices
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
        /// Get the ServiceStatusEntity with the given ID, or null of no such action exists.
        /// </summary>
        public static ServiceStatusEntity GetWindowsService(long windowsServiceID)
        {
            return WindowsServices.SingleOrDefault(a => a.ServiceStatusID == windowsServiceID);
        }

        /// <summary>
        /// Get the ServiceStatusEntity with the given Computer ID and ShipWorksServiceType, or null of no such action exists.
        /// </summary>
        public static ServiceStatusEntity GetWindowsService(long computerID, ShipWorksServiceType shipWorksServiceType)
        {
            return WindowsServices.SingleOrDefault(a => a.ComputerID == computerID && a.ServiceType == (int)shipWorksServiceType);
        }

        /// <summary>
        /// Delete the given ServiceStatusEntity
        /// </summary>
        public static void DeleteWindowsService(ServiceStatusEntity windowsService)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Finally delete the action entity itself
                adapter.DeleteEntity(windowsService);

                // Commit transaction
                adapter.Commit();
            }
        }

        /// <summary>
        /// Saves the given ServiceStatusEntity. 
        /// </summary>
        public static void SaveWindowsService(ServiceStatusEntity windowsService, SqlAdapter adapter)
        {
            try
            {
                // Save and refetch.
                adapter.SaveAndRefetch(windowsService);
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
        public static void SaveWindowsService(ServiceStatusEntity windowsService)
        {
            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Save and refetch.
                    SaveWindowsService(windowsService, adapter);

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
        /// Update and persist the last check-in for the WindowsService
        /// </summary>
        /// <param name="windowsService">The WindowsService for which to check-in.</param>
        public static void CheckIn(ServiceStatusEntity windowsService)
        {
            windowsService.LastCheckInDateTime = DateTime.UtcNow;

            log.InfoFormat("Windows Service '{0}' checking in at {1}", windowsService.ServiceDisplayName, windowsService.LastCheckInDateTime);

            SaveWindowsService(windowsService);
        }
    }
}
