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
    /// Read-only representation of the entity 'SystemData'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySystemDataEntity : ISystemDataEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySystemDataEntity(ISystemDataEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SystemDataID = source.SystemDataID;
            RowVersion = source.RowVersion;
            DatabaseID = source.DatabaseID;
            DateFiltersLastUpdate = source.DateFiltersLastUpdate;
            TemplateVersion = source.TemplateVersion;
            
            
            

            CopyCustomSystemDataData(source);
        }

        
        /// <summary> The SystemDataID property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."SystemDataID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Boolean SystemDataID { get; }
        /// <summary> The RowVersion property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The DatabaseID property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."DatabaseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid DatabaseID { get; }
        /// <summary> The DateFiltersLastUpdate property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."DateFiltersLastUpdate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime DateFiltersLastUpdate { get; }
        /// <summary> The TemplateVersion property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."TemplateVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TemplateVersion { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISystemDataEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISystemDataEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSystemDataData(ISystemDataEntity source);
    }
}
