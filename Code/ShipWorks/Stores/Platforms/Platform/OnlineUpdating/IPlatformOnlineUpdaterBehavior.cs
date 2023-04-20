using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public interface IPlatformOnlineUpdaterBehavior
	{
		bool UseSwatId { get; }

		bool IncludeSalesOrderItems { get; }

		bool SetOrderStatusesOnShipmentNotify { get; }

		void SetOrderStatuses(OrderEntity order, UnitOfWork2 unitOfWork);

        string GetCarrierName(IShippingManager shippingManager, ShipmentEntity shipment);

        string GetTrackingUrl(ShipmentEntity shipment);
    }
}
