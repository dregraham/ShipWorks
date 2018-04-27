using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the Archive Notification View Model
    /// </summary>
    [Component]
    public class ArchiveNotificationViewModel : IArchiveNotificationViewModel, INotifyPropertyChanged
    {
        private readonly Func<IArchiveNotification> createControl;
        private readonly IArchiveManagerDataAccess dataAccess;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool isConnecting = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveNotificationViewModel(
            Func<IArchiveNotification> createControl,
            IArchiveManagerDataAccess dataAccess,
            IMessageHelper messageHelper)
        {
            this.dataAccess = dataAccess;
            this.messageHelper = messageHelper;
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
        public void Show(ElementHost host) =>
            createControl().AddTo(host, this);

        /// <summary>
        /// Connect to the live database
        /// </summary>
        private void ConnectToLiveDatabaseAction() =>
            Functional.UsingAsync(StartConnecting(), progress => dataAccess.GetLiveDatabase())
                .Map(dataAccess.ChangeDatabase, ShowError, ContinueOn.CurrentThread);

        /// <summary>
        /// Start the connection process
        /// </summary>
        private ISingleItemProgressDialog StartConnecting()
        {
            var dialog = messageHelper.ShowProgressDialog("Reconnect to Live Database", "Reconnecting to the live ShipWorks database");
            dialog.ProgressItem.CanCancel = false;
            return dialog;
        }

        /// <summary>
        /// Show an error message
        /// </summary>
        private bool ShowError(Exception ex)
        {
            messageHelper.ShowError(ex.Message);
            return true;
        }
    }
}