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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'FilterNodeContentDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterNodeContentDetailEntity
    {
        
        /// <summary> The FilterNodeContentID property of the Entity FilterNodeContentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContentDetail"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeContentID { get; }
        /// <summary> The ObjectID property of the Entity FilterNodeContentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContentDetail"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ObjectID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeContentDetailEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeContentDetailEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterNodeContentDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterNodeContentDetailEntity : IFilterNodeContentDetailEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentDetailEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterNodeContentDetailEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterNodeContentDetailEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterNodeContentDetailEntity(this, objectMap);
        }
    }
}
