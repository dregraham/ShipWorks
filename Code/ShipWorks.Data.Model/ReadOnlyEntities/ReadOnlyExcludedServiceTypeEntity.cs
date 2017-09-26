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
    /// Read-only representation of the entity 'ExcludedServiceType'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyExcludedServiceTypeEntity : IExcludedServiceTypeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyExcludedServiceTypeEntity(IExcludedServiceTypeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentType = source.ShipmentType;
            ServiceType = source.ServiceType;
            
            
            

            CopyCustomExcludedServiceTypeData(source);
        }

        
        /// <summary> The ShipmentType property of the Entity ExcludedServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedServiceType"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The ServiceType property of the Entity ExcludedServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedServiceType"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int32 ServiceType { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedServiceTypeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomExcludedServiceTypeData(IExcludedServiceTypeEntity source);
    }
}
