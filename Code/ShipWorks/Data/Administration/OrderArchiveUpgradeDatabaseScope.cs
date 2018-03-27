using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Extensions;
using ShipWorks.ApplicationCore;
using ShipWorks.Archiving;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Extensions;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Scope used for disabling archive "read only" triggers and re-enabling them on completion.
    /// </summary>
    public class OrderArchiveUpgradeDatabaseScope : IDisposable
    {
        private readonly bool isArchive = false;
        private readonly ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope();
        private readonly IOrderArchiveDataAccess orderArchiveDataAccess;
        private DbConnection connection;

        /// <summary>
        /// Disable order archiving triggers.
        /// </summary>
        public OrderArchiveUpgradeDatabaseScope(DbConnection connection)
        {
            this.connection = connection;
            isArchive = ConfigurationData.IsArchive(connection);

            if (isArchive)
            {
                orderArchiveDataAccess = lifetimeScope.Resolve<IOrderArchiveDataAccess>();
                orderArchiveDataAccess.DisableArchiveTriggers(connection);
            }
        }

        /// <summary>
        /// Add back archiving triggers if the database is an archive 
        /// </summary>
        public void Dispose()
        {
            if (isArchive)
            {
                connection = connection?.State == ConnectionState.Open ? connection : SqlSession.Current.OpenConnection();
                orderArchiveDataAccess.EnableArchiveTriggers(connection);
            }

            lifetimeScope?.Dispose();
        }
    }
}
