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
    /// Entity interface which represents the entity 'ActionQueue'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionQueueEntity
    {
        
        /// <summary> The ActionQueueID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ActionQueueID { get; }
        /// <summary> The RowVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ActionID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ActionID { get; }
        /// <summary> The ActionName property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ActionName { get; }
        /// <summary> The ActionQueueType property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionQueueType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ActionQueueType { get; }
        /// <summary> The ActionVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] ActionVersion { get; }
        /// <summary> The QueueVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."QueueVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] QueueVersion { get; }
        /// <summary> The TriggerDate property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."TriggerDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime TriggerDate { get; }
        /// <summary> The TriggerComputerID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."TriggerComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TriggerComputerID { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InternalComputerLimitedList { get; }
        /// <summary> The ObjectID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ObjectID { get; }
        /// <summary> The Status property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."Status"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Status { get; }
        /// <summary> The NextStep property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."NextStep"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 NextStep { get; }
        /// <summary> The ContextLock property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ContextLock"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 36<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ContextLock { get; }
        
        
        
        IEnumerable<IActionQueueSelectionEntity> ActionQueueSelection { get; }
        IEnumerable<IActionQueueStepEntity> Steps { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ActionQueue'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionQueueEntity : IActionQueueEntity
    {
        
        
        IEnumerable<IActionQueueSelectionEntity> IActionQueueEntity.ActionQueueSelection => ActionQueueSelection;
        IEnumerable<IActionQueueStepEntity> IActionQueueEntity.Steps => Steps;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionQueueEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionQueueEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionQueueEntity(this, objectMap);
        }
    }
}
