﻿using System;
using System.Data.Common;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data
{
    /// <summary>
    /// Configuration data interface
    /// </summary>
    public interface IConfigurationData
    {
        /// <summary>
        /// Update the configuration entity
        /// </summary>
        void UpdateConfiguration(Action<ConfigurationEntity> setConfiguration);

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        IConfigurationEntity FetchReadOnly();

        /// <summary>
        /// Get the customer key
        /// </summary>
        string FetchCustomerKey(CustomerLicenseKeyType licenseKeyType);

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        void CheckForChangesNeeded();

        /// <summary>
        /// Gets the ActionQueueType for this process based on if it's background or not
        /// </summary>
        ActionQueueType ExecutionModeActionQueueType { get; }

        /// <summary>
        /// Should UI actions be included.  If the UI isn't running somewhere,
        /// and we are the background process, go ahead and do UI actions too since it's not open
        /// </summary>
        bool IncludeUserInterfaceActions { get; }

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        bool IsArchive(DbConnection connection);

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        bool IsArchive();

        /// <summary>
        /// Get the next update window after the given date
        /// </summary>
        DateTime GetNextUpdateWindow(DateTime date);
    }
}