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
    /// Read-only representation of the entity 'GridColumnFormat'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGridColumnFormatEntity : IGridColumnFormatEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGridColumnFormatEntity(IGridColumnFormatEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            GridColumnFormatID = source.GridColumnFormatID;
            UserID = source.UserID;
            ColumnGuid = source.ColumnGuid;
            Settings = source.Settings;
            
            
            

            CopyCustomGridColumnFormatData(source);
        }

        
        /// <summary> The GridColumnFormatID property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."GridColumnFormatID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 GridColumnFormatID { get; }
        /// <summary> The UserID property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The ColumnGuid property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."ColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid ColumnGuid { get; }
        /// <summary> The Settings property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."Settings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Settings { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnFormatEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnFormatEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGridColumnFormatData(IGridColumnFormatEntity source);
    }
}
