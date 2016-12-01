using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Process shipments one at a time
    /// </summary>
    [Component]
    public class SerialProcessShipmentsWorkflow : IProcessShipmentsWorkflow
    {
        readonly PrepareShipmentStep prepareShipmentTask;
        readonly GetLabelStep getLabelTask;
        readonly SaveLabelStep saveLabelTask;
        readonly CompleteLabelStep completeLabelTask;
        readonly IShippingManager shippingManager;
        readonly Func<Control> ownerRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public SerialProcessShipmentsWorkflow(
            PrepareShipmentStep prepareShipmentTask,
            GetLabelStep getLabelTask,
            SaveLabelStep saveLabelTask,
            CompleteLabelStep completeLabelTask,
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
        /// Process the shipments
        /// </summary>
        public async Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction)
        {
            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(ownerRetriever(),
                "Processing Shipments",
                "ShipWorks is processing the shipments.",
                "Shipment {0} of {1}",
                false);

            prepareShipmentTask.CounterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessingAction;
            ShipmentProcessorExecutionState executionState = new ShipmentProcessorExecutionState(chosenRateResult);

            // What to do before it gets started (but is on the background thread)
            // sender is a ShipWorks.Common.Threading.BackgroundExecutor<ShipWorks.Data.Model.EntityClasses.ShipmentEntity>
            executor.ExecuteStarting += (sender, args) =>
            {
                // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
                executionState.ConcurrencyErrors = shippingManager.SaveShipmentsToDatabase(shipments, true);
            };

            await executor.ExecuteAsync(ProcessShipment, shipments, executionState);

            return executionState;
        }

        /// <summary>
        /// Process a single shipment
        /// </summary>
        private void ProcessShipment(ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder)
        {
            ShipmentProcessorExecutionState executionState = (ShipmentProcessorExecutionState) state;
            ProcessShipmentState initial = new ProcessShipmentState(shipment, executionState.LicenseCheckResults, executionState.SelectedRate);

            IPrepareShipmentResult prepareShipmentResult = prepareShipmentTask.PrepareShipment(initial);
            IGetLabelResult getLabelResult = getLabelTask.GetLabel(prepareShipmentResult);
            ISaveLabelResult saveLabelResult = saveLabelTask.SaveLabel(getLabelResult);
            ICompleteLabelCreationResult completeLabelResult = completeLabelTask.Complete(saveLabelResult);

            // When we introduce Akka.net, the rest of this method would go into the reducer method
            if (!string.IsNullOrEmpty(completeLabelResult.ErrorMessage))
            {
                executionState.NewErrors.Add("Order " + shipment.Order.OrderNumberComplete + ": " + completeLabelResult.ErrorMessage);
            }

            executionState.OutOfFundsException = executionState.OutOfFundsException ?? completeLabelResult.OutOfFundsException;
            executionState.TermsAndConditionsException = executionState.TermsAndConditionsException ?? completeLabelResult.TermsAndConditionsException;
            executionState.WorldshipExported |= completeLabelResult.WorldshipExported;

            if (completeLabelResult.Success)
            {
                executionState.OrderHashes.Add(shipment.Order.ShipSenseHashKey);
            }
        }
    }
}
