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
    /// Entity interface which represents the entity 'WorldShipGoods'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWorldShipGoodsEntity : IWorldShipGoodsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWorldShipGoodsEntity(IWorldShipGoodsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            WorldShipGoodsID = source.WorldShipGoodsID;
            ShipmentID = source.ShipmentID;
            ShipmentCustomsItemID = source.ShipmentCustomsItemID;
            Description = source.Description;
            TariffCode = source.TariffCode;
            CountryOfOrigin = source.CountryOfOrigin;
            Units = source.Units;
            UnitOfMeasure = source.UnitOfMeasure;
            UnitPrice = source.UnitPrice;
            Weight = source.Weight;
            InvoiceCurrencyCode = source.InvoiceCurrencyCode;
            
            
            

            CopyCustomWorldShipGoodsData(source);
        }

        
        /// <summary> The WorldShipGoodsID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."WorldShipGoodsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 WorldShipGoodsID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The ShipmentCustomsItemID property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."ShipmentCustomsItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentCustomsItemID { get; }
        /// <summary> The Description property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The TariffCode property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."TariffCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TariffCode { get; }
        /// <summary> The CountryOfOrigin property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryOfOrigin { get; }
        /// <summary> The Units property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Units"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Units { get; }
        /// <summary> The UnitOfMeasure property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."UnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UnitOfMeasure { get; }
        /// <summary> The UnitPrice property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."UnitPrice"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal UnitPrice { get; }
        /// <summary> The Weight property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The InvoiceCurrencyCode property of the Entity WorldShipGoods<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipGoods"."InvoiceCurrencyCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InvoiceCurrencyCode { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipGoodsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipGoodsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWorldShipGoodsData(IWorldShipGoodsEntity source);
    }
}
