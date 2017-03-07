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
    /// Entity interface which represents the entity 'UserShortcutOverrides'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUserShortcutOverridesEntity
    {
        
        /// <summary> The UserShortcutOverrideID property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."UserShortcutOverrideID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UserShortcutOverrideID { get; }
        /// <summary> The UserID property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The CommandType property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."CommandType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        ShipWorks.Shared.IO.KeyboardShortcuts.KeyboardShortcutCommand CommandType { get; }
        /// <summary> The Alt property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Alt"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Alt { get; }
        /// <summary> The Ctrl property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Ctrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Ctrl { get; }
        /// <summary> The Shift property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Shift"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Shift { get; }
        /// <summary> The KeyValue property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."KeyValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String KeyValue { get; }
        
        
        IUserEntity User { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserShortcutOverridesEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserShortcutOverridesEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UserShortcutOverrides'. <br/><br/>
    /// 
    /// </summary>
    public partial class UserShortcutOverridesEntity : IUserShortcutOverridesEntity
    {
        
        IUserEntity IUserShortcutOverridesEntity.User => User;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserShortcutOverridesEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUserShortcutOverridesEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUserShortcutOverridesEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUserShortcutOverridesEntity(this, objectMap);
        }
    }
}
