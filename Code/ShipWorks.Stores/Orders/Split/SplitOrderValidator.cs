using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.SplitOrder
{
    /// <summary>
    /// Validate the splitting of an order
    /// </summary>
    /// <remarks>
    /// Validation requirements:
    /// Number of orders is = 1
    /// User has permissions to modify the store orders
    /// No processed shipments
    /// </remarks>
    [Component]
    public class SplitOrderValidator : ISplitOrderValidator
    {
        private readonly ISecurityContext securityContext;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SplitOrderValidator(ISecurityContext securityContext, IOrderManager orderManager)
        {
            this.securityContext = securityContext;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Validate the list of orders
        /// </summary>
        public Result Validate(IEnumerable<long> orderIDs)
        {
            if (!orderIDs.IsCountEqualTo(1))
            {
                return Result.FromError("A single order must be selected to combine orders");
            }

            // All ids must be order ids, not customer
            if (!orderIDs.All(orderID => EntityUtility.GetEntityType(orderID) == EntityType.OrderEntity))
            {
                return Result.FromError("The selected row must be an order");
            }

            long firstId = orderIDs.First();
            bool hasPermission = securityContext.HasPermission(PermissionType.OrdersModify, firstId);

            if (!hasPermission)
            {
                return Result.FromError("The current user does not have permission to modify orders");
            }

            ShipmentEntity processedShipment = orderManager.GetLatestActiveShipment(firstId);

            bool canCombine = processedShipment == null;

            return canCombine == false ?
                Result.FromError("Selected order cannot be split as it has a processed shipment") :
                Result.FromSuccess();
        }
    }
}