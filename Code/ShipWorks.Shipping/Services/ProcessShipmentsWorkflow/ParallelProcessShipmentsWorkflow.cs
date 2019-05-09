﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Win32;
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
        private readonly ILabelRetrievalStep getLabelTask;
        private readonly LabelPersistenceStep saveLabelTask;
        private readonly LabelResultLogStep completeLabelTask;
        private readonly IShippingManager shippingManager;
        private int? concurrencyCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParallelProcessShipmentsWorkflow(
            ShipmentPreparationStep prepareShipmentTask,
            ILabelRetrievalStep getLabelTask,
            LabelPersistenceStep saveLabelTask,
            LabelResultLogStep completeLabelTask,
            IShippingManager shippingManager)
        {
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
        /// Concurrent number of tasks used for processing shipments
        /// </summary>
        public int ConcurrencyCount
        {
            get
            {
                if (!concurrencyCount.HasValue)
                {
                    concurrencyCount = GetConcurrencyCount("requests", 3, 64);
                }

                return concurrencyCount.Value;
            }
        }

        /// <summary>
        /// Process the shipments
        /// </summary>
        public async Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, IProgressReporter workProgress, CancellationTokenSource cancellationSource,
            Action counterRateCarrierConfiguredWhileProcessingAction)
        {
            workProgress.Starting();
            prepareShipmentTask.CounterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessingAction;

            DataFlow<ProcessShipmentState, Tuple<ILabelResultLogResult, ILabelResultLogResult>> dataflow = CreateDataFlow(cancellationSource);

            // Get shipment count with automatic returns
            int shipmentCount = 0;
            foreach (ShipmentEntity shipment in shipments)
            {
                shipmentCount++;
                if (shipment.IncludeReturn)
                {
                    shipmentCount++;
                }
            }

            var results = Consume(
                x => workProgress.PercentComplete = (100 * x) / shipmentCount,
                new ProcessShipmentsWorkflowResult(chosenRateResult),
                dataflow);

            IEnumerable<ProcessShipmentState> input = await CreateShipmentProcessorInput(shipments, chosenRateResult, cancellationSource);
            foreach (var shipment in input)
            {
                if (cancellationSource.IsCancellationRequested)
                {
                    break;
                }

                workProgress.Detail = $"Shipment {shipment.Index + 1} of {shipmentCount}";
                await dataflow.SendAsync(shipment);
            }

            dataflow.Complete();

            ProcessShipmentsWorkflowResult result = await results;
            workProgress.Completed();
            return result;
        }

        /// <summary>
        /// Create the data flow
        /// </summary>
        /// <remarks>
        /// We only handle canceling in the first step because once we're past that, we need to finish the rest
        /// </remarks>
        private DataFlow<ProcessShipmentState, Tuple<ILabelResultLogResult, ILabelResultLogResult>> CreateDataFlow(CancellationTokenSource cancellationSource)
        {
            int taskCount = ConcurrencyCount;

            var prepareShipmentBlock = new TransformBlock<ProcessShipmentState, IShipmentPreparationResult>(x => prepareShipmentTask.PrepareShipment(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 1, CancellationToken = cancellationSource.Token });
            var getLabelBlock = new TransformBlock<IShipmentPreparationResult, Tuple<ILabelRetrievalResult, ILabelRetrievalResult>>(x => getLabelTask.GetLabels(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = taskCount, BoundedCapacity = 8 });
            var saveLabelBlock = new TransformBlock<Tuple<ILabelRetrievalResult, ILabelRetrievalResult>,
                Tuple<ILabelPersistenceResult, ILabelPersistenceResult>>(x => saveLabelTask.SaveLabels(x),
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 8 });
            var completeLabelBlock = new TransformBlock<Tuple<ILabelPersistenceResult, ILabelPersistenceResult>,
                Tuple<ILabelResultLogResult, ILabelResultLogResult>>(x => completeLabelTask.Complete(x),
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = taskCount, BoundedCapacity = 8 });

            prepareShipmentBlock.LinkTo(getLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });
            getLabelBlock.LinkTo(saveLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });
            saveLabelBlock.LinkTo(completeLabelBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return DataFlow.Create(prepareShipmentBlock, completeLabelBlock);
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
        /// Consume the results of processing
        /// </summary>
        /// <remarks>
        /// We don't handle cancellation here because anything that's in flight needs to finish,
        /// regardless of whether a cancel was requested or not
        /// </remarks>
        private static async Task<ProcessShipmentsWorkflowResult> Consume(
            Action<int> logProgress,
            ProcessShipmentsWorkflowResult accumulator,
            IReceivableSourceBlock<Tuple<ILabelResultLogResult, ILabelResultLogResult>> phase)
        {
            Tuple<ILabelResultLogResult, ILabelResultLogResult> items = null;

            while (await phase.OutputAvailableAsync())
            {
                if (phase.TryReceive(out items))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        ILabelResultLogResult item = items.Item1;

                        if (i == 1)
                        {
                            item = items.Item2;
                        }

                        if (item != null)
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

                            accumulator.Shipments.Add(item.OriginalShipment);
                        }
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
