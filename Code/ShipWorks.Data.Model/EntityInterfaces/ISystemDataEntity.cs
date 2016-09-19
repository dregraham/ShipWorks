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
    /// Entity interface which represents the entity 'SystemData'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISystemDataEntity
    {
        
        /// <summary> The SystemDataID property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."SystemDataID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Boolean SystemDataID { get; }
        /// <summary> The RowVersion property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The DatabaseID property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."DatabaseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid DatabaseID { get; }
        /// <summary> The DateFiltersLastUpdate property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."DateFiltersLastUpdate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime DateFiltersLastUpdate { get; }
        /// <summary> The TemplateVersion property of the Entity SystemData<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SystemData"."TemplateVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TemplateVersion { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISystemDataEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISystemDataEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SystemData'. <br/><br/>
    /// 
    /// </summary>
    public partial class SystemDataEntity : ISystemDataEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISystemDataEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ISystemDataEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISystemDataEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySystemDataEntity(this, objectMap);
        }
    }
}
