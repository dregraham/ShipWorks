using System;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public class DefaultPlatformOnlineUpdaterBehavior : IPlatformOnlineUpdaterBehavior
	{
		public virtual bool UseSwatId => false;

		public virtual bool IncludeSalesOrderItems => false;

		public virtual bool SetOrderStatusesOnShipmentNotify => false;
	}
}
