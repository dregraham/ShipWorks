using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Orders.Split
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
    public class OrderSplitValidator : ISplitOrderValidator
    {
        private readonly ISecurityContext securityContext;
        private readonly IOrderSplitGateway orderSplitGateway;
        private readonly IStoreManager storeManager;
        private readonly IEnumerable<StoreTypeCode> storeTypeCodesNotAllowed =
            new[] {StoreTypeCode.Walmart, StoreTypeCode.Groupon, StoreTypeCode.NeweggMarketplace, StoreTypeCode.Sears, StoreTypeCode.Overstock};

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitValidator(ISecurityContext securityContext, IOrderSplitGateway orderSplitGateway, IStoreManager storeManager)
        {
            this.securityContext = securityContext;
            this.orderSplitGateway = orderSplitGateway;
            this.storeManager = storeManager;
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

            StoreEntity store = storeManager.GetRelatedStore(firstId);

            if (store == null)
            {
                return Result.FromError($"This order's store has been deleted, order cannot be split");
            }

            if (storeTypeCodesNotAllowed.Contains(store.StoreTypeCode))
            {
                return Result.FromError($"{ EnumHelper.GetDescription(store.StoreTypeCode)} orders cannot be split");
            }

            return orderSplitGateway.CanSplit(firstId) == false ?
                Result.FromError("Selected order cannot be split as it has a processed shipment") :
                Result.FromSuccess();
        }
    }
}