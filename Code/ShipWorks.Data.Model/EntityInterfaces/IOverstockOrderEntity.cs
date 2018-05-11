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
    /// Entity interface which represents the entity 'OverstockOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOverstockOrderEntity: IOrderEntity
    {
        
        /// <summary> The OverstockOrderID property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."OverstockOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OverstockOrderID { get; }
        /// <summary> The ChannelName property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."ChannelName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ChannelName { get; }
        /// <summary> The WarehouseName property of the Entity OverstockOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OverstockOrder"."WarehouseName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String WarehouseName { get; }
        
        
        
        IEnumerable<IOverstockOrderSearchEntity> OverstockOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockOrderEntity AsReadOnlyOverstockOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOverstockOrderEntity AsReadOnlyOverstockOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OverstockOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class OverstockOrderEntity : IOverstockOrderEntity
    {
        
        
        IEnumerable<IOverstockOrderSearchEntity> IOverstockOrderEntity.OverstockOrderSearch => OverstockOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOverstockOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOverstockOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOverstockOrderEntity AsReadOnlyOverstockOrder() =>
            (IOverstockOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOverstockOrderEntity AsReadOnlyOverstockOrder(IDictionary<object, object> objectMap) =>
            (IOverstockOrderEntity) AsReadOnly(objectMap);
        
    }
}
