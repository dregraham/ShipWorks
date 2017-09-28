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
    /// Entity interface which represents the entity 'StatusPreset'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IStatusPresetEntity
    {
        
        /// <summary> The StatusPresetID property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusPresetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 StatusPresetID { get; }
        /// <summary> The RowVersion property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> StoreID { get; }
        /// <summary> The StatusTarget property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 StatusTarget { get; }
        /// <summary> The StatusText property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StatusText { get; }
        /// <summary> The IsDefault property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."IsDefault"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsDefault { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IStatusPresetEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IStatusPresetEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'StatusPreset'. <br/><br/>
    /// 
    /// </summary>
    public partial class StatusPresetEntity : IStatusPresetEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStatusPresetEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IStatusPresetEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IStatusPresetEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyStatusPresetEntity(this, objectMap);
        }

        
    }
}
