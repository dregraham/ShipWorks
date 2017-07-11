using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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
        private readonly ICombineOrdersGateway gateway;
        private readonly ISecurityContext securityContext;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombineValidator(ICombineOrdersGateway gateway, ISecurityContext securityContext, IStoreManager storeManager)
        {
            this.gateway = gateway;
            this.securityContext = securityContext;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Validate the list of orders
        /// </summary>
        public Result Validate(IEnumerable<long> orderIDs)
        {

            if (orderIDs.None() || orderIDs.Count() < 2)
            {
                return Result.FromError("A minimum of two orders must be selected to combine orders");
            }

            bool hasPermission = securityContext.HasPermission(PermissionType.OrdersModify, orderIDs.First());

            if (!hasPermission)
            {
                return Result.FromError("The current user does not have permission to modify orders");
            }

            IStoreEntity storeEntity = storeManager.GetRelatedStore(orderIDs.First());
            Task<bool> loadResult = gateway.CanCombine(storeEntity, orderIDs);

            if (loadResult.Result == false)
            {
                return Result.FromError("Selected orders cannot be combined");
            }

            return Result.FromSuccess();
        }
    }
}