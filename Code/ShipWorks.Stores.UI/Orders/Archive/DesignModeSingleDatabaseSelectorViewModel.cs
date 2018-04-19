using System;
using System.Collections.Generic;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// Design-time view model for the SingleDatabaseSelector dialog
    /// </summary>
    public class DesignModeSingleDatabaseSelectorViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeSingleDatabaseSelectorViewModel()
        {
            Databases = new[]
            {
                new DesignModeSqlDatabaseDetail("ShipWorks Database 1", "John Doe", new DateTime(2018, 3, 6), "1003", new DateTime(2018, 2, 6)),
                new DesignModeSqlDatabaseDetail("Foo Database 2", "", new DateTime(2018, 3, 6), "", new DateTime(2018, 2, 6)),
                new DesignModeSqlDatabaseDetail("ShipWorks Database 1", "Jane Doe", new DateTime(2018, 3, 6), "1003", new DateTime(2018, 2, 6)),
            };
        }

        /// <summary>
        /// List of databases
        /// </summary>
        public IEnumerable<ISqlDatabaseDetail> Databases { get; }
    }

    /// <summary>
    /// Design mode version of the Sql Database detail
    /// </summary>
    public class DesignModeSqlDatabaseDetail : ISqlDatabaseDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeSqlDatabaseDetail(string name, string lastUsedBy, DateTime lastUsedOn, string lastOrderNumber, DateTime lastOrderDate)
        {
            Name = name;
            LastUsedBy = lastUsedBy;
            LastUsedOn = lastUsedOn;
            LastOrderNumber = lastOrderNumber;
            LastOrderDate = lastOrderDate;
        }

        public Guid Guid => Guid.NewGuid();

        public bool IsArchive => false;

        public DateTime LastOrderDate { get; }

        public string LastOrderNumber { get; }

        public string LastUsedBy { get; }

        public DateTime LastUsedOn { get; }

        public string Name { get; }

        public Version SchemaVersion { get; }

        public SqlDatabaseStatus Status { get; }

        public int OrderCount { get; }

        public DateTime? OldestOrderDate { get; }

        public DateTime? NewestOrderDate { get; }
    }
}
