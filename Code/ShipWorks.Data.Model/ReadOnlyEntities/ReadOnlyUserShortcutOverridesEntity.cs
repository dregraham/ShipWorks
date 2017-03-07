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
    /// Read-only representation of the entity 'UserShortcutOverrides'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUserShortcutOverridesEntity : IUserShortcutOverridesEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUserShortcutOverridesEntity(IUserShortcutOverridesEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UserShortcutOverrideID = source.UserShortcutOverrideID;
            UserID = source.UserID;
            CommandType = source.CommandType;
            Alt = source.Alt;
            Ctrl = source.Ctrl;
            Shift = source.Shift;
            KeyValue = source.KeyValue;
            
            
            User = source.User?.AsReadOnly(objectMap);
            

            CopyCustomUserShortcutOverridesData(source);
        }

        
        /// <summary> The UserShortcutOverrideID property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."UserShortcutOverrideID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UserShortcutOverrideID { get; }
        /// <summary> The UserID property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The CommandType property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."CommandType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public ShipWorks.Shared.IO.KeyboardShortcuts.KeyboardShortcutCommand CommandType { get; }
        /// <summary> The Alt property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Alt"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Alt { get; }
        /// <summary> The Ctrl property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Ctrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Ctrl { get; }
        /// <summary> The Shift property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."Shift"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Shift { get; }
        /// <summary> The KeyValue property of the Entity UserShortcutOverrides<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserShortcutOverride"."KeyValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String KeyValue { get; }
        
        
        public IUserEntity User { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserShortcutOverridesEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserShortcutOverridesEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUserShortcutOverridesData(IUserShortcutOverridesEntity source);
    }
}
