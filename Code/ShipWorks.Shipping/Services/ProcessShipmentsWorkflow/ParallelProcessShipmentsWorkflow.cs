using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Interapptive.Shared;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Process shipments in parallel
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ParallelProcessShipmentsWorkflow : IProcessShipmentsWorkflow
    {
        public const string LabelProcessingConcurrencyBasePath = @"Software\Interapptive\ShipWorks\Options\LabelProcessingConcurrency";
        private readonly ShipmentPreparationStep prepareShipmentTask;
        private readonly LabelRetrievalStep getLabelTask;
        private readonly LabelPersistenceStep saveLabelTask;
        private readonly LabelResultLogStep completeLabelTask;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ParallelProcessShipmentsWorkflow(
            ShipmentPreparationStep prepareShipmentTask,
            LabelRetrievalStep getLabelTask,
            LabelPersistenceStep saveLabelTask,
            LabelResultLogStep completeLabelTask,
            IShippingManager shippingManager,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.shippingManager = shippingManager;
            this.completeLabelTask = completeLabelTask;
            this.saveLabelTask = saveLabelTask;
            this.getLabelTask = getLabelTask;
            this.prepareShipmentTask = prepareShipmentTask;
        }

        /// <summary>
        /// Get the name of the workflow
        /// </summary>
        public string Name => "Parallel";

        /// <summary>
        /// Process the shipments
        /// </summary>
        public async Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction)
        {
            IProgressProvider progressProvider = new ProgressProvider();

            // Progress Item
            IProgressReporter workProgress = new ProgressItem("Processing Shipments");
            progressProvider.ProgressItems.Add(workProgress);

            using (messageHelper.ShowProgressDialog("Processing Shipments",
                "ShipWorks is processing the shipments.", progressProvider, TimeSpan.Zero))
            {
                prepareShipmentTask.CounterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessingAction;

                DataFlow<ProcessShipmentState, ILabelResultLogResult> dataflow = CreateDataFlow();

                int shipmentCount = shipments.Count();
                var results = Consume(
                    x => workProgress.PercentComplete = (100 * x) / shipmentCount,
                    new ShipmentProcessorExecutionState(chosenRateResult),
                    dataflow);

                IEnumerable<ProcessShipmentState> input = await CreateShipmentProcessorInput(shipments, chosenRateResult);
                foreach (var shipment in input)
                {
                    workProgress.Detail = $"Shipment {shipment.Index + 1} of {shipmentCount}";
                    await dataflow.SendAsync(shipment);
                }

                dataflow.Complete();

                return await results;
            }
        }

        /// <summary>
        /// Create the data flow
        /// </summary>
        private DataFlow<ProcessShipmentState, ILabelResultLogResult> CreateDataFlow()
        {
            int taskCount = GetConcurrencyCount("requests", 4, 64);

            var prepareShipmentBlock = new TransformBlock<ProcessShipmentState, IShipmentPreparationResult>(x => prepareShipmentTask.PrepareShipment(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 8 });
            var getLabelBlock = new TransformBlock<IShipmentPreparationResult, ILabelRetrievalResult>(x => getLabelTask.GetLabel(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = taskCount, BoundedCapacity = 8 });
            var saveLabelBlock = new TransformBlock<ILabelRetrievalResult, ILabelPersistenceResult>(x => saveLabelTask.SaveLabel(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 8 });
            var completeLabelBlock = new TransformBlock<ILabelPersistenceResult, ILabelResultLogResult>(x => completeLabelTask.Complete(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = taskCount, BoundedCapacity = 8 });

            prepareShipmentBlock.LinkTo(getLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });
            getLabelBlock.LinkTo(saveLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });
            saveLabelBlock.LinkTo(completeLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return DataFlow.Create(prepareShipmentBlock, completeLabelBlock);
        }

        /// <summary>
        /// Create processing input from each shipment that will be used by the processor
        /// </summary>
        private async Task<IEnumerable<ProcessShipmentState>> CreateShipmentProcessorInput(IEnumerable<ShipmentEntity> shipments, RateResult chosenRateResult)
        {
            IDictionary<long, Exception> licenseCheckCache = new Dictionary<long, Exception>();

            // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
            IDictionary<ShipmentEntity, Exception> concurrencyErrors =
                await Task.Run(() => shippingManager.SaveShipmentsToDatabase(shipments, true));

            return shipments.Select((shipment, i) =>
            {
                return concurrencyErrors.ContainsKey(shipment) ?
                    new ProcessShipmentState(i, concurrencyErrors[shipment]) :
                    new ProcessShipmentState(i, shipment, licenseCheckCache, chosenRateResult);
            });
        }

        /// <summary>
        /// Consume the results of processing
        /// </summary>
        private static async Task<ShipmentProcessorExecutionState> Consume(
            Action<int> logProgress,
            ShipmentProcessorExecutionState accumulator,
            IReceivableSourceBlock<ILabelResultLogResult> phase)
        {
            ILabelResultLogResult item = null;

            while (await phase.OutputAvailableAsync())
            {
                if (phase.TryReceive(out item))
                {
                    logProgress(item.Index);
                    accumulator.OutOfFundsException = accumulator.OutOfFundsException ?? item.OutOfFundsException;
                    accumulator.TermsAndConditionsException = accumulator.TermsAndConditionsException ?? item.TermsAndConditionsException;
                    accumulator.WorldshipExported |= item.WorldshipExported;

                    if (!string.IsNullOrEmpty(item.ErrorMessage))
                    {
                        accumulator.NewErrors.Add(item.ErrorMessage);
                    }

                    if (item.Success)
                    {
                        accumulator.OrderHashes.Add(item.OriginalShipment.Order.ShipSenseHashKey);
                    }
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Get how many labels should be processed concurrently
        /// </summary>
        private static int GetConcurrencyCount(string key, int defaultValue, int maxValue)
        {
            RegistryHelper registry = new RegistryHelper(LabelProcessingConcurrencyBasePath);
            int taskCount = defaultValue;

            if (!int.TryParse(registry.GetValue(key, defaultValue.ToString()), out taskCount))
            {
                return defaultValue;
            }

            if (taskCount < 1 || taskCount > maxValue)
            {
                return defaultValue;
            }

            return taskCount;
        }
    }
}
