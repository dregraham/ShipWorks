///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity interface which represents the entity 'OrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderItemEntity : IOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderItemEntity(IOrderItemEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderItemID = source.OrderItemID;
            RowVersion = source.RowVersion;
            OrderID = source.OrderID;
            Name = source.Name;
            Code = source.Code;
            SKU = source.SKU;
            ISBN = source.ISBN;
            UPC = source.UPC;
            Description = source.Description;
            Location = source.Location;
            Image = source.Image;
            Thumbnail = source.Thumbnail;
            UnitPrice = source.UnitPrice;
            UnitCost = source.UnitCost;
            Weight = source.Weight;
            Quantity = source.Quantity;
            LocalStatus = source.LocalStatus;
            IsManual = source.IsManual;
            
            
            Order = source.Order?.AsReadOnly(objectMap);
            
            OrderItemAttributes = source.OrderItemAttributes?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderItemAttributeEntity>();

            CopyCustomOrderItemData(source);
        }

        
        /// <summary> The OrderItemID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."OrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderItemID { get; }
        /// <summary> The RowVersion property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The Name property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Code property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Code"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Code { get; }
        /// <summary> The SKU property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SKU { get; }
        /// <summary> The ISBN property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."ISBN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ISBN { get; }
        /// <summary> The UPC property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UPC"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UPC { get; }
        /// <summary> The Description property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The Location property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Location"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Location { get; }
        /// <summary> The Image property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Image"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Image { get; }
        /// <summary> The Thumbnail property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Thumbnail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Thumbnail { get; }
        /// <summary> The UnitPrice property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitPrice { get; }
        /// <summary> The UnitCost property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."UnitCost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitCost { get; }
        /// <summary> The Weight property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The Quantity property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Quantity { get; }
        /// <summary> The LocalStatus property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."LocalStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LocalStatus { get; }
        /// <summary> The IsManual property of the Entity OrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderItem"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsManual { get; }
        
        
        public IOrderEntity Order { get; }
        
        
        public IEnumerable<IOrderItemAttributeEntity> OrderItemAttributes { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderItemData(IOrderItemEntity source);
    }
}
