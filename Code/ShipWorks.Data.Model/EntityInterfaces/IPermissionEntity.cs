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
    /// Entity interface which represents the entity 'Permission'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPermissionEntity
    {
        
        /// <summary> The PermissionID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."PermissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 PermissionID { get; }
        /// <summary> The UserID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The PermissionType property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."PermissionType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PermissionType { get; }
        /// <summary> The ObjectID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ObjectID { get; }
        
        
        IUserEntity User { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPermissionEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPermissionEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Permission'. <br/><br/>
    /// 
    /// </summary>
    public partial class PermissionEntity : IPermissionEntity
    {
        
        IUserEntity IPermissionEntity.User => User;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPermissionEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPermissionEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPermissionEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPermissionEntity(this, objectMap);
        }
    }
}
