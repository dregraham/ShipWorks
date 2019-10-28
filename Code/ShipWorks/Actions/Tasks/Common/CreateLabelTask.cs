using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for creating a label
    /// </summary>
    [ActionTask("Create a label", "CreateLabel", ActionTaskCategory.Output)]
    public class CreateLabelTask : ActionTask
    {
        private readonly IOrderLoader orderLoader;
        private readonly IShipmentFactory shipmentFactory;
        private readonly BackgroundAsyncMessageHelper messageHelper;
        private readonly Func<IAsyncMessageHelper, IShipmentProcessor> shipmentProcessorFactory;
        private readonly Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory;

        /// <summary>
        /// Whether or not to create labels for orders with multiple unprocessed shipments
        /// </summary>
        public bool AllowMultiShipments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelTask(IOrderLoader orderLoader,
            IShipmentFactory shipmentFactory,
            BackgroundAsyncMessageHelper messageHelper,
            Func<IAsyncMessageHelper, IShipmentProcessor> shipmentProcessorFactory,
            Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory)
        {
            this.orderLoader = orderLoader;
            this.shipmentFactory = shipmentFactory;
            this.messageHelper = messageHelper;
            this.shipmentProcessorFactory = shipmentProcessorFactory;
            this.shipmentRefresherFactory = shipmentRefresherFactory;
        }

        /// <summary>
        /// Create the editor for editing the settings of the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new CreateLabelTaskEditor(this);
        }

        /// <summary>
        /// How to label the input selection for the task
        /// </summary>
        public override string InputLabel
        {
            get { return "Create a label for:"; }
        }

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// Should the ActionTask be run async
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Is the task allowed to be run using the specified trigger type?
        /// </summary>
        /// <param name="triggerType">Type of trigger that should be tested</param>
        /// <returns></returns>
        public override bool IsAllowedForTrigger(ActionTriggerType triggerType)
        {
            return triggerType != ActionTriggerType.ShipmentProcessed &&
                triggerType != ActionTriggerType.Scheduled;
        }

        /// <summary>
        /// Run the task over the given input
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            var errors = new Dictionary<string, string>();
            var batches = inputKeys.SplitIntoChunksOf(25);

            foreach (var batch in batches)
            {
                var shipmentsToProcess = new List<ShipmentEntity>();

                // Get all of the shipments for the order given order IDs. This will add a new shipment if the order currently has no shipments
                ShipmentsLoadedEventArgs loadedShipments = await orderLoader.LoadAsync(batch.ToArray(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);

                // All of the orders were deleted. Do nothing.
                if (loadedShipments == null)
                {
                    continue;
                }

                IEnumerable<OrderEntity> orders = loadedShipments.Shipments.Select(x => x.Order).Distinct();

                foreach (OrderEntity order in orders)
                {
                    var shipments = VerifyShipments(order.Shipments);

                    if (shipments.Failure)
                    {
                        errors.Add(order.OrderNumberComplete, shipments.Exception.Message);
                        continue;
                    }

                    shipmentsToProcess.AddRange(shipments.Value);
                }

                if (shipmentsToProcess.Any())
                {
                    var results = await ProcessShipments(shipmentsToProcess);

                    // Add any errors from processing. Uses indexer instead of .Add so that we always get the last error for a given order
                    results.Where(x => !x.IsSuccessful).ForEach(x => errors[x.Shipment.Order.OrderNumberComplete] = x.Error.Message);
                }
            }

            if (errors.Any())
            {
                // Return a list of errors with the order numbers
                string errorList = string.Join("\n", errors.Select(x => $"{x.Key}: {x.Value}"));

                throw new ActionTaskRunException($"Some errors occured during processing:\n\n{errorList}");
            }
        }

        /// <summary>
        /// Gets the shipments that should be auto processed
        /// </summary>
        private GenericResult<IEnumerable<ShipmentEntity>> VerifyShipments(IEnumerable<ShipmentEntity> shipments)
        {
            IEnumerable<ShipmentEntity> validShipments = GetValidShipments(shipments);

            if (validShipments.None())
            {
                return new ActionTaskRunException("No processable shipments");
            }

            if (validShipments.Count() > 1 && !AllowMultiShipments)
            {
                return new ActionTaskRunException("More than one unprocessed shipment. To process multiple shipments, enable the option in the action editor.");
            }

            return GenericResult.FromSuccess(validShipments);
        }

        /// <summary>
        /// Returns all unprocessed, non-voided shipments with a type other than none
        /// </summary>
        private IEnumerable<ShipmentEntity> GetValidShipments(IEnumerable<ShipmentEntity> shipments) =>
            shipments.Where(s => s.ShipmentTypeCode != ShipmentTypeCode.None && !(s.Voided || s.Processed));

        private async Task<IEnumerable<ProcessShipmentResult>> ProcessShipments(IEnumerable<ShipmentEntity> shipmentsToProcess)
        {
            IEnumerable<ProcessShipmentResult> results;

            using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory())
            {
                refresher.RetrieveShipments = () => shipmentsToProcess;

                var shipmentProcessor = shipmentProcessorFactory(messageHelper);

                results = await shipmentProcessor.Process(shipmentsToProcess, refresher, null, null);
            }

            return results;
        }
    }
}
