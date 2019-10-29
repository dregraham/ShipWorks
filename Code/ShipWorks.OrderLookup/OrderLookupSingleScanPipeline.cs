using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup.ScanToShip;
using ShipWorks.Settings;
using ShipWorks.SingleScan;
using ShipWorks.Stores.Communication;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Listens for scan. Sends OrderFound message when the scan corresponds to an order
    /// </summary>
    public class OrderLookupSingleScanPipeline : IOrderLookupPipeline
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
        public OrderLookupSingleScanPipeline(
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
            ILicenseService licenseService)
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
            log = createLogger(GetType());
        }

        /// <summary>
        /// Interface for initializing order lookup pipelines under a top level lifetime scope
        /// </summary>
        public void InitializeForCurrentScope()
        {
            EndSession();

            allowScanPack = (EditionRestrictionLevel.None == licenseService.CheckRestriction(EditionFeature.Warehouse, null));            
            scanToShipViewModel.ScanPackViewModel.Enabled=allowScanPack;

            subscriptions = new CompositeDisposable(

                // Process a barcode scan
                messenger.OfType<SingleScanMessage>()
                .Where(x => CanProcessSearchMessage() && !IsVerifyingOrder())
                .Do(_ => processingScan = true)
                .Do(_ => shipmentModel.Unload(OrderClearReason.NewSearch))
                .Do(x => OnSingleScanMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Process manual order seach (non-barcode scan)
                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => CanProcessSearchMessage())
                .Do(_ => processingScan = true)
                .Do(_ => shipmentModel.Unload(OrderClearReason.NewSearch))
                .Do(x => OnOrderLookupSearchMessage(x).Wait())
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
                .Do(x => LoadOrder(x.Order).Wait())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                // Clear 
                messenger.OfType<OrderLookupClearOrderMessage>()
                .Where(x => CanProcessSearchMessage())
                .Where(x => x.Reason == OrderClearReason.Reset)
                .Do(x => scanToShipViewModel.ScanPackViewModel.Reset())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger
                .OfType<OrderLookupClearOrderMessage>()
                .Where(x =>
                        x.Reason == OrderClearReason.Reset ||
                        x.Reason == OrderClearReason.NewSearch ||
                        x.Reason == OrderClearReason.ErrorLoadingOrder)
                .Subscribe(x => ClearOrderError(x.Reason))
            );
        }

        private void ClearOrderError(OrderClearReason reason)
        {
            scanToShipViewModel.SearchViewModel.ClearOrderError(reason);
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
        private bool CanProcessSearchMessage()
        {
            return !processingScan &&
                !mainForm.AdditionalFormsOpen() &&
                mainForm.UIMode == UIMode.OrderLookup &&
                !mainForm.IsShipmentHistoryActive();
        }

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
            log.Error("Error occurred while handling scan message in OrderLookupSingleScanPipeline.", ex);

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
                        }).ConfigureAwait(true);

                        // Capture the time required for loading the shipment data
                        await shipmentLoadTelemetricResult.RunTimedEventAsync(DataLoadingTelemetryTimeSliceName, async () =>
                        {
                            if (order == null)
                            {
                                order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
                            }

                            loadOrder = order?.Shipments.Any() == true;

                            if (loadOrder)
                            {
                                if (!order.Shipments.Last().Processed)
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
                        }).ConfigureAwait(true);

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
                            order = await orderRepository.GetOrder(orderId.Value).ConfigureAwait(true);
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
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscriptions?.Dispose();
    }
}
