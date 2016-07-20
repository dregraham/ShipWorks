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
    /// Read-only representation of the entity 'BuyDotComOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyBuyDotComOrderItemEntity : ReadOnlyOrderItemEntity, IBuyDotComOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyBuyDotComOrderItemEntity(IBuyDotComOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ReceiptItemID = source.ReceiptItemID;
            ListingID = source.ListingID;
            Shipping = source.Shipping;
            Tax = source.Tax;
            Commission = source.Commission;
            ItemFee = source.ItemFee;
            
            
            

            CopyCustomBuyDotComOrderItemData(source);
        }

        
        /// <summary> The ReceiptItemID property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ReceiptItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ReceiptItemID { get; }
        /// <summary> The ListingID property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ListingID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ListingID { get; }
        /// <summary> The Shipping property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Shipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Shipping { get; }
        /// <summary> The Tax property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Tax"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Tax { get; }
        /// <summary> The Commission property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Commission { get; }
        /// <summary> The ItemFee property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ItemFee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal ItemFee { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBuyDotComOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBuyDotComOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomBuyDotComOrderItemData(IBuyDotComOrderItemEntity source);
    }
}
