using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Users;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Interact with the database for managing archives
    /// </summary>
    [Component]
    public class ArchiveManagerDataAccess : IArchiveManagerDataAccess
    {
        private readonly Func<ISqlSession> getSqlSession;
        private readonly IShipWorksDatabaseUtility databaseUtility;
        private readonly Func<ISingleDatabaseSelectorViewModel> singleDatabaseSelector;
        private readonly IUserLoginWorkflow loginWorkflow;
        private readonly IDatabaseIdentifier databaseIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveManagerDataAccess(
            Func<ISqlSession> getSqlSession,
            IDatabaseIdentifier databaseIdentifier,
            IShipWorksDatabaseUtility databaseUtility,
            Func<ISingleDatabaseSelectorViewModel> singleDatabaseSelector,
            IUserLoginWorkflow loginWorkflow)
        {
            this.databaseIdentifier = databaseIdentifier;
            this.loginWorkflow = loginWorkflow;
            this.getSqlSession = getSqlSession;
            this.databaseUtility = databaseUtility;
            this.singleDatabaseSelector = singleDatabaseSelector;
        }

        /// <summary>
        /// Change the database
        /// </summary>
        public bool ChangeDatabase(ISqlDatabaseDetail database)
        {
            if (!loginWorkflow.Logoff(false))
            {
                return false;
            }

            var newSession = getSqlSession().CreateCopy();
            newSession.DatabaseName = database.Name;
            newSession.SaveAsCurrent();

            loginWorkflow.Logon(null);

            return true;
        }

        /// <summary>
        /// Get a list of archive databases for the current database
        /// </summary>
        public Task<IEnumerable<ISqlDatabaseDetail>> GetArchiveDatabases() =>
            GetArchiveDatabases(databaseUtility, getSqlSession(), databaseIdentifier.Get());

        /// <summary>
        /// Get the live database
        /// </summary>
        public Task<ISqlDatabaseDetail> GetLiveDatabase() =>
            GetLiveDatabase(getSqlSession());

        /// <summary>
        /// Get the live database
        /// </summary>
        private Task<ISqlDatabaseDetail> GetLiveDatabase(ISqlSession sqlSession) =>
            Functional.UsingAsync(
                (DbConnection) sqlSession.OpenConnection(),
                con => databaseUtility
                    .GetDatabaseDetails(con)
                    .Map(x => x.Where(IsLiveDatabase(sqlSession.DatabaseName, databaseIdentifier.Get())))
                    .Map(singleDatabaseSelector().SelectSingleDatabase)
                    .Bind(EnsureDatabaseExists));

        /// <summary>
        /// Ensure that the live database was found
        /// </summary>
        private Task<ISqlDatabaseDetail> EnsureDatabaseExists(ISqlDatabaseDetail database) =>
            database != null ?
                Task.FromResult(database) :
                Task.FromException<ISqlDatabaseDetail>(new InvalidOperationException("Unable to locate live database"));

        /// <summary>
        /// Is the given database the live database for this archive
        /// </summary>
        public static Func<ISqlDatabaseDetail, bool> IsLiveDatabase(string databaseName, Guid databaseID) =>
            database =>
                database.Status == SqlDatabaseStatus.ShipWorks &&
                !database.IsArchive &&
                database.Guid == databaseID &&
                database.Name != databaseName;

        /// <summary>
        /// Get a list of archive databases for the current database
        /// </summary>
        private static Task<IEnumerable<ISqlDatabaseDetail>> GetArchiveDatabases(IShipWorksDatabaseUtility databaseUtility, ISqlSession sqlSession, Guid databaseID) =>
            Functional.UsingAsync(
                (DbConnection) sqlSession.OpenConnection(),
                databaseUtility.GetDatabaseDetails)
            .Map(databases => databases.Where(x => x.IsArchive && databaseID == x.Guid));
    }
}