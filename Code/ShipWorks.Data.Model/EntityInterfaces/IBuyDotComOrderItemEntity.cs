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
    /// Entity interface which represents the entity 'BuyDotComOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IBuyDotComOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The ReceiptItemID property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ReceiptItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ReceiptItemID { get; }
        /// <summary> The ListingID property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ListingID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ListingID { get; }
        /// <summary> The Shipping property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Shipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Shipping { get; }
        /// <summary> The Tax property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Tax"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Tax { get; }
        /// <summary> The Commission property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Commission { get; }
        /// <summary> The ItemFee property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."ItemFee"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal ItemFee { get; }
        /// <summary> The OriginalOrderID property of the Entity BuyDotComOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BuyDotComOrderItem"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBuyDotComOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IBuyDotComOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'BuyDotComOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class BuyDotComOrderItemEntity : IBuyDotComOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IBuyDotComOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IBuyDotComOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IBuyDotComOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyBuyDotComOrderItemEntity(this, objectMap);
        }
    }
}
