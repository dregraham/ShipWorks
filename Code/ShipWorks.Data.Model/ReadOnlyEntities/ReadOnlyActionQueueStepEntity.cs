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
    /// Entity interface which represents the entity 'ActionQueueStep'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionQueueStepEntity : IActionQueueStepEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionQueueStepEntity(IActionQueueStepEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionQueueStepID = source.ActionQueueStepID;
            RowVersion = source.RowVersion;
            ActionQueueID = source.ActionQueueID;
            StepStatus = source.StepStatus;
            StepIndex = source.StepIndex;
            StepName = source.StepName;
            TaskIdentifier = source.TaskIdentifier;
            TaskSettings = source.TaskSettings;
            InputSource = source.InputSource;
            InputFilterNodeID = source.InputFilterNodeID;
            FilterCondition = source.FilterCondition;
            FilterConditionNodeID = source.FilterConditionNodeID;
            FlowSuccess = source.FlowSuccess;
            FlowSkipped = source.FlowSkipped;
            FlowError = source.FlowError;
            AttemptDate = source.AttemptDate;
            AttemptError = source.AttemptError;
            AttemptCount = source.AttemptCount;
            
            
            ActionQueue = source.ActionQueue?.AsReadOnly(objectMap);
            

            CopyCustomActionQueueStepData(source);
        }

        
        /// <summary> The ActionQueueStepID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."ActionQueueStepID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ActionQueueStepID { get; }
        /// <summary> The RowVersion property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ActionQueueID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ActionQueueID { get; }
        /// <summary> The StepStatus property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 StepStatus { get; }
        /// <summary> The StepIndex property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepIndex"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 StepIndex { get; }
        /// <summary> The StepName property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StepName { get; }
        /// <summary> The TaskIdentifier property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."TaskIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TaskIdentifier { get; }
        /// <summary> The TaskSettings property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."TaskSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TaskSettings { get; }
        /// <summary> The InputSource property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."InputSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InputSource { get; }
        /// <summary> The InputFilterNodeID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."InputFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 InputFilterNodeID { get; }
        /// <summary> The FilterCondition property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FilterCondition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FilterCondition { get; }
        /// <summary> The FilterConditionNodeID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FilterConditionNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterConditionNodeID { get; }
        /// <summary> The FlowSuccess property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowSuccess"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowSuccess { get; }
        /// <summary> The FlowSkipped property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowSkipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowSkipped { get; }
        /// <summary> The FlowError property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowError"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowError { get; }
        /// <summary> The AttemptDate property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime AttemptDate { get; }
        /// <summary> The AttemptError property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttemptError { get; }
        /// <summary> The AttemptCount property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AttemptCount { get; }
        
        
        public IActionQueueEntity ActionQueue { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueStepEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueStepEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionQueueStepData(IActionQueueStepEntity source);
    }
}
