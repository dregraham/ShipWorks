using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
        private readonly LabelRetrievalStep getLabelTask;
        private readonly LabelPersistenceStep saveLabelTask;
        private readonly LabelResultLogStep completeLabelTask;
        private readonly IShippingManager shippingManager;
        private readonly Func<Control> ownerRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public SerialProcessShipmentsWorkflow(
            ShipmentPreparationStep prepareShipmentTask,
            LabelRetrievalStep getLabelTask,
            LabelPersistenceStep saveLabelTask,
            LabelResultLogStep completeLabelTask,
            IShippingManager shippingManager,
            Func<Control> ownerRetriever)
        {
            this.ownerRetriever = ownerRetriever;
            this.shippingManager = shippingManager;
            this.completeLabelTask = completeLabelTask;
            this.saveLabelTask = saveLabelTask;
            this.getLabelTask = getLabelTask;
            this.prepareShipmentTask = prepareShipmentTask;
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
            prepareShipmentTask.CounterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessingAction;

            IEnumerable<ProcessShipmentState> input = await CreateShipmentProcessorInput(shipments, chosenRateResult, cancellationSource);
            return await Task.Run(() =>
            {
                ShipmentProcessorExecutionState executionState = new ShipmentProcessorExecutionState(chosenRateResult);
                return input.Where(_ => !cancellationSource.IsCancellationRequested)
                    .Select(x => ProcessShipment(x, workProgress, shipments.Count()))
                    .Where(x => x != null)
                    .Aggregate(executionState, AggregateResults);
            });
        }

        /// <summary>
        /// Aggregate the results
        /// </summary>
        private ShipmentProcessorExecutionState AggregateResults(ShipmentProcessorExecutionState executionState, ILabelResultLogResult result)
        {
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                executionState.NewErrors.Add("Order " + result.OriginalShipment.Order.OrderNumberComplete + ": " + result.ErrorMessage);
            }

            executionState.OutOfFundsException = executionState.OutOfFundsException ?? result.OutOfFundsException;
            executionState.TermsAndConditionsException = executionState.TermsAndConditionsException ?? result.TermsAndConditionsException;
            executionState.WorldshipExported |= result.WorldshipExported;

            if (result.Success)
            {
                executionState.OrderHashes.Add(result.OriginalShipment.Order.ShipSenseHashKey);
            }

            return executionState;
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
        private ILabelResultLogResult ProcessShipment(ProcessShipmentState initial, IProgressReporter workProgress, int shipmentCount)
        {
            workProgress.Detail = $"Shipment {initial.Index + 1} of {shipmentCount}";

            IShipmentPreparationResult prepareShipmentResult = null;

            try
            {
                prepareShipmentResult = prepareShipmentTask.PrepareShipment(initial);
                if (initial.CancellationSource.IsCancellationRequested)
                {
                    return null;
                }

                ILabelRetrievalResult getLabelResult = getLabelTask.GetLabel(prepareShipmentResult);
                ILabelPersistenceResult saveLabelResult = saveLabelTask.SaveLabel(getLabelResult);
                return completeLabelTask.Complete(saveLabelResult);
            }
            finally
            {
                prepareShipmentResult?.EntityLock?.Dispose();
            }
        }
    }
}
