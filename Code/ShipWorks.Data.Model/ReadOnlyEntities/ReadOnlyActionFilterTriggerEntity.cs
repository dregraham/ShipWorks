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
    /// Entity interface which represents the entity 'ActionFilterTrigger'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionFilterTriggerEntity : IActionFilterTriggerEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionFilterTriggerEntity(IActionFilterTriggerEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionID = source.ActionID;
            FilterNodeID = source.FilterNodeID;
            Direction = source.Direction;
            ComputerLimitedType = source.ComputerLimitedType;
            InternalComputerLimitedList = source.InternalComputerLimitedList;
            
            
            

            CopyCustomActionFilterTriggerData(source);
        }

        
        /// <summary> The ActionID property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ActionID { get; }
        /// <summary> The FilterNodeID property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The Direction property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."Direction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Direction { get; }
        /// <summary> The ComputerLimitedType property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ComputerLimitedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ComputerLimitedType { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalComputerLimitedList { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionFilterTriggerEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionFilterTriggerEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionFilterTriggerData(IActionFilterTriggerEntity source);
    }
}
