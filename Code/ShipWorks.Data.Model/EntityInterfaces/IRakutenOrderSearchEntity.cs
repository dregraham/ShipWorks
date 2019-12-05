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
    /// Entity interface which represents the entity 'RakutenOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IRakutenOrderSearchEntity
    {
        
        /// <summary> The RakutenOrderSearchID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."RakutenOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 RakutenOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        /// <summary> The RakutenPackageID property of the Entity RakutenOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderSearch"."RakutenPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 36<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String RakutenPackageID { get; }
        
        
        IRakutenOrderEntity RakutenOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'RakutenOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class RakutenOrderSearchEntity : IRakutenOrderSearchEntity
    {
        
        IRakutenOrderEntity IRakutenOrderSearchEntity.RakutenOrder => RakutenOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IRakutenOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IRakutenOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IRakutenOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyRakutenOrderSearchEntity(this, objectMap);
        }

        
    }
}
