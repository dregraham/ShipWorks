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
using ShipWorks.Data;
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
            return triggerType == ActionTriggerType.OrderDownloaded ||
                triggerType == ActionTriggerType.FilterContentChanged ||
                triggerType == ActionTriggerType.UserInitiated;
        }

        /// <summary>
        /// Run the task over the given input
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            var errors = new Dictionary<string, string>();

            var shipmentsToProcess = new List<ShipmentEntity>();

            foreach (long orderID in inputKeys)
            {
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);

                // The order was deleted
                if (order == null)
                {
                    continue;
                }

                var shipments = await GetShipments(order.OrderID);

                if (shipments.Failure)
                {
                    errors.Add(order.OrderNumberComplete, shipments.Exception.Message);
                    continue;
                }

                shipmentsToProcess.AddRange(shipments.Value);
            }

            if (shipmentsToProcess.Any())
            {
                IEnumerable<ProcessShipmentResult> results;

                using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory())
                {
                    refresher.RetrieveShipments = () => shipmentsToProcess;

                    var shipmentProcessor = shipmentProcessorFactory(messageHelper);

                    results = await shipmentProcessor.Process(shipmentsToProcess, refresher, null, null);
                }

                // Add any errors from processing
                results.Where(x => !x.IsSuccessful).ForEach(x => errors.Add(x.Shipment.Order.OrderNumberComplete, x.Error.Message));
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
        private async Task<GenericResult<IEnumerable<ShipmentEntity>>> GetShipments(long orderId)
        {
            // Get all of the shipments for the order id that are not voided, this will add a new shipment if the order currently has no shipments
            ShipmentsLoadedEventArgs loadedOrders = await orderLoader.LoadAsync(new[] { orderId }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);

            IEnumerable<ShipmentEntity> shipments = loadedOrders?.Shipments.Where(s => !s.Voided);
            IEnumerable<ShipmentEntity> confirmedShipments = GetConfirmedShipments(orderId, shipments);

            if (confirmedShipments.None())
            {
                return new ActionTaskRunException("No processable shipments");
            }

            if (confirmedShipments.Count() > 1 && !AllowMultiShipments)
            {
                return new ActionTaskRunException("More than one unprocessed shipment. To process multiple shipments, enable the option in the action editor.");
            }

            if (HasDisqualifyingShipmentTypes(confirmedShipments))
            {
                return new ActionTaskRunException("Cannot process shipments of type 'None'");
            }

            return GenericResult.FromSuccess(confirmedShipments);
        }

        /// <summary>
        /// Gets Confirmed Shipments
        /// </summary>
        /// <remarks>
        /// If the order has no shipments we create and return a shipment
        /// If the order only has processed shipments, return no shipments
        /// If the order has unprocessed shipments, we return them
        /// </remarks>
        private IEnumerable<ShipmentEntity> GetConfirmedShipments(long orderId, IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments != null)
            {
                if (shipments.None())
                {
                    return new[] { shipmentFactory.Create(orderId) };
                }

                if (shipments.All(s => s.Processed))
                {
                    return new ShipmentEntity[0];
                }

                return shipments.Where(s => !s.Processed);
            }

            return new ShipmentEntity[0];
        }

        /// <summary>
        /// Determines whether any shipment has a ShipmentTypeCode of "None".
        /// </summary>
        private static bool HasDisqualifyingShipmentTypes(IEnumerable<ShipmentEntity> shipments) =>
            shipments.Any(shipment => shipment.ShipmentTypeCode == ShipmentTypeCode.None);
    }
}
