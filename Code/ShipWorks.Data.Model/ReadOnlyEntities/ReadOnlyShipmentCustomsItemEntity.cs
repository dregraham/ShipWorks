///////////////////////////////////////////////////////////////
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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'ShipmentCustomsItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShipmentCustomsItemEntity : IShipmentCustomsItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShipmentCustomsItemEntity(IShipmentCustomsItemEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentCustomsItemID = source.ShipmentCustomsItemID;
            RowVersion = source.RowVersion;
            ShipmentID = source.ShipmentID;
            Description = source.Description;
            Quantity = source.Quantity;
            Weight = source.Weight;
            UnitValue = source.UnitValue;
            CountryOfOrigin = source.CountryOfOrigin;
            HarmonizedCode = source.HarmonizedCode;
            NumberOfPieces = source.NumberOfPieces;
            UnitPriceAmount = source.UnitPriceAmount;
            
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            

            CopyCustomShipmentCustomsItemData(source);
        }

        
        /// <summary> The ShipmentCustomsItemID property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."ShipmentCustomsItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShipmentCustomsItemID { get; }
        /// <summary> The RowVersion property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentID property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Description property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The Quantity property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Quantity { get; }
        /// <summary> The Weight property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The UnitValue property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."UnitValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitValue { get; }
        /// <summary> The CountryOfOrigin property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryOfOrigin { get; }
        /// <summary> The HarmonizedCode property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."HarmonizedCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String HarmonizedCode { get; }
        /// <summary> The NumberOfPieces property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."NumberOfPieces"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NumberOfPieces { get; }
        /// <summary> The UnitPriceAmount property of the Entity ShipmentCustomsItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipmentCustomsItem"."UnitPriceAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitPriceAmount { get; }
        
        
        public IShipmentEntity Shipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentCustomsItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentCustomsItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShipmentCustomsItemData(IShipmentCustomsItemEntity source);
    }
}
