using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service for handling and opening the shipping dialog
    /// </summary>
    public class ShippingDialogService : IInitializeForCurrentSession, IDisposable
    {
        private readonly IDictionary<InitialShippingTabDisplay, string> shippingPanelTabNames =
            new Dictionary<InitialShippingTabDisplay, string>
            {
                {InitialShippingTabDisplay.Shipping, "ship"},
                {InitialShippingTabDisplay.Tracking, "track"},
                {InitialShippingTabDisplay.Insurance, "submit claims on" }
            };
        private readonly IMessenger messenger;
        private CompositeDisposable subscriptions;
        private readonly IMessageHelper messageHelper;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IWin32Window mainWindow;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDialogService(IMessenger messenger, IMessageHelper messageHelper,
            ISchedulerProvider schedulerProvider, IWin32Window mainWindow, IShippingManager shippingManager)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.schedulerProvider = schedulerProvider;
            this.mainWindow = mainWindow;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscriptions = new CompositeDisposable(
                messenger.OfType<OpenShippingDialogMessage>()
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Subscribe(OpenShippingDialog),
                messenger.OfType<OpenShippingDialogWithOrdersMessage>()
                    .Subscribe(async x => await LoadOrdersForShippingDialog(x).ConfigureAwait(false)),
                messenger.OfType<ShipAgainMessage>()
                    .SelectInBackgroundWithDialog(schedulerProvider, CreateProgressDialog, ShipAgain)
                    .Do(x => messenger.Send(new OrderSelectionChangingMessage(this, x.Shipments.Select(s => s.OrderID))))
                    .Subscribe(OpenShippingDialog),
                messenger.OfType<CreateReturnShipmentMessage>()
                    .SelectInBackgroundWithDialog(schedulerProvider, CreateProgressDialog, CreateReturnShipment)
                    .Do(x => messenger.Send(new OrderSelectionChangingMessage(this, x.Shipments.Select(s => s.OrderID))))
                    .Subscribe(OpenShippingDialog)
            );
        }

        /// <summary>
        /// Create a return shipment
        /// </summary>
        private OpenShippingDialogMessage CreateReturnShipment(CreateReturnShipmentMessage message)
        {
            ShipmentEntity shipment = shippingManager.CreateShipment(message.Shipment.Order);
            shipment.ReturnShipment = true;
            return new OpenShippingDialogMessage(this, new[] { shipment });
        }

        /// <summary>
        /// Ship the order again
        /// </summary>
        private OpenShippingDialogMessage ShipAgain(ShipAgainMessage message)
        {
            ShipmentEntity shipment = shippingManager.CreateShipment(message.Shipment.Order);
            return new OpenShippingDialogMessage(this, new[] { shipment });
        }

        /// <summary>
        /// Load orders that will be opened by the shipping dialog
        /// </summary>
        private async Task LoadOrdersForShippingDialog(OpenShippingDialogWithOrdersMessage message)
        {
            Control owner = IoC.UnsafeGlobalLifetimeScope.Resolve<Control>();

            if (message.OrderIDs.Count() > ShipmentsLoader.MaxAllowedOrders)
            {
                string actionName = shippingPanelTabNames[message.InitialDisplay];
                MessageHelper.ShowInformation(mainWindow, $"You can only {actionName} up to {ShipmentsLoader.MaxAllowedOrders} orders at a time.");
                return;
            }

            ShipmentsLoader loader = new ShipmentsLoader(owner);
            ShipmentsLoadedEventArgs results = await loader.LoadAsync(message.OrderIDs).ConfigureAwait(false);

            if (results.Cancelled)
            {
                return;
            }

            messenger.Send(new OpenShippingDialogMessage(this, results.Shipments, message.InitialDisplay));
        }

        /// <summary>
        /// Open the shipping dialog
        /// </summary>
        private void OpenShippingDialog(OpenShippingDialogMessage message)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope(ConfigureShippingDialogDependencies))
            {
                // Show the shipping window.
                ShippingDlg dlg = lifetimeScope.Resolve<ShippingDlg>(TypedParameter.From(message));

                dlg.ShowDialog(mainWindow);
            }

            // We always check for new server messages after shipping, since if there was a shipping problem
            // it could be we put out a server message related to it.
            DashboardManager.DownloadLatestServerMessages();
        }

        /// <summary>
        /// Configure extra dependencies for the shipping dialog
        /// </summary>
        private void ConfigureShippingDialogDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ShippingDlg>()
                .AsSelf()
                .As<Control>()
                .SingleInstance();
        }

        /// <summary>
        /// Create a progress dialog that will be displayed when creating new shipments
        /// </summary>
        private IDisposable CreateProgressDialog()
        {
            return messageHelper.ShowProgressDialog("Create Shipment",
                "ShipWorks is creating a new shipment for the selected orders.");
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}
