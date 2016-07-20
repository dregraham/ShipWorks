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
    /// Entity interface which represents the entity 'ExcludedPackageType'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyExcludedPackageTypeEntity : IExcludedPackageTypeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyExcludedPackageTypeEntity(IExcludedPackageTypeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentType = source.ShipmentType;
            PackageType = source.PackageType;
            
            
            

            CopyCustomExcludedPackageTypeData(source);
        }

        
        /// <summary> The ShipmentType property of the Entity ExcludedPackageType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedPackageType"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The PackageType property of the Entity ExcludedPackageType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedPackageType"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int32 PackageType { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedPackageTypeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedPackageTypeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomExcludedPackageTypeData(IExcludedPackageTypeEntity source);
    }
}
