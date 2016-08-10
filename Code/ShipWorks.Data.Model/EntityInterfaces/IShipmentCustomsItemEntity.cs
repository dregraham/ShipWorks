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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ShipmentCustomsItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShipmentCustomsItemEntity
    {
        
        /// <summary> The ShipmentCustomsItemID property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."ShipmentCustomsItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShipmentCustomsItemID { get; }
        /// <summary> The RowVersion property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentID property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Description property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The Quantity property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Quantity { get; }
        /// <summary> The Weight property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The UnitValue property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."UnitValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitValue { get; }
        /// <summary> The CountryOfOrigin property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryOfOrigin { get; }
        /// <summary> The HarmonizedCode property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."HarmonizedCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 14<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String HarmonizedCode { get; }
        /// <summary> The NumberOfPieces property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."NumberOfPieces"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 NumberOfPieces { get; }
        /// <summary> The UnitPriceAmount property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."UnitPriceAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitPriceAmount { get; }
        
        
        IShipmentEntity Shipment { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentCustomsItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentCustomsItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShipmentCustomsItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShipmentCustomsItemEntity : IShipmentCustomsItemEntity
    {
        
        IShipmentEntity IShipmentCustomsItemEntity.Shipment => Shipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentCustomsItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShipmentCustomsItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShipmentCustomsItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShipmentCustomsItemEntity(this, objectMap);
        }
    }
}
