﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services.Dialogs
{
    /// <summary>
    /// Service for handling and opening the shipping dialog
    /// </summary>
    public class ShippingDialogService : IInitializeForCurrentUISession
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
        private readonly IShippingManager shippingManager;
        private readonly Func<Owned<IOrderLoader>> shipmentsLoaderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDialogService(IMessenger messenger, IMessageHelper messageHelper,
            ISchedulerProvider schedulerProvider, IShippingManager shippingManager,
            Func<Owned<IOrderLoader>> shipmentsLoaderFactory)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.schedulerProvider = schedulerProvider;
            this.shippingManager = shippingManager;
            this.shipmentsLoaderFactory = shipmentsLoaderFactory;
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // We should never initialize an already initialized session. We'll re-subscribe in release but when
            // debugging, we should get alerted that this is happening
            Debug.Assert(subscriptions == null, "Subscription is already initialized");
            EndSession();

            subscriptions = new CompositeDisposable(
                messenger.OfType<OpenShippingDialogMessage>()
                    .IntervalCountThrottle(TimeSpan.FromSeconds(2), schedulerProvider)
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Subscribe(OpenShippingDialog),
                messenger.OfType<OpenShippingDialogWithOrdersMessage>()
                    .IntervalCountThrottle(TimeSpan.FromSeconds(2), schedulerProvider)
                    .Subscribe(async x => await LoadOrdersForShippingDialog(x).ConfigureAwait(false)),
                messenger.OfType<ShipAgainMessage>()
                    .SelectInBackgroundWithDialog(schedulerProvider, CreateProgressDialog, ShipAgain)
                    .Where(x => x != null)
                    .Select(x => new OpenShippingDialogMessage(this, new[] { x }))
                    .Do(x => messenger.Send(new OrderSelectionChangingMessage(this, x.Shipments.Select(s => s.OrderID))))
                    .Subscribe(OpenShippingDialog),
                messenger.OfType<CreateReturnShipmentMessage>()
                    .SelectInBackgroundWithDialog(schedulerProvider, CreateProgressDialog, CreateReturnShipment)
                    .Where(x => x != null)
                    .Select(x => new OpenShippingDialogMessage(this, new[] { x }))
                    .Do(x => messenger.Send(new OrderSelectionChangingMessage(this, x.Shipments.Select(s => s.OrderID))))
                    .Subscribe(OpenShippingDialog)
            );
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            subscriptions?.Dispose();
        }

        /// <summary>
        /// Create a return shipment
        /// </summary>
        private ShipmentEntity CreateReturnShipment(CreateReturnShipmentMessage message)
        {
            return CreateShipmentCopy(message.Shipment, x => x.ReturnShipment = true);
        }

        /// <summary>
        /// Ship the order again
        /// </summary>
        private ShipmentEntity ShipAgain(ShipAgainMessage message)
        {
            return CreateShipmentCopy(message.Shipment, null);
        }

        /// <summary>
        /// Load orders that will be opened by the shipping dialog
        /// </summary>
        private async Task LoadOrdersForShippingDialog(OpenShippingDialogWithOrdersMessage message)
        {
            if (message.OrderIDs.Count() > ShipmentsLoaderConstants.MaxAllowedOrders)
            {
                string actionName = shippingPanelTabNames[message.InitialDisplay];
                messageHelper.ShowInformation(
                    $"You can only {actionName} up to {ShipmentsLoaderConstants.MaxAllowedOrders} orders at a time.");
                return;
            }

            IOrderLoader shipmentsLoader = shipmentsLoaderFactory().Value;
            ShipmentsLoadedEventArgs results = await shipmentsLoader.LoadAsync(message.OrderIDs, ProgressDisplayOptions.Delay)
                .ConfigureAwait(false);

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
                messageHelper.ShowDialog(() => lifetimeScope.Resolve<ShippingDlg>(TypedParameter.From(message)));
            }

            // We always check for new server messages after shipping, since if there was a shipping problem
            // it could be we put out a server message related to it.
            DashboardManager.DownloadLatestServerMessages();

            messenger.Send(new OrderSelectionChangingMessage(this, message.Shipments.Select(x => x.OrderID).Distinct()));
        }

        /// <summary>
        /// Configure extra dependencies for the shipping dialog
        /// </summary>
        private void ConfigureShippingDialogDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ShippingDlg>()
                .AsSelf()
                .As<Control>()
                .As<IWin32Window>()
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
        /// Create a copy of the shipment
        /// </summary>
        private ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment, Action<ShipmentEntity> configure)
        {
            try
            {
                return shippingManager.CreateShipmentCopy(shipment, configure);
            }
            catch (SqlForeignKeyException)
            {
                messageHelper.ShowError("This order has been deleted, ShipWorks was unable to create a copy of the shipment.");
                return null;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose() => EndSession();
    }
}
