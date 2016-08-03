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
    /// Read-only representation of the entity 'ActionQueueSelection'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionQueueSelectionEntity : IActionQueueSelectionEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionQueueSelectionEntity(IActionQueueSelectionEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionQueueSelectionID = source.ActionQueueSelectionID;
            ActionQueueID = source.ActionQueueID;
            EntityID = source.EntityID;
            
            
            ActionQueue = source.ActionQueue?.AsReadOnly(objectMap);
            

            CopyCustomActionQueueSelectionData(source);
        }

        
        /// <summary> The ActionQueueSelectionID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ActionQueueSelectionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ActionQueueSelectionID { get; }
        /// <summary> The ActionQueueID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ActionQueueID { get; }
        /// <summary> The EntityID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EntityID { get; }
        
        
        public IActionQueueEntity ActionQueue { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueSelectionEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueSelectionEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionQueueSelectionData(IActionQueueSelectionEntity source);
    }
}
