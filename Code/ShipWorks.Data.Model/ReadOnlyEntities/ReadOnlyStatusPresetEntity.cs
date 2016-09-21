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
    /// Read-only representation of the entity 'StatusPreset'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyStatusPresetEntity : IStatusPresetEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyStatusPresetEntity(IStatusPresetEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StatusPresetID = source.StatusPresetID;
            RowVersion = source.RowVersion;
            StoreID = source.StoreID;
            StatusTarget = source.StatusTarget;
            StatusText = source.StatusText;
            IsDefault = source.IsDefault;
            
            
            

            CopyCustomStatusPresetData(source);
        }

        
        /// <summary> The StatusPresetID property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusPresetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 StatusPresetID { get; }
        /// <summary> The RowVersion property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> StoreID { get; }
        /// <summary> The StatusTarget property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusTarget"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 StatusTarget { get; }
        /// <summary> The StatusText property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."StatusText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StatusText { get; }
        /// <summary> The IsDefault property of the Entity StatusPreset<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "StatusPreset"."IsDefault"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsDefault { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStatusPresetEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IStatusPresetEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomStatusPresetData(IStatusPresetEntity source);
    }
}
