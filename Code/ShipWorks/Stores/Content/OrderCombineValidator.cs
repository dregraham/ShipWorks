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

        public Result Validate(IEnumerable<long> orderIDs)
        {
            if (orderIDs.None())
            {
                return Result.FromSuccess();
            }
            
            IStoreEntity storeEntity = storeManager.GetRelatedStore(orderIDs.First());
            Task<bool> loadResult = gateway.CanCombine(storeEntity, orderIDs);
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
