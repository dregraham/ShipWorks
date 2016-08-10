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
    /// Read-only representation of the entity 'Permission'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPermissionEntity : IPermissionEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPermissionEntity(IPermissionEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PermissionID = source.PermissionID;
            UserID = source.UserID;
            PermissionType = source.PermissionType;
            ObjectID = source.ObjectID;
            
            
            User = source.User?.AsReadOnly(objectMap);
            

            CopyCustomPermissionData(source);
        }

        
        /// <summary> The PermissionID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."PermissionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 PermissionID { get; }
        /// <summary> The UserID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The PermissionType property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."PermissionType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PermissionType { get; }
        /// <summary> The ObjectID property of the Entity Permission<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Permission"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ObjectID { get; }
        
        
        public IUserEntity User { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPermissionEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPermissionEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPermissionData(IPermissionEntity source);
    }
}
