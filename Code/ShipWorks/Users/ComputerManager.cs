﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using System.ComponentModel;
using ShipWorks.Data;

namespace ShipWorks.Users
{
    /// <summary>
    /// Manages the computers known to ShipWorks
    /// </summary>
    public static class ComputerManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ComputerManager));

        static TableSynchronizer<ComputerEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ComputerEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) ComputerFieldIndex.Name, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// All the known computers ShipWorks runs on
        /// </summary>
        public static List<ComputerEntity> Computers
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    List<ComputerEntity> computers = EntityUtility.CloneEntityCollection(synchronizer.EntityCollection);

                    // Load the computers' ServiceStatusEntities.
                    computers.ForEach(EnsureServiceStatusesLoaded);

                    return computers;
                }
            }
        }

        /// <summary>
        /// Ensures the ServiceStatuses for the computer exist.
        /// </summary>
        public static void EnsureServiceStatusesLoaded(ComputerEntity computer)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                computer.ServiceStatus.Clear();
                computer.ServiceStatus.AddRange(DataProvider.GetRelatedEntities(computer.ComputerID, EntityType.ServiceStatusEntity).Cast<ServiceStatusEntity>());
            }
        }

        /// <summary>
        /// Get the computer entity with the given ID, or null if it does not exist
        /// </summary>
        public static ComputerEntity GetComputer(long computerID)
        {
            return Computers.SingleOrDefault(c => c.ComputerID == computerID);
        }

        /// <summary>
        /// Gets the computer that Sql Server is currently running on.
        /// </summary>
        public static ComputerEntity GetSqlServerComputer
        {
            get
            {
                return Computers.SingleOrDefault(c => c.Name == SqlSession.Current.GetServerMachineName());   
            }
        }

        /// <summary>
        /// Ensures that this computer is loaded and registered in the database.
        /// </summary>
        public static ComputerEntity RegisterThisComputer()
        {
            if (ShipWorksSession.ComputerID == Guid.Empty)
            {
                throw new InvalidOperationException("ComputerID not loaded.");
            }

            // Load this computer by guid
            ComputerCollection computers = ComputerCollection.Fetch(SqlAdapter.Default, ComputerFields.Identifier == ShipWorksSession.ComputerID);

            // If it wasn't fetched, it does not exist.
            if (computers.Count == 0)
            {
                // Default name is the machine name
                ComputerEntity computer = new ComputerEntity();
                computer.Identifier = ShipWorksSession.ComputerID;
                computer.Name = Environment.MachineName;

                log.InfoFormat("Adding this computer to database as '{0}'.", computer.Name);
                SqlAdapter.Default.SaveAndRefetch(computer);

                return computer;
            }
            else
            {
                ComputerEntity computer = computers[0];

                log.InfoFormat("Computer already registered as '{0}'.", computer.Name);

                if (computer.Name != Environment.MachineName)
                {
                    computer.Name = Environment.MachineName;

                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(computer);
                    }
                }

                return computer;
            }
        }
    }
}
