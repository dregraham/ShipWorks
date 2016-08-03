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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'WorldShipGoods'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWorldShipGoodsEntity
    {
        
        /// <summary> The WorldShipGoodsID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."WorldShipGoodsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 WorldShipGoodsID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The ShipmentCustomsItemID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."ShipmentCustomsItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentCustomsItemID { get; }
        /// <summary> The Description property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The TariffCode property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."TariffCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TariffCode { get; }
        /// <summary> The CountryOfOrigin property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CountryOfOrigin { get; }
        /// <summary> The Units property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Units"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Units { get; }
        /// <summary> The UnitOfMeasure property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."UnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UnitOfMeasure { get; }
        /// <summary> The UnitPrice property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal UnitPrice { get; }
        /// <summary> The Weight property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The InvoiceCurrencyCode property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."InvoiceCurrencyCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InvoiceCurrencyCode { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipGoodsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipGoodsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WorldShipGoods'. <br/><br/>
    /// 
    /// </summary>
    public partial class WorldShipGoodsEntity : IWorldShipGoodsEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipGoodsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IWorldShipGoodsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWorldShipGoodsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWorldShipGoodsEntity(this, objectMap);
        }
    }
}
