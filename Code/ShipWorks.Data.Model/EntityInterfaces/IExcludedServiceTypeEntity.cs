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
    /// Entity interface which represents the entity 'ExcludedServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IExcludedServiceTypeEntity
    {
        
        /// <summary> The ShipmentType property of the Entity ExcludedServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedServiceType"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The ServiceType property of the Entity ExcludedServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedServiceType"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int32 ServiceType { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IExcludedServiceTypeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IExcludedServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ExcludedServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial class ExcludedServiceTypeEntity : IExcludedServiceTypeEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedServiceTypeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IExcludedServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IExcludedServiceTypeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyExcludedServiceTypeEntity(this, objectMap);
        }

        
    }
}
