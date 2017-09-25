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
    /// Entity interface which represents the entity 'GrouponOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGrouponOrderEntity: IOrderEntity
    {
        
        /// <summary> The GrouponOrderID property of the Entity GrouponOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrder"."GrouponOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String GrouponOrderID { get; }
        /// <summary> The ParentOrderID property of the Entity GrouponOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrder"."ParentOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ParentOrderID { get; }
        
        
        
        IEnumerable<IGrouponOrderSearchEntity> GrouponOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGrouponOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGrouponOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GrouponOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class GrouponOrderEntity : IGrouponOrderEntity
    {
        
        
        IEnumerable<IGrouponOrderSearchEntity> IGrouponOrderEntity.GrouponOrderSearch => GrouponOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IGrouponOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IGrouponOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGrouponOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGrouponOrderEntity(this, objectMap);
        }
    }
}
