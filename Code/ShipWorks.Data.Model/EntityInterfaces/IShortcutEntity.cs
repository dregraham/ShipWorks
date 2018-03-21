﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'Shortcut'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShortcutEntity
    {
        
        /// <summary> The ShortcutID property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."ShortcutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShortcutID { get; }
        /// <summary> The RowVersion property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ModifierKeys property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."ModifierKeys"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ModifierKeys { get; }
        /// <summary> The VirtualKey property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."VirtualKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> VirtualKey { get; }
        /// <summary> The Barcode property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."Barcode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Barcode { get; }
        /// <summary> The Action property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."Action"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Action { get; }
        /// <summary> The RelatedObjectID property of the Entity Shortcut<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shortcut"."RelatedObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> RelatedObjectID { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShortcutEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShortcutEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Shortcut'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShortcutEntity : IShortcutEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShortcutEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShortcutEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShortcutEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShortcutEntity(this, objectMap);
        }

        
    }
}
