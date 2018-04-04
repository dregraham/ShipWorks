using System;
using System.Linq;
using System.Reactive;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Users;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// View Model for the Archive Notification View Model
    /// </summary>
    [Component]
    public class ArchiveNotificationViewModel : IArchiveNotificationViewModel
    {
        private readonly Func<IArchiveNotification> createControl;
        private readonly IUserLoginWorkflow loginWorkflow;
        private readonly ISqlSession sqlSession;
        private readonly IShipWorksDatabaseUtility databaseUtility;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveNotificationViewModel(
            Func<IArchiveNotification> createControl,
            IUserLoginWorkflow loginWorkflow,
            ISqlSession sqlSession,
            IShipWorksDatabaseUtility databaseUtility,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.databaseUtility = databaseUtility;
            this.loginWorkflow = loginWorkflow;
            this.sqlSession = sqlSession;
            this.createControl = createControl;

            ConnectToLiveDatabase = new RelayCommand(() => ConnectToLiveDatabaseAction());
        }

        /// <summary>
        /// Command to connect to the live database
        /// </summary>
        public ICommand ConnectToLiveDatabase { get; set; }

        /// <summary>
        /// Show the archive notification
        /// </summary>
        public void Show(ElementHost host)
        {
            createControl().AddTo(host, this);
        }

        /// <summary>
        /// Connect to the live database
        /// </summary>
        private void ConnectToLiveDatabaseAction() =>
            GetLiveDatabase()
                .Match(ChangeDatabase, ShowError);

        /// <summary>
        /// Get the live database
        /// </summary>
        private GenericResult<SqlDatabaseDetail> GetLiveDatabase() =>
            Functional.Using(
                sqlSession.OpenConnection(),
                con => databaseUtility
                    .GetDatabaseDetails(con)
                    .Where(IsLiveDatabase)
                    .FirstOrDefault()
                    .ToResult(() => new InvalidOperationException("Could not find live database")));

        /// <summary>
        /// Is the given database the live database for this archive
        /// </summary>
        private bool IsLiveDatabase(SqlDatabaseDetail database) =>
            database.Status == SqlDatabaseStatus.ShipWorks &&
            !database.IsArchive &&
            database.Guid == sqlSession.DatabaseIdentifier &&
            database.Name != sqlSession.Configuration.DatabaseName;

        /// <summary>
        /// Change to the given database
        /// </summary>
        private Unit ChangeDatabase(SqlDatabaseDetail database)
        {
            if (loginWorkflow.Logoff(false))
            {
                var newSession = sqlSession.CreateCopy();
                newSession.Configuration.DatabaseName = database.Name;
                newSession.SaveAsCurrent();

                loginWorkflow.Logon(null);
            }

            return Unit.Default;
        }

        /// <summary>
        /// Show an error message
        /// </summary>
        private Unit ShowError(Exception arg)
        {
            messageHelper.ShowError(arg.Message);
            return Unit.Default;
        }
    }
}