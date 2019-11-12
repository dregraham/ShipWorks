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
    /// Entity interface which represents the entity 'RakutenOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IRakutenOrderEntity: IOrderEntity
    {
        
        /// <summary> The RakutenOrderID property of the Entity RakutenOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrder"."RakutenOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String RakutenOrderID { get; }
        
        
        
        IEnumerable<IRakutenOrderSearchEntity> RakutenOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderEntity AsReadOnlyRakutenOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderEntity AsReadOnlyRakutenOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'RakutenOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class RakutenOrderEntity : IRakutenOrderEntity
    {
        
        
        IEnumerable<IRakutenOrderSearchEntity> IRakutenOrderEntity.RakutenOrderSearch => RakutenOrderSearch;

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
                return (IRakutenOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyRakutenOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IRakutenOrderEntity AsReadOnlyRakutenOrder() =>
            (IRakutenOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IRakutenOrderEntity AsReadOnlyRakutenOrder(IDictionary<object, object> objectMap) =>
            (IRakutenOrderEntity) AsReadOnly(objectMap);
        
    }
}
