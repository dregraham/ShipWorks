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
    /// Read-only representation of the entity 'OverstockOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOverstockOrderEntity : ReadOnlyOrderEntity, IOverstockOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOverstockOrderEntity(IOverstockOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            WarehouseCode = source.WarehouseCode;
            SalesChannelName = source.SalesChannelName;
            SofsCreatedDate = source.SofsCreatedDate;
            
            
            
            OverstockOrderSearch = source.OverstockOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IOverstockOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IOverstockOrderSearchEntity>();

            CopyCustomOverstockOrderData(source);
        }

        
        /// <summary> The WarehouseCode property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."WarehouseCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String WarehouseCode { get; }
        /// <summary> The SalesChannelName property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."SalesChannelName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SalesChannelName { get; }
        /// <summary> The SofsCreatedDate property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."SofsCreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime SofsCreatedDate { get; }
        
        
        
        public IEnumerable<IOverstockOrderSearchEntity> OverstockOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOverstockOrderEntity AsReadOnlyOverstockOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOverstockOrderEntity AsReadOnlyOverstockOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOverstockOrderData(IOverstockOrderEntity source);
    }
}
