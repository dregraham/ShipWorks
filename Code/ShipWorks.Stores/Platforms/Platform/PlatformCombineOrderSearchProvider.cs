using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Platform
{
	/// <summary>
	/// Combined order search provider for Platform
	/// </summary>
	[Component]
	public class PlatformCombineOrderSearchProvider : CombineOrderSearchBaseProvider<string>, IPlatformOrderSearchProvider
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public PlatformCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
			base(sqlAdapterFactory)
		{

		}

		/// <summary>
		/// Gets the online store's order identifier
		/// </summary>
		protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
		{
			QueryFactory factory = new QueryFactory();

			var query = factory.OrderSearch
				.Select(() => OrderSearchFields.OriginalChannelOrderID.ToValue<string>())
				.Where(OrderSearchFields.OrderID == order.OrderID)
				.AndWhere(OrderSearchFields.IsManual == false);

			using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
			{
				return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Gets the Platform online order identifier
		/// </summary>
		protected override string GetOnlineOrderIdentifier(IOrderEntity order) =>
			order.ChannelOrderID ?? string.Empty;
	}
}
