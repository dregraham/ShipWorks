using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Process shipments one at a time
    /// </summary>
    [Component(RegistrationType.Self)]
    public class SerialProcessShipmentsWorkflow : IProcessShipmentsWorkflow
    {
        private readonly ShipmentPreparationStep prepareShipmentTask;
        private readonly ILabelRetrievalStep getLabelTask;
        private readonly LabelPersistenceStep saveLabelTask;
        private readonly LabelResultLogStep completeLabelTask;
        private readonly IShippingManager shippingManager;
        private readonly Func<string, ITrackedDurationEvent> telemetryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SerialProcessShipmentsWorkflow(
            ShipmentPreparationStep prepareShipmentTask,
            ILabelRetrievalStep getLabelTask,
            LabelPersistenceStep saveLabelTask,
            LabelResultLogStep completeLabelTask,
            IShippingManager shippingManager,
            Func<string, ITrackedDurationEvent> telemetryFactory)
        {
            this.shippingManager = shippingManager;
            this.completeLabelTask = completeLabelTask;
            this.saveLabelTask = saveLabelTask;
            this.getLabelTask = getLabelTask;
            this.prepareShipmentTask = prepareShipmentTask;
            this.telemetryFactory = telemetryFactory;
        }

        /// <summary>
        /// Get the name of the workflow
        /// </summary>
        public string Name => "Serial";

        /// <summary>
        /// Concurrent number of tasks used for processing shipments
        /// </summary>
        public int ConcurrencyCount => 1;

        /// <summary>
        /// Process the shipments
        /// </summary>
        public async Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, IProgressReporter workProgress, CancellationTokenSource cancellationSource,
            Action counterRateCarrierConfiguredWhileProcessingAction)
        {
            using (ITrackedDurationEvent telemetryEvent = telemetryFactory("Shipping.Shipments.Process"))
            {
                telemetryEvent.AddMetric("Shipments.Count", shipments.Count());
                telemetryEvent.AddProperty("Processing.Workflow", Name);

                workProgress.CanCancel = false;
                workProgress.Starting();
                prepareShipmentTask.CounterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessingAction;

                IEnumerable<ProcessShipmentState> input = await CreateShipmentProcessorInput(shipments, chosenRateResult, cancellationSource);
                return await Task.Run(async () =>
                {
                    ProcessShipmentsWorkflowResult workflowResult = new ProcessShipmentsWorkflowResult(chosenRateResult);
                    List<ILabelResultLogResult> results = new List<ILabelResultLogResult>();
                    foreach (ProcessShipmentState processShipmentState in input)
                    {
                        if (cancellationSource.IsCancellationRequested)
                        {
                            break;
                        }

                        results.Add(await ProcessShipment(processShipmentState, workProgress, shipments.Count(), telemetryEvent).ConfigureAwait(false));
                    }

                    ProcessShipmentsWorkflowResult result = results
                        .Where(x => x != null)
                        .Aggregate(workflowResult, AggregateResults);

                    workProgress.Completed();

                    return result;
                });
            }
        }

        /// <summary>
        /// Aggregate the results
        /// </summary>
        private ProcessShipmentsWorkflowResult AggregateResults(ProcessShipmentsWorkflowResult workflowResult, ILabelResultLogResult result)
        {
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                workflowResult.NewErrors.Add("Order " + result.OriginalShipment.Order.OrderNumberComplete + ": " + result.ErrorMessage);
            }

            workflowResult.OutOfFundsException = workflowResult.OutOfFundsException ?? result.OutOfFundsException;
            workflowResult.TermsAndConditionsException = workflowResult.TermsAndConditionsException ?? result.TermsAndConditionsException;
            workflowResult.WorldshipExported |= result.WorldshipExported;

            if (result.Success)
            {
                workflowResult.OrderHashes.Add(result.OriginalShipment.Order.ShipSenseHashKey);
            }

            return workflowResult;
        }

        /// <summary>
        /// Create processing input from each shipment that will be used by the processor
        /// </summary>
        private async Task<IEnumerable<ProcessShipmentState>> CreateShipmentProcessorInput(
            IEnumerable<ShipmentEntity> shipments, RateResult chosenRateResult, CancellationTokenSource cancellationSource)
        {
            IDictionary<long, Exception> licenseCheckCache = new Dictionary<long, Exception>();

            // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
            IDictionary<ShipmentEntity, Exception> concurrencyErrors =
                await Task.Run(() => shippingManager.SaveShipmentsToDatabase(shipments, true));

            return shipments.Select((shipment, i) =>
            {
                return concurrencyErrors.ContainsKey(shipment) ?
                    new ProcessShipmentState(i, shipment, concurrencyErrors[shipment], cancellationSource) :
                    new ProcessShipmentState(i, shipment, licenseCheckCache, chosenRateResult, cancellationSource);
            });
        }

        /// <summary>
        /// Process a single shipment
        /// </summary>
        private async Task<ILabelResultLogResult> ProcessShipment(ProcessShipmentState initial,
            IProgressReporter workProgress, int shipmentCount, ITrackedDurationEvent telemetryEvent)
        {
            workProgress.Detail = $"Shipment {initial.Index + 1} of {shipmentCount}";

            TelemetricResult<ILabelResultLogResult> telemetricResult = new TelemetricResult<ILabelResultLogResult>("Shipments.Process");
            IShipmentPreparationResult prepareShipmentResult = null;

            try
            {
                prepareShipmentResult = telemetricResult.RunTimedEvent("PrepareShipment.DurationInMilliseconds",
                    () => prepareShipmentTask.PrepareShipment(initial));

                if (initial.CancellationSource.IsCancellationRequested)
                {
                    return null;
                }

                ILabelRetrievalResult getLabelResult = await telemetricResult.RunTimedEventAsync(
                        "GenerateLabel.DurationInMilliseconds",
                        () => getLabelTask.GetLabel(prepareShipmentResult))
                    .ConfigureAwait(false);

                ILabelPersistenceResult saveLabelResult = telemetricResult.RunTimedEvent("SaveLabel.DurationInMilliseconds",
                    () => saveLabelTask.SaveLabel(getLabelResult));

                ILabelResultLogResult logLabelResult = telemetricResult.RunTimedEvent("LogLabel.DurationInMilliseconds",
                    () => completeLabelTask.Complete(saveLabelResult));

                return logLabelResult;

            }
            finally
            {
                telemetricResult.WriteTo(telemetryEvent);
                prepareShipmentResult?.EntityLock?.Dispose();
            }
        }
    }
}
