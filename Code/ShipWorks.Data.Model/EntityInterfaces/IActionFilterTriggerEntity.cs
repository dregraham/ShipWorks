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
    /// Entity interface which represents the entity 'ActionFilterTrigger'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionFilterTriggerEntity
    {
        
        /// <summary> The ActionID property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ActionID { get; }
        /// <summary> The FilterNodeID property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The Direction property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."Direction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Direction { get; }
        /// <summary> The ComputerLimitedType property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ComputerLimitedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ComputerLimitedType { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity ActionFilterTrigger<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionFilterTrigger"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InternalComputerLimitedList { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionFilterTriggerEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionFilterTriggerEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ActionFilterTrigger'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionFilterTriggerEntity : IActionFilterTriggerEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionFilterTriggerEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionFilterTriggerEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionFilterTriggerEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionFilterTriggerEntity(this, objectMap);
        }
    }
}
