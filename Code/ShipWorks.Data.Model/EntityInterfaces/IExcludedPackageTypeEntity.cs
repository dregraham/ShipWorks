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
    /// Entity interface which represents the entity 'ExcludedPackageType'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IExcludedPackageTypeEntity
    {
        
        /// <summary> The ShipmentType property of the Entity ExcludedPackageType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedPackageType"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The PackageType property of the Entity ExcludedPackageType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ExcludedPackageType"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int32 PackageType { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IExcludedPackageTypeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IExcludedPackageTypeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ExcludedPackageType'. <br/><br/>
    /// 
    /// </summary>
    public partial class ExcludedPackageTypeEntity : IExcludedPackageTypeEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IExcludedPackageTypeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IExcludedPackageTypeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IExcludedPackageTypeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyExcludedPackageTypeEntity(this, objectMap);
        }
    }
}
