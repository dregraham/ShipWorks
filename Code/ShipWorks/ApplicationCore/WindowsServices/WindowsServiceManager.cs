﻿using System;
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
using log4net;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Class for managing WindowsServiceEntities
    /// </summary>
    public static class WindowsServiceManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceManager));

        static TableSynchronizer<WindowsServiceEntity> tableSynchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            tableSynchronizer = new TableSynchronizer<WindowsServiceEntity>();
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
                    tableSynchronizer.EntityCollection.Sort((int)WindowsServiceFieldIndex.ServiceDisplayName, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the current list of all WindowsServiceEntities
        /// </summary>
        public static IList<WindowsServiceEntity> WindowsServices
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
        /// Get the WindowsServiceEntity with the given ID, or null of no such action exists.
        /// </summary>
        public static WindowsServiceEntity GetWindowsService(long windowsServiceID)
        {
            return WindowsServices.SingleOrDefault(a => a.WindowsServiceID == windowsServiceID);
        }

        /// <summary>
        /// Delete the given WindowsServiceEntity
        /// </summary>
        public static void DeleteWindowsService(WindowsServiceEntity windowsService)
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
        /// Saves the given WindowsServiceEntity. 
        /// </summary>
        public static void SaveWindowsService(WindowsServiceEntity windowsService, SqlAdapter adapter)
        {
            try
            {
                // Save and refetch.
                adapter.SaveAndRefetch(windowsService);
            }
            catch (ORMConcurrencyException ex)
            {
                string errorMessage = "Another user has recently made changes.\n\n" +
                                      "Your changes cannot be saved since they would overwrite the other changes.";
                log.Error(errorMessage, ex);

                throw new WindowsServiceConcurrencyException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Saves the given WindowsServiceEntity. 
        /// </summary>
        public static void SaveWindowsService(WindowsServiceEntity windowsService)
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
                string errorMessage = "Another user has recently made changes.\n\n" +
                                      "Your changes cannot be saved since they would overwrite the other changes.";
                log.Error(errorMessage, ex);

                throw new WindowsServiceConcurrencyException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Update and persist the last check-in for the WindowsService
        /// </summary>
        /// <param name="windowsService">The WindowsService for which to check-in.</param>
        public static void CheckIn(WindowsServiceEntity windowsService)
        {
            windowsService.LastCheckInDateTime = DateTime.UtcNow;

            log.InfoFormat("Windows Service '{0}' checking in at {1}", windowsService.ServiceDisplayName, windowsService.LastCheckInDateTime);

            SaveWindowsService(windowsService);
        }
    }
}
