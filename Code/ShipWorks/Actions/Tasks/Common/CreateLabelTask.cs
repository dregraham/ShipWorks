using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Users.Security;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for creating a label
    /// </summary>
    [ActionTask("Create label", "CreateLabel", ActionTaskCategory.Output)]
    public class CreateLabelTask : ActionTask
    {
        private readonly IOrderLoader orderLoader;
        private readonly Func<ISecurityContext> securityContext;
        private readonly IShipmentFactory shipmentFactory;

        /// <summary>
        /// Whether or not to create labels for orders with multiple unprocessed shipments
        /// </summary>
        public bool AllowMultiShipments { get; set; }

        /// <summary>
        /// Whether or not to create labels for orders with both processed and unprocessed shipments
        /// </summary>
        public bool AllowProcessedShipments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelTask(IOrderLoader orderLoader,
            Func<ISecurityContext> securityContext,
            IShipmentFactory shipmentFactory)
        {
            this.orderLoader = orderLoader;
            this.securityContext = securityContext;
            this.shipmentFactory = shipmentFactory;
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
            get { return "Create label for:"; }
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
        /// Run the task over the given input
        /// </summary>
        public override async Task RunAsync(List<long> inputKeys, IActionStepContext context)
        {
            var errors = new Dictionary<string, Exception>();

            foreach (long orderID in inputKeys)
            {
                // Get the order
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);

                if (order == null)
                {
                    continue;
                }

                var result = await GetShipments(order.OrderID);

                if (result.Failure)
                {
                    errors.Add(order.OrderNumberComplete, result.Exception);
                    continue;
                }
            }
        }

        /// <summary>
        /// Gets the Shipments that should be auto print/process
        /// </summary>
        private async Task<GenericResult<IEnumerable<ShipmentEntity>>> GetShipments(long orderId)
        {
            if (!securityContext().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderId))
            {
                return new ActionTaskRunException("You do not have permission to auto print");
            }

            // Get all of the shipments for the order id that are not voided, this will add a new shipment if the order currently has no shipments
            ShipmentsLoadedEventArgs loadedOrders = await orderLoader.LoadAsync(new[] { orderId }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);

            ShipmentEntity[] shipments = loadedOrders?.Shipments.Where(s => !s.Voided).ToArray();
            ShipmentEntity[] confirmedShipments = GetConfirmedShipments(orderId, shipments);

            if (confirmedShipments.None())
            {
                return new ActionTaskRunException("No processable shipments");
            }

            if (HasDisqualifyingShipmentTypes(confirmedShipments))
            {
                return new ActionTaskRunException("Cannot process shipments of type 'None'");
            }

            return confirmedShipments;
        }

        /// <summary>
        /// Gets Confirmed Shipments
        /// </summary>
        /// <remarks>
        /// If the order has a single unprocessed shipment, we return that shipment
        /// If the order has no shipments we create and return a shipment
        /// If the order only has processed shipments, we return an empty list
        /// If the order has multiple unprocessed shipments, we return them
        /// </remarks>
        private ShipmentEntity[] GetConfirmedShipments(long orderId, ShipmentEntity[] shipments)
        {
            if (shipments != null)
            {
                if (shipments.IsCountEqualTo(1) && shipments.All(s => !s.Processed))
                {
                    return shipments;
                }

                if (shipments.None())
                {
                    return new[] { shipmentFactory.Create(orderId) };
                }

                if (shipments.All(s => s.Processed))
                {
                    return new ShipmentEntity[0];
                }
                else if (ShouldPrintAndProcessShipments(shipments, scannedBarcode))
                {
                    // If all of the shipments are processed and the user confirms they want to process again add a shipment
                    if (shipments.All(s => s.Processed))
                    {
                        confirmedShipments = new[] { shipmentFactory.Create(orderId) };
                    }

                    // If some of the shipments are not process and the user confirms return only the unprocessed shipments
                    if (shipments.Any(s => !s.Processed))
                    {
                        confirmedShipments = shipments.Where(s => !s.Processed).ToArray();
                    }
                }

                if (singleScanAutomationSettings.IsAutoWeighEnabled && confirmedShipments.IsCountEqualTo(1))
                {
                    ShipmentEntity confirmedShipment = confirmedShipments.SingleOrDefault();
                    int packageCount = shipmentAdapterFactory.Get(confirmedShipment).GetPackageAdaptersAndEnsureShipmentIsLoaded().Count();

                    if (packageCount > 1 && !ShouldPrintAndProcessShipmentWithMultiplePackages(packageCount, scannedBarcode))
                    {
                        confirmedShipments = new ShipmentEntity[0];
                    }
                }
            }

            return confirmedShipments;
        }

        /// <summary>
        /// Determines whether any shipment has a ShipmentTypeCode of "None".
        /// </summary>
        private static bool HasDisqualifyingShipmentTypes(IEnumerable<ShipmentEntity> shipments) =>
            shipments.Any(shipment => shipment.ShipmentTypeCode == ShipmentTypeCode.None);
    }
}
