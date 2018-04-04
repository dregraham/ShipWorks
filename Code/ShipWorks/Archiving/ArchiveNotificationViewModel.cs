using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Users;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// View Model for the Archive Notification View Model
    /// </summary>
    [Component]
    public class ArchiveNotificationViewModel : IArchiveNotificationViewModel, INotifyPropertyChanged
    {
        private readonly Func<IArchiveNotification> createControl;
        private readonly IUserLoginWorkflow loginWorkflow;
        private readonly ISqlSession sqlSession;
        private readonly IShipWorksDatabaseUtility databaseUtility;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool isConnecting = false;

        public event PropertyChangedEventHandler PropertyChanged;

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

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConnectToLiveDatabase = new RelayCommand(() => ConnectToLiveDatabaseAction());
        }

        /// <summary>
        /// Command to connect to the live database
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConnectToLiveDatabase { get; set; }

        /// <summary>
        /// Is the panel connecting to the live database
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsConnecting
        {
            get => isConnecting;
            set => handler.Set(nameof(IsConnecting), ref isConnecting, value);
        }

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
            Functional.UsingAsync(
                StartConnecting(),
                _ => GetLiveDatabase()
                    .Do((Action<SqlDatabaseDetail>) ChangeDatabase, ShowError, ContinueOn.CurrentThread));

        /// <summary>
        /// Start the connection process
        /// </summary>
        private IDisposable StartConnecting()
        {
            IsConnecting = true;
            return Disposable.Create(() => IsConnecting = false);
        }

        /// <summary>
        /// Get the live database
        /// </summary>
        private Task<SqlDatabaseDetail> GetLiveDatabase() =>
            Functional.UsingAsync(
                sqlSession.OpenConnection(),
                con => databaseUtility
                    .GetDatabaseDetails(con)
                    .Map(x => x.Where(IsLiveDatabase).FirstOrDefault())
                    .Bind(x => x != null ?
                        Task.FromResult(x) :
                        Task.FromException<SqlDatabaseDetail>(new InvalidOperationException("Could not find live database"))));

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
        private void ChangeDatabase(SqlDatabaseDetail database)
        {
            if (loginWorkflow.Logoff(false))
            {
                var newSession = sqlSession.CreateCopy();
                newSession.Configuration.DatabaseName = database.Name;
                newSession.SaveAsCurrent();

                loginWorkflow.Logon(null);
            }
        }

        /// <summary>
        /// Show an error message
        /// </summary>
        private void ShowError(Exception arg) =>
            messageHelper.ShowError(arg.Message);
    }
}