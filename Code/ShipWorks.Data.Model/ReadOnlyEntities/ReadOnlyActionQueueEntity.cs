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
    /// Read-only representation of the entity 'ActionQueue'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionQueueEntity : IActionQueueEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionQueueEntity(IActionQueueEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionQueueID = source.ActionQueueID;
            RowVersion = source.RowVersion;
            ActionID = source.ActionID;
            ActionName = source.ActionName;
            ActionQueueType = source.ActionQueueType;
            ActionVersion = source.ActionVersion;
            QueueVersion = source.QueueVersion;
            TriggerDate = source.TriggerDate;
            TriggerComputerID = source.TriggerComputerID;
            InternalComputerLimitedList = source.InternalComputerLimitedList;
            EntityID = source.EntityID;
            Status = source.Status;
            NextStep = source.NextStep;
            ContextLock = source.ContextLock;
            ExtraData = source.ExtraData;
            
            
            
            ActionQueueSelection = source.ActionQueueSelection?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IActionQueueSelectionEntity>();
            Steps = source.Steps?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IActionQueueStepEntity>();

            CopyCustomActionQueueData(source);
        }

        
        /// <summary> The ActionQueueID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ActionQueueID { get; }
        /// <summary> The RowVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ActionID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ActionID { get; }
        /// <summary> The ActionName property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ActionName { get; }
        /// <summary> The ActionQueueType property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionQueueType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ActionQueueType { get; }
        /// <summary> The ActionVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ActionVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] ActionVersion { get; }
        /// <summary> The QueueVersion property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."QueueVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] QueueVersion { get; }
        /// <summary> The TriggerDate property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."TriggerDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime TriggerDate { get; }
        /// <summary> The TriggerComputerID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."TriggerComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TriggerComputerID { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalComputerLimitedList { get; }
        /// <summary> The EntityID property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> EntityID { get; }
        /// <summary> The Status property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."Status"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Status { get; }
        /// <summary> The NextStep property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."NextStep"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 NextStep { get; }
        /// <summary> The ContextLock property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ContextLock"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 36<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ContextLock { get; }
        /// <summary> The ExtraData property of the Entity ActionQueue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueue"."ExtraData"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ExtraData { get; }
        
        
        
        public IEnumerable<IActionQueueSelectionEntity> ActionQueueSelection { get; }
        
        public IEnumerable<IActionQueueStepEntity> Steps { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionQueueData(IActionQueueEntity source);
    }
}
