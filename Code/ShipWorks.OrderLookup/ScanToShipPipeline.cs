using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup.Messages;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.OrderLookup.ScanToShip;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.SingleScan;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Listens for scan. Sends OrderFound message when the scan corresponds to an order
    /// </summary>
    public class ScanToShipPipeline : IScanToShipPipeline
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly IOrderLookupOrderRepository orderRepository;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly IOrderLookupAutoPrintService orderLookupAutoPrintService;
        private readonly IAutoWeighService autoWeighService;
        private readonly IOrderLookupShipmentModel shipmentModel;
        private readonly IOrderLookupConfirmationService orderLookupConfirmationService;
        private readonly Func<string, ITrackedDurationEvent> telemetryFactory;
        private readonly IOrderLookupOrderIDRetriever orderIDRetriever;
        private readonly IScanToShipViewModel scanToShipViewModel;
        private readonly ILicenseService licenseService;
        private readonly IUserSession userSession;
        private readonly IShortcutManager shortcutManager;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private readonly IOrderLoader orderLoader;
        private readonly IVerifiedOrderService verifiedOrderService;
        private readonly ILog log;
        private IDisposable subscriptions;
        private bool processingScan = false;
        private object loadingOrderLock = new object();
        bool allowScanPack = true;

        private const string AutoPrintTelemetryTimeSliceName = "AutoPrint.DurationInMilliseconds";
        private const string DataLoadingTelemetryTimeSliceName = "Data.Load.DurationInMilliseconds";
        private const string UserInputTelemetryTimeSliceName = "UserInput.DurationInMilliseconds";
        private const string OrderCountTelemetryPropertyName = "OrderLookup.Order.ResultCount";
        private const string InputSourceTelemetryPropertyName = "Input.Source";
        private const string InputTextTelemetryPropertyName = "Input.Text";

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IOrderLookupOrderRepository orderRepository,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            IOrderLookupAutoPrintService orderLookupAutoPrintService,
            IAutoWeighService autoWeighService,
            IOrderLookupShipmentModel shipmentModel,
            Func<Type, ILog> createLogger,
            IOrderLookupConfirmationService orderLookupConfirmationService,
            Func<string, ITrackedDurationEvent> telemetryFactory,
            IOrderLookupOrderIDRetriever orderIDRetriever,
            IScanToShipViewModel scanToShipViewModel,
            ILicenseService licenseService,
            IUserSession userSession,
            IShortcutManager shortcutManager,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IOrderLoader orderLoader,
            IVerifiedOrderService verifiedOrderService)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.orderRepository = orderRepository;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.orderLookupAutoPrintService = orderLookupAutoPrintService;
            this.autoWeighService = autoWeighService;
            this.shipmentModel = shipmentModel;
            this.orderLookupConfirmationService = orderLookupConfirmationService;
            this.telemetryFactory = telemetryFactory;
            this.orderIDRetriever = orderIDRetriever;
            this.scanToShipViewModel = scanToShipViewModel;
            this.licenseService = licenseService;
            this.userSession = userSession;
            this.shortcutManager = shortcutManager;
            this.shippingManager = shippingManager;
            this.messageHelper = messageHelper;
            this.orderLoader = orderLoader;
            this.verifiedOrderService = verifiedOrderService;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Interface for initializing order lookup pipelines under a top level lifetime scope
        /// </summary>
        public void InitializeForCurrentScope()
        {
            EndSession();

            allowScanPack = licenseService.IsHub;
            scanToShipViewModel.ScanPackViewModel.Enabled = allowScanPack;
            scanToShipViewModel.SelectedTab = (int) (allowScanPack ? ScanToShipTab.PackTab : ScanToShipTab.ShipTab);

            subscriptions = new CompositeDisposable(

                // Process a barcode scan
                messenger.OfType<SingleScanMessage>()
                .Where(x => CanProcessSearchMessage() && !IsVerifyingOrder())
                .Do(_ => processingScan = true)
                .Do(_ => shipmentModel.Unload(OrderClearReason.NewSearch))
                .Do(x => OnSingleScanMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Process manual order search (non-barcode scan)
                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => CanProcessSearchMessage())
                .Do(_ => processingScan = true)
                .Do(_ => shipmentModel.Unload(OrderClearReason.NewSearch))
                .Do(x => OnOrderLookupSearchMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Validate order line item scan
                messenger.OfType<SingleScanMessage>()
                .Where(x => CanProcessSearchMessage() && IsVerifyingOrder())
                .Do(x => processingScan = true)
                .Do(x => ValidateLineItem(x.ScannedText))
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Load the order in the UI
                messenger.OfType<OrderLookupLoadOrderMessage>()
                .Where(x => !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup)
                .Do(_ => shipmentModel.Unload(OrderClearReason.NewSearch))
                .Do(x => LoadOrder(x.Order).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Clear 
                messenger.OfType<OrderLookupClearOrderMessage>()
                .Where(x => CanProcessSearchMessage())
                .Where(x => x.Reason == OrderClearReason.Reset)
                .Do(x => scanToShipViewModel.ScanPackViewModel.Reset())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupClearOrderMessage>()
                    .Subscribe(x => HandleClearOrderMessage(x.Reason)),

                // Shipment loaded
                messenger.OfType<ScanToShipShipmentLoadedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Do(HandleShipmentLoadedMessage)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                // Shipment processed
                messenger.OfType<ShipmentsProcessedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Shipments.All(s => s.Shipment.Processed))
                    .Do(_ => scanToShipViewModel.IsOrderProcessed = true)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                // Order verified
                messenger.OfType<OrderVerifiedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Order.Verified)
                    .Do(HandleOrderVerifiedMessage)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                // Handle Shortcuts
                messenger.OfType<ShortcutMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Do(HandleShortcutMessages)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<OrderLookupUnverifyMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Do(HandleUnverifyOrderMessage)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<OrderLookupShipAgainMessage>()
                .Do(x => ShipAgain(x))
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe()
            );
        }

        /// <summary>
        /// Clear any messages or feedback related to the selected order.
        /// </summary>
        private void HandleClearOrderMessage(OrderClearReason orderClearReason)
        {
            scanToShipViewModel.Reset();

            if (orderClearReason == OrderClearReason.Reset ||
                orderClearReason == OrderClearReason.NewSearch ||
                orderClearReason == OrderClearReason.ErrorLoadingOrder)
            {
                scanToShipViewModel.SearchViewModel.ClearSearchMessage(orderClearReason);
            }
        }

        /// <summary>
        /// When validating an order, call this to validate a line item
        /// </summary>
        private void ValidateLineItem(string searchText)
        {
            try
            {
                scanToShipViewModel.ScanPackViewModel.ProcessItemScan(searchText);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Can we process an order search message from the scanner or searchbar
        /// </summary>
        private bool CanProcessSearchMessage() =>
            !processingScan &&
            !mainForm.AdditionalFormsOpen() &&
            mainForm.UIMode == UIMode.OrderLookup &&
            !mainForm.IsShipmentHistoryActive();

        /// <summary>
        /// Are we in the process of verifying an order?
        /// </summary>
        private bool IsVerifyingOrder() => 
            allowScanPack && 
            scanToShipViewModel.IsPackTabActive &&
            (scanToShipViewModel.ScanPackViewModel.State == ScanPack.ScanPackState.OrderLoaded ||
             scanToShipViewModel.ScanPackViewModel.State == ScanPack.ScanPackState.ScanningItems);

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        private void HandleException(Exception ex) =>
            log.Error("Error occurred while handling scan message in ScanToShipPipeline.", ex);

        /// <summary>
        /// Download order, auto print if needed, send order message
        /// </summary>
        public async Task OnSingleScanMessage(SingleScanMessage message)
        {
            try
            {
                using (ITrackedDurationEvent telemetryEvent = telemetryFactory("SingleScan.Search.OrderLookup"))
                {
                    telemetryEvent.AddProperty(InputSourceTelemetryPropertyName, "Barcode");
                    telemetryEvent.AddProperty(InputTextTelemetryPropertyName, message.ScannedText);

                    TelemetricResult<long?> orderLookupTelemetricResult = await orderIDRetriever
                        .GetOrderID(message.ScannedText, UserInputTelemetryTimeSliceName, DataLoadingTelemetryTimeSliceName, OrderCountTelemetryPropertyName).ConfigureAwait(true);
                    orderLookupTelemetricResult.WriteTo(telemetryEvent);

                    long? orderId = orderLookupTelemetricResult.Value;
                    bool loadOrder = false;

                    OrderEntity order = null;
                    if (orderId.HasValue)
                    {
                        TelemetricResult<bool> shipmentLoadTelemetricResult = new TelemetricResult<bool>("Shipment");
                        order = await shipmentLoadTelemetricResult.RunTimedEventAsync(AutoPrintTelemetryTimeSliceName, async () =>
                        {
                            AutoPrintCompletionResult result = await orderLookupAutoPrintService.AutoPrintShipment(orderId.Value, message.ScannedText).ConfigureAwait(false);
                            return result.ProcessShipmentResults?.Select(x => x.Shipment.Order).FirstOrDefault();
                        }).ConfigureAwait(true); // setting this to false will cause problems

                        // Capture the time required for loading the shipment data
                        await shipmentLoadTelemetricResult.RunTimedEventAsync(DataLoadingTelemetryTimeSliceName, async () =>
                        {
                            if (order == null)
                            {
                                // setting ConfigureAwait to false will cause problems
                                order = await orderRepository.GetOrder(orderId.Value, true).ConfigureAwait(true); 
                            }

                            loadOrder = order?.Shipments.Any() == true;

                            if (loadOrder)
                            {
                                // only auto weigh if auto print is disabled because auto print would have auto weigh already otherwise
                                if (!order.Shipments.Last().Processed && !orderLookupAutoPrintService.AllowAutoPrint(message.ScannedText))
                                {
                                    using (ITrackedEvent telemetry = new TrackedEvent("OrderLookup.Search.AutoWeigh"))
                                    {
                                        autoWeighService.ApplyWeight(new[] { order.Shipments.Last() }, telemetry);
                                    }
                                }

                                messenger.Send(new OrderLookupLoadOrderMessage(this, order));
                            }

                            if (!loadOrder)
                            {
                                shipmentModel.Unload();
                            }
                        }).ConfigureAwait(true); // setting ConfigureAwait to false will cause problems

                        shipmentLoadTelemetricResult.WriteTo(telemetryEvent);
                    }
                    else
                    {
                        messenger.Send(new OrderLookupLoadOrderMessage(this, null));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while loading an order", ex);
                shipmentModel.Unload(OrderClearReason.ErrorLoadingOrder);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Load the order
        /// </summary>
        public async Task LoadOrder(OrderEntity order)
        {
            processingScan = true;

            // If the order doesn't have any shipments, load them.
            if (order != null && order.Shipments.None())
            {
                ShipmentsLoadedEventArgs loadedOrders =
                    await orderLoader.LoadAsync(new[] {order.OrderID}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite)
                        .ConfigureAwait(true);

                OrderEntity orderWithShipments = loadedOrders?.Shipments?.FirstOrDefault()?.Order;

                if (orderWithShipments != null)
                {
                    order = orderWithShipments;
                }
            }

            shipmentModel.LoadOrder(order);

            if (allowScanPack)
            {
                await scanToShipViewModel.ScanPackViewModel.LoadOrder(order).ConfigureAwait(true);
            }

            processingScan = false;
        }

        /// <summary>
        /// Download order, send order message
        /// </summary>
        public async Task OnOrderLookupSearchMessage(OrderLookupSearchMessage message)
        {
            scanToShipViewModel.ScanPackViewModel.ScanHeader = "Loading order...";
            scanToShipViewModel.ScanPackViewModel.ScanFooter = string.Empty;

            try
            {
                using (ITrackedDurationEvent telemetryEvent = telemetryFactory("SingleScan.Search.OrderLookup"))
                {
                    telemetryEvent.AddProperty(InputSourceTelemetryPropertyName, "Keyboard");
                    telemetryEvent.AddProperty(InputTextTelemetryPropertyName, message.SearchText);

                    TelemetricResult<long?> orderSearchTelemetricResult = new TelemetricResult<long?>("Order");

                    // Track the time it took to load the order
                    List<long> orderIds = await orderSearchTelemetricResult.RunTimedEventAsync(
                        DataLoadingTelemetryTimeSliceName,
                                async () =>
                                {
                                    await Task.Run(() => onDemandDownloaderFactory.CreateOnDemandDownloader().Download(message.SearchText)).ConfigureAwait(false);
                                    return orderRepository.GetOrderIDs(message.SearchText);
                                })
                            .ConfigureAwait(true);

                    // Make a note of how many orders were found, so we can marry this up with the confirmation telemetry
                    orderSearchTelemetricResult.AddEntry(OrderCountTelemetryPropertyName, orderIds.Count);

                    // Track the time for gather user input (if needed at all)
                    long? selectedOrderId = await orderSearchTelemetricResult.RunTimedEventAsync(
                            UserInputTelemetryTimeSliceName,
                            () => orderLookupConfirmationService.ConfirmOrder(message.SearchText, orderIds))
                        .ConfigureAwait(true);
                    orderSearchTelemetricResult.SetValue(selectedOrderId);

                    // Record the order data to the telemetry event
                    orderSearchTelemetricResult.WriteTo(telemetryEvent);

                    // Instrument the time required to load the shipment
                    TelemetricResult<bool> shipmentLoadTelemetricResult = new TelemetricResult<bool>("Shipment");
                    await shipmentLoadTelemetricResult.RunTimedEventAsync(DataLoadingTelemetryTimeSliceName, async () =>
                    {
                        long? orderId = orderSearchTelemetricResult.Value;
                        OrderEntity order = null;

                        if (orderId.HasValue)
                        {
                            order = await orderRepository.GetOrder(orderId.Value, true).ConfigureAwait(true);
                        }

                        messenger.Send(new OrderLookupLoadOrderMessage(this, order));
                    }).ConfigureAwait(true);

                    // Record the shipment data to the telemetry event
                    shipmentLoadTelemetricResult.WriteTo(telemetryEvent);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while loading an order", ex);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Handle the shipment loaded message
        /// </summary>
        private void HandleShipmentLoadedMessage(ScanToShipShipmentLoadedMessage shipmentLoadedMessage)
        {
            scanToShipViewModel.UpdateOrderVerificationError();
            scanToShipViewModel.IsOrderVerified = shipmentLoadedMessage?.Shipment?.Order?.Verified ?? false;
            scanToShipViewModel.IsOrderProcessed = shipmentLoadedMessage?.Shipment?.Processed ?? false;

            scanToShipViewModel.SelectedTab = (int) GetInitialTab();
        }

        /// <summary>
        /// Gets the initial tab when loading an order
        /// </summary>
        private ScanToShipTab GetInitialTab()
        {
            var selectedTab = (ScanToShipTab) scanToShipViewModel.SelectedTab;

            if (!licenseService.IsHub)
            {
                return selectedTab;
            }

            if (userSession?.Settings?.AutoPrintRequireValidation == true && !scanToShipViewModel.IsOrderVerified)
            {
                selectedTab = ScanToShipTab.PackTab;
            }

            if (userSession?.Settings?.ScanToShipAutoAdvance == true)
            {
                if (scanToShipViewModel.IsOrderVerified)
                {
                    selectedTab = ScanToShipTab.ShipTab;
                }
                else
                {
                    selectedTab = ScanToShipTab.PackTab;
                }
            }

            return selectedTab;
        }

        /// <summary>
        /// Handle the order verified message
        /// </summary>
        private void HandleOrderVerifiedMessage(OrderVerifiedMessage orderVerifiedMessage)
        {
            scanToShipViewModel.ShowOrderVerificationError = false;
            scanToShipViewModel.IsOrderVerified = true;

            if (licenseService.IsHub && userSession?.Settings?.ScanToShipAutoAdvance == true)
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.ShipTab;
            }
        }

        /// <summary>
        /// Handle the unverify order message
        /// </summary>
        private async void HandleUnverifyOrderMessage(OrderLookupUnverifyMessage orderLookupUnverifyMessage)
        {
            if (shipmentModel.SelectedOrder.OrderID != orderLookupUnverifyMessage.OrderID)
            {
                // should never really happen
                log.Error("In HandleUnverifyOrderMessage, message orderid != SelectedOrderID");
                return;
            }

            verifiedOrderService.Save(shipmentModel.SelectedOrder, false);
            await LoadOrder(shipmentModel.SelectedOrder).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle Shortcuts
        /// </summary>
        private void HandleShortcutMessages(ShortcutMessage shortcutMessage)
        {
            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.NavigateToPackTab))
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.PackTab;
                shortcutManager.ShowShortcutIndicator(shortcutMessage);
            }

            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.NavigateToShipTab))
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.ShipTab;
                shortcutManager.ShowShortcutIndicator(shortcutMessage);
            }

            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.UnverifyOrder))
            {
                HandleUnverifyShortcutMessage(shortcutMessage);
            }
        }

        /// <summary>
        /// Handle UnverifyShortcutMessage
        /// </summary>
        private void HandleUnverifyShortcutMessage(ShortcutMessage shortcutMessage)
        {
            long? orderID = shipmentModel?.SelectedOrder?.OrderID;
            if (orderID == null)
            {
                messageHelper.ShowBarcodePopup("Order not selected - Cannot unverify.");
            }
            else if (shipmentModel.SelectedOrder.Shipments?.Any(s => s.Processed) == true)
            {
                messageHelper.ShowBarcodePopup("Cannot unverify a processed order.");
            }
            else
            {
                messenger.Send(new OrderLookupUnverifyMessage(this, orderID.Value));
                shortcutManager.ShowShortcutIndicator(shortcutMessage);
            }
        }

        /// <summary>
        /// Ship an order again
        /// </summary>
        private async void ShipAgain(OrderLookupShipAgainMessage message)
        {
            scanToShipViewModel.SelectedTab = (int) ScanToShipTab.ShipTab;

            using (messageHelper.ShowProgressDialog("Create Shipment", "Creating new shipment"))
            {
                var sourceShipment = scanToShipViewModel.OrderLookupViewModel.ShipmentModel.ShipmentAdapter?.Shipment;
                if (sourceShipment == null || sourceShipment.ShipmentID != message.ShipmentID)
                {
                    throw new InvalidOperationException("ShipmentID of message doesn't match the selected shipment");
                }
                
                ShipmentEntity shipment = shippingManager.CreateShipmentCopy(sourceShipment);
                scanToShipViewModel.OrderLookupViewModel.ShipmentModel.SelectedOrder.Shipments.Add(shipment);

                await LoadOrder(scanToShipViewModel.OrderLookupViewModel.ShipmentModel.SelectedOrder).ConfigureAwait(false);
                mainForm.SelectScanToShipTab();
            }
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscriptions?.Dispose();
    }
}
