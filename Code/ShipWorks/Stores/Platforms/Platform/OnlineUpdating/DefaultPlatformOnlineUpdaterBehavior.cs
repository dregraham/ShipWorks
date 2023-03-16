using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public class DefaultPlatformOnlineUpdaterBehavior : IPlatformOnlineUpdaterBehavior
	{
		public virtual bool UseSwatId => false;

		public virtual bool IncludeSalesOrderItems => false;

		public virtual bool SetOrderStatusesOnShipmentNotify => false;

		public virtual void SetOrderStatuses(OrderEntity order, UnitOfWork2 unitOfWork)
		{
			order.OnlineStatus = OrderSourceSalesOrderStatus.Completed.ToString();

			unitOfWork.AddForSave(order);
		}
	}
}
