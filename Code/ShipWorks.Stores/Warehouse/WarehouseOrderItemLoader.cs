using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    public class WarehouseOrderItemLoader : IWarehouseOrderItemLoader
    {
        public void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseOrderItem)
        {
            itemEntity.Name = warehouseOrderItem.Name;
            itemEntity.Code = warehouseOrderItem.Code;
            itemEntity.SKU = warehouseOrderItem.SKU;
            itemEntity.ISBN = warehouseOrderItem.ISBN;
            itemEntity.UPC = warehouseOrderItem.UPC;
            itemEntity.HarmonizedCode = warehouseOrderItem.HarmonizedCode;
            itemEntity.Brand = warehouseOrderItem.Brand;
            itemEntity.MPN = warehouseOrderItem.MPN;
            itemEntity.Description = warehouseOrderItem.Description;
            itemEntity.Location = warehouseOrderItem.Location;
            itemEntity.Image = warehouseOrderItem.Image;
            itemEntity.Thumbnail = warehouseOrderItem.Thumbnail;
            itemEntity.UnitPrice = warehouseOrderItem.UnitPrice;
            itemEntity.UnitCost = warehouseOrderItem.UnitCost;
            itemEntity.Quantity = warehouseOrderItem.Quantity;
            itemEntity.Weight = warehouseOrderItem.Weight;
            itemEntity.Length = warehouseOrderItem.Length;
            itemEntity.Width = warehouseOrderItem.Width;
            itemEntity.Height = warehouseOrderItem.Height;

            LoadItemAttributes(itemEntity, warehouseOrderItem);
        }

        private static void LoadItemAttributes(OrderItemEntity itemEntity, WarehouseOrderItem warehouseOrderItem)
        {
            foreach (WarehouseOrderItemAttribute warehouseItemAttribute in warehouseOrderItem.ItemAttributes)
            {
                // todo: instantiate attribute
                OrderItemAttributeEntity itemAttributeEntity = new OrderItemAttributeEntity(itemEntity.OrderItemID);
                itemAttributeEntity.Name = warehouseItemAttribute.Name;
                itemAttributeEntity.Description = warehouseItemAttribute.Description;
                itemAttributeEntity.UnitPrice = warehouseItemAttribute.UnitPrice;
            }
        }
    }
}