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
    /// Entity interface which represents the entity 'GrouponOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGrouponOrderSearchEntity
    {
        
        /// <summary> The GrouponOrderSearchID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."GrouponOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 GrouponOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The GrouponOrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."GrouponOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String GrouponOrderID { get; }
        /// <summary> The ParentOrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."ParentOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ParentOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity GrouponOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IGrouponOrderEntity GrouponOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGrouponOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGrouponOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GrouponOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class GrouponOrderSearchEntity : IGrouponOrderSearchEntity
    {
        
        IGrouponOrderEntity IGrouponOrderSearchEntity.GrouponOrder => GrouponOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGrouponOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IGrouponOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGrouponOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGrouponOrderSearchEntity(this, objectMap);
        }
    }
}
