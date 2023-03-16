using System;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public interface IPlatformOnlineUpdaterBehavior
	{
		bool UseSwatId { get; }

		bool IncludeSalesOrderItems { get; }

		bool SetOrderStatusesOnShipmentNotify { get; }
	}
}
