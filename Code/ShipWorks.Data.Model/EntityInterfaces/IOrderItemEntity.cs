﻿///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'OrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderItemEntity
    {
        
        /// <summary> The OrderItemID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."OrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderItemID { get; }
        /// <summary> The RowVersion property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The Name property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Code property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Code"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Code { get; }
        /// <summary> The SKU property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SKU { get; }
        /// <summary> The ISBN property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."ISBN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ISBN { get; }
        /// <summary> The UPC property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UPC"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UPC { get; }
        /// <summary> The Description property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The Location property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Location"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Location { get; }
        /// <summary> The Image property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Image"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Image { get; }
        /// <summary> The Thumbnail property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Thumbnail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Thumbnail { get; }
        /// <summary> The UnitPrice property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitPrice { get; }
        /// <summary> The UnitCost property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UnitCost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitCost { get; }
        /// <summary> The Weight property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The Quantity property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Quantity { get; }
        /// <summary> The LocalStatus property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."LocalStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LocalStatus { get; }
        /// <summary> The IsManual property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsManual { get; }
        /// <summary> The HarmonizedCode property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."HarmonizedCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HarmonizedCode { get; }
        /// <summary> The OriginalOrderID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        /// <summary> The Length property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Length { get; }
        /// <summary> The Width property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Width { get; }
        /// <summary> The Brand property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Brand"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Brand { get; }
        /// <summary> The Height property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Height { get; }
        /// <summary> The Custom1 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom1 { get; }
        /// <summary> The MPN property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."MPN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MPN { get; }
        /// <summary> The Custom2 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom2 { get; }
        /// <summary> The Custom3 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom3 { get; }
        /// <summary> The Custom4 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom4"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom4 { get; }
        /// <summary> The Custom5 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom5"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom5 { get; }
        /// <summary> The Custom6 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom6"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom6 { get; }
        /// <summary> The HubItemID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."HubItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String HubItemID { get; }
        /// <summary> The Custom7 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom7"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom7 { get; }
        /// <summary> The Custom8 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom8"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom8 { get; }
        /// <summary> The Custom9 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom9"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom9 { get; }
        /// <summary> The Custom10 property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Custom10"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Custom10 { get; }
        /// <summary> The StoreOrderItemID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."StoreOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String StoreOrderItemID { get; }
        
        
        IOrderEntity Order { get; }
        
        IEnumerable<IOrderItemAttributeEntity> OrderItemAttributes { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderItemEntity : IOrderItemEntity
    {
        
        IOrderEntity IOrderItemEntity.Order => Order;
        
        IEnumerable<IOrderItemAttributeEntity> IOrderItemEntity.OrderItemAttributes => OrderItemAttributes;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderItemEntity(this, objectMap);
        }

        
    }
}
