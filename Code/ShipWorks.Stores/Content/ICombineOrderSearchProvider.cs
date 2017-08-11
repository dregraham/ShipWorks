//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using Interapptive.Shared.ComponentRegistration;
//using SD.LLBLGen.Pro.ORMSupportClasses;
//using SD.LLBLGen.Pro.QuerySpec;
//using ShipWorks.ApplicationCore;
//using ShipWorks.Data.Connection;
//using ShipWorks.Data.Model.EntityClasses;
//using ShipWorks.Data.Model.EntityInterfaces;
//using ShipWorks.Data.Model.FactoryClasses;
//using ShipWorks.Data.Model.HelperClasses;
//using Interapptive.Shared.Enums;

//namespace ShipWorks.Stores.Content
//{
//    public interface ICombineOrderSearchProvider<TResult>
//    {
//        Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);

//        Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order);

//        TResult GetOnlineOrderIdentifier(IOrderEntity order);
//    }


//    public abstract class CombineOrderSearchBaseProvider<TResult> : ICombineOrderSearchProvider<TResult>
//    {
//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        /// <typeparam name="TResult">Return type of the selectExpression</typeparam>
//        /// <param name="order">The order for which to find combined order identifiers</param>
//        /// <param name="searchTableName">Name of the table, i.e. "OrderSearch"</param>
//        /// <param name="predicate">Where clause predicate, i.e. OrderSearchFields.OrderID == order.OrderID</param>
//        /// <param name="selectExpression">What to return, i.e. , () => OrderSearchFields.OrderNumber.ToValue<long>()</param>
//        /// <returns></returns>
//        public virtual async Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(
//            OrderEntity order,
//            string searchTableName,
//            IPredicate predicate,
//            Expression<Func<TResult>> selectExpression)
//        {
//            QueryFactory factory = new QueryFactory();
//            var query = factory.Create(searchTableName)
//                .Select(selectExpression)
//                .Where(predicate);

//            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
//            {
//                using (ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create())
//                {
//                    return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
//                }
//            }
//        }

//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        /// <param name="order">The order for which to find combined order identifiers</param>
//        //public abstract Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);

//        public abstract Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);

//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        public abstract TResult GetOnlineOrderIdentifier(IOrderEntity order);

//        ///// <summary>
//        ///// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
//        ///// combined orders.
//        ///// </summary>
//        //public abstract Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order);


//        /// <summary>
//        /// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
//        /// combined orders.
//        /// </summary>
//        public virtual async Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order)
//        {
//            return order.CombineSplitStatus == CombineSplitStatusType.Combined ?
//                await GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
//                new[] { GetOnlineOrderIdentifier(order) };
//        }
//    }



//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Amosoft)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Brightpearl)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Cart66Lite)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Cart66Pro)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.ChannelSale)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Choxi)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.CloudConversion)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.CreLoaded)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.CsCart)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Fortune3)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.GeekSeller)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.GenericModule)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.InfiPlex)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.InstaStore)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Jigoshop)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.LimeLightCRM)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.LiveSite)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.LoadedCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.nopCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.OpenCart)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.OpenSky)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.OrderBot)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.OrderDesk)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.OrderDynamics)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.osCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.PowersportsSupport)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.PrestaShop)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.RevolutionParts)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SearchFit)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SellerActive)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SellerCloud)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SellerExpress)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SellerVantage)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Shopp)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Shopperpress)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SolidCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.StageBloc)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.SureDone)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.VirtueMart)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.WebShopManager)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.WooCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.WPeCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.XCart)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.ZenCart)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.Zenventory)]
//    public class CombineOrderNumberCompleteSearchProvider : CombineOrderSearchBaseProvider<string>
//    {
//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        /// <param name="order">The order for which to find combined order identifiers</param>
//        public override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
//        {
//            return await GetCombinedOnlineOrderIdentifiers(order as OrderEntity, "OrderSearch",
//                OrderSearchFields.OrderID == order.OrderID, () => OrderSearchFields.OrderNumberComplete.ToValue<string>()).ConfigureAwait(false);
//        }

//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        public override string GetOnlineOrderIdentifier(IOrderEntity order) => order.OrderNumberComplete;

//        ///// <summary>
//        ///// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
//        ///// combined orders.
//        ///// </summary>
//        //public override async Task<IEnumerable<string>> GetOrderIdentifiers(IOrderEntity order)
//        //{
//        //    return order.CombineSplitStatus == CombineSplitStatusType.Combined ?
//        //        await GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
//        //        new[] { GetOnlineOrderIdentifier(order) };
//        //}
//    }

//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.AmeriCommerce)]
//    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.ChannelAdvisor)]
//    public class CombineOrderNumberSearchProvider : CombineOrderSearchBaseProvider<long>
//    {
//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        /// <param name="order">The order for which to find combined order identifiers</param>
//        public override async Task<IEnumerable<long>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
//        {
//            return await GetCombinedOnlineOrderIdentifiers(order as OrderEntity, "OrderSearch",
//                OrderSearchFields.OrderID == order.OrderID, () => OrderSearchFields.OrderNumber.ToValue<long>()).ConfigureAwait(false);
//        }

//        /// <summary>
//        /// Gets the online store's order identifier
//        /// </summary>
//        public override long GetOnlineOrderIdentifier(IOrderEntity order) => order.OrderNumber;

//        ///// <summary>
//        ///// Get the order identifier(s) for the given order.  Multiple will be returned in the case of
//        ///// combined orders.
//        ///// </summary>
//        //public override async Task<IEnumerable<long>> GetOrderIdentifiers(IOrderEntity order)
//        //{
//        //    return order.CombineSplitStatus == CombineSplitStatusType.Combined ?
//        //        await GetCombinedOnlineOrderIdentifiers(order).ConfigureAwait(false) :
//        //        new[] { GetOnlineOrderIdentifier(order) };
//        //}
//    }
//}
