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
    /// Read-only representation of the entity 'FilterNodeContentDetail'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterNodeContentDetailEntity : IFilterNodeContentDetailEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterNodeContentDetailEntity(IFilterNodeContentDetailEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterNodeContentID = source.FilterNodeContentID;
            EntityID = source.EntityID;
            
            
            

            CopyCustomFilterNodeContentDetailData(source);
        }

        
        /// <summary> The FilterNodeContentID property of the Entity FilterNodeContentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContentDetail"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 FilterNodeContentID { get; }
        /// <summary> The EntityID property of the Entity FilterNodeContentDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContentDetail"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 EntityID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentDetailEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentDetailEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterNodeContentDetailData(IFilterNodeContentDetailEntity source);
    }
}
