using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public interface IPlatformOnlineUpdaterBehavior
	{
		bool UseSwatId { get; }

		bool IncludeSalesOrderItems { get; }

		bool SetOrderStatusesOnShipmentNotify { get; }

		void SetOrderStatuses(OrderEntity order, UnitOfWork2 unitOfWork);
	}
}
