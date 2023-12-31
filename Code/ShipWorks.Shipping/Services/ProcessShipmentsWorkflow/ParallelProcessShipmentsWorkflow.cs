﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Process shipments in parallel
    /// </summary>
    [KeyedComponent(typeof(IProcessShipmentsWorkflow), ProcessShipmentsWorkflow.Parallel)]
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

            DataFlow<ProcessShipmentState, IEnumerable<ILabelResultLogResult>> dataflow = CreateDataFlow(cancellationSource);

            // Get shipment count with automatic returns
            int shipmentCount = 0;
            
            foreach (ShipmentEntity shipment in shipments)
            {
                shipmentCount++;
                if (shipment.IncludeReturn)
                {
                    shipmentCount++;
                }

                SetShipmentCarrierAccount(shipment, chosenRateResult);
            }
            
            workProgress.Detail = $"Shipment 1 of {shipmentCount}";

            var results = Consume(
                x =>
                {
                    workProgress.PercentComplete = (100 * x) / shipmentCount;
                    workProgress.Detail = $"Shipment {x} of {shipmentCount}";
                },
                new ProcessShipmentsWorkflowResult(chosenRateResult),
                dataflow);

            IEnumerable<ProcessShipmentState> input = await CreateShipmentProcessorInput(shipments, chosenRateResult, cancellationSource);

            foreach (var shipment in input)
            {
                if (cancellationSource.IsCancellationRequested)
                {
                    break;
                }

                await dataflow.SendAsync(shipment);
            }

            dataflow.Complete();

            ProcessShipmentsWorkflowResult result = await results;
            workProgress.Completed();
            return result;
        }

        /// <summary>
        /// Set the CarrierAccount property of the shipment
        /// </summary>
        private void SetShipmentCarrierAccount(ShipmentEntity shipment, RateResult rateResult)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                var tag = rateResult?.Tag as BestRateResultTag;
                shipment.CarrierAccount = tag?.AccountDescription;
            }

            else if (shipment.ShipmentTypeCode != ShipmentTypeCode.iParcel)
            {
                shipment.CarrierAccount = shippingManager.GetCarrierAccount(shipment)?.ShortAccountDescription;
            }
        }

        /// <summary>
        /// Create the data flow
        /// </summary>
        /// <remarks>
        /// We only handle canceling in the first step because once we're past that, we need to finish the rest
        /// </remarks>
        private DataFlow<ProcessShipmentState, IEnumerable<ILabelResultLogResult>> CreateDataFlow(CancellationTokenSource cancellationSource)
        {
            int taskCount = ConcurrencyCount;

            var prepareShipmentBlock = new TransformBlock<ProcessShipmentState, IShipmentPreparationResult>(x => prepareShipmentTask.PrepareShipment(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 1, CancellationToken = cancellationSource.Token });
            var getLabelBlock = new TransformBlock<IShipmentPreparationResult, IEnumerable<ILabelRetrievalResult>>(x => getLabelTask.GetLabels(x),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = taskCount, BoundedCapacity = 8 });
            var saveLabelBlock = new TransformBlock<IEnumerable<ILabelRetrievalResult>,
                IEnumerable<ILabelPersistenceResult>>(x => saveLabelTask.SaveLabels(x),
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 8 });
            var completeLabelBlock = new TransformBlock<IEnumerable<ILabelPersistenceResult>,
                IEnumerable<ILabelResultLogResult>>(x => completeLabelTask.Complete(x),
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
                await Task.Run(() =>
                {
                    using (new AuditBehaviorScope(AuditState.Disabled))
                    {
                        return shippingManager.SaveShipmentsToDatabase(shipments, true);
                    }
                });

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
            IReceivableSourceBlock<IEnumerable<ILabelResultLogResult>> phase)
        {
            IEnumerable<ILabelResultLogResult> items = null;

            int curShipmentIndex = 0;

            while (await phase.OutputAvailableAsync())
            {
                if (phase.TryReceive(out items))
                {
                    foreach (ILabelResultLogResult item in items)
                    {
                        if (item != null)
                        {
                            curShipmentIndex++;
                            logProgress(curShipmentIndex);
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
