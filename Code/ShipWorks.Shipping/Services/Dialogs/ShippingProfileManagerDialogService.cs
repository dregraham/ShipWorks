using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services.Dialogs
{
    /// <summary>
    /// Service for handling and opening the profile manager dialog
    /// </summary>
    public class ShippingProfileManagerDialogService : IInitializeForCurrentUISession, IDisposable
    {
        readonly IObservable<IShipWorksMessage> messenger;
        readonly IWin32Window mainWindow;
        readonly ISchedulerProvider schedulerProvider;
        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogService(IObservable<IShipWorksMessage> messenger,
            ISchedulerProvider schedulerProvider, IWin32Window mainWindow)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<OpenProfileManagerDialogMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(OpenProfileManagerDialog);
        }

        /// <summary>
        /// Open the profile manager dialog
        /// </summary>
        private void OpenProfileManagerDialog(OpenProfileManagerDialogMessage message)
        {
            using (ShippingProfileManagerDlg dlg = new ShippingProfileManagerDlg(null))
            {
                dlg.ShowDialog(message.Sender as IWin32Window ?? mainWindow);
            }

            message.OnComplete?.Invoke();
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
