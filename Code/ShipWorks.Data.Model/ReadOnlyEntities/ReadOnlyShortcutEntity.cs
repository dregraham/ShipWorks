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
    /// Read-only representation of the entity 'Shortcut'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShortcutEntity : IShortcutEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShortcutEntity(IShortcutEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShortcutID = source.ShortcutID;
            RowVersion = source.RowVersion;
            ModifierKeys = source.ModifierKeys;
            VirtualKey = source.VirtualKey;
            Barcode = source.Barcode;
            Action = source.Action;
            RelatedObjectID = source.RelatedObjectID;
            
            
            

            CopyCustomShortcutData(source);
        }

        
        /// <summary> The ShortcutID property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."ShortcutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShortcutID { get; }
        /// <summary> The RowVersion property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ModifierKeys property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."ModifierKeys"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ModifierKeys { get; }
        /// <summary> The VirtualKey property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."VirtualKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> VirtualKey { get; }
        /// <summary> The Barcode property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."Barcode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Barcode { get; }
        /// <summary> The Action property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."Action"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Action { get; }
        /// <summary> The RelatedObjectID property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."RelatedObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> RelatedObjectID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShortcutEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShortcutEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShortcutData(IShortcutEntity source);
    }
}
