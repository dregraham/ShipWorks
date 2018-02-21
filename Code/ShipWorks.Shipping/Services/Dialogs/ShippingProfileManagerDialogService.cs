﻿using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Messaging.Messages.Dialogs;

namespace ShipWorks.Shipping.Services.Dialogs
{
    /// <summary>
    /// Service for handling and opening the profile manager dialog
    /// </summary>
    public class ShippingProfileManagerDialogService : IInitializeForCurrentUISession
    {
        readonly IObservable<IShipWorksMessage> messenger;
        readonly IWin32Window mainWindow;
        private readonly IShippingProfileManagerDialogFactory shippingProfileManagerDialogFactory;
        readonly ISchedulerProvider schedulerProvider;
        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogService(IObservable<IShipWorksMessage> messenger,
            ISchedulerProvider schedulerProvider, IWin32Window mainWindow, 
            IShippingProfileManagerDialogFactory shippingProfileManagerDialogFactory)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.mainWindow = mainWindow;
            this.shippingProfileManagerDialogFactory = shippingProfileManagerDialogFactory;
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // We should never initialize an already initialized session. We'll re-subscribe in release but when
            // debugging, we should get alerted that this is happening
            Debug.Assert(subscription == null, "Subscription is already initialized");
            EndSession();

            subscription = messenger.OfType<OpenProfileManagerDialogMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(OpenProfileManagerDialog);
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Open the profile manager dialog
        /// </summary>
        private void OpenProfileManagerDialog(OpenProfileManagerDialogMessage message)
        {
            IDialog dlg = shippingProfileManagerDialogFactory.Create();
            dlg.ShowDialog();
            
            message.OnComplete?.Invoke();
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose() => EndSession();
    }
}
