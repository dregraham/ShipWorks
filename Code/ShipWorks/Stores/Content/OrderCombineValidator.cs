using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Validate the combination of orders
    /// </summary>
    /// <remarks>
    /// Validation requirements:
    /// Number of orders is > 1
    /// User has permissions to modify the store orders
    /// </remarks>
    [Component]
    public class OrderCombineValidator : IOrderCombineValidator
    {
        readonly ICombineOrdersGateway gateway;
        readonly ISecurityContext securityContext;
        readonly IStoreEntity storeEntity;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombineValidator(ICombineOrdersGateway gateway, 
            ISecurityContext securityContext,
            IStoreEntity storeEntity)
        {
            this.gateway = gateway;
            this.securityContext = securityContext;
            this.storeEntity = storeEntity;
        }

        public Result Validate(IEnumerable<long> orderIDs)
        {
            var loadResult = gateway.CanCombine(storeEntity, orderIDs);
            bool hasPermission = securityContext.HasPermission(PermissionType.OrdersModify, orderIDs.First());

            if (orderIDs.Count() < 2)
            {
                return Result.FromError("Only two or more orders can be combined");
            }

            if (!hasPermission)
            {
                return Result.FromError("User does not have permission to modify orders");
            }

            if (loadResult.Result == false)
            {
                return Result.FromError("Orders can't be combined");
            }

            return Result.FromSuccess();


        }
    }
}
