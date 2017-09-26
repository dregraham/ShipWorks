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
    /// Entity interface which represents the entity 'ActionQueueStep'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionQueueStepEntity
    {
        
        /// <summary> The ActionQueueStepID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."ActionQueueStepID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ActionQueueStepID { get; }
        /// <summary> The RowVersion property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ActionQueueID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ActionQueueID { get; }
        /// <summary> The StepStatus property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 StepStatus { get; }
        /// <summary> The StepIndex property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepIndex"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 StepIndex { get; }
        /// <summary> The StepName property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."StepName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String StepName { get; }
        /// <summary> The TaskIdentifier property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."TaskIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TaskIdentifier { get; }
        /// <summary> The TaskSettings property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."TaskSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TaskSettings { get; }
        /// <summary> The InputSource property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."InputSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InputSource { get; }
        /// <summary> The InputFilterNodeID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."InputFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 InputFilterNodeID { get; }
        /// <summary> The FilterCondition property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FilterCondition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean FilterCondition { get; }
        /// <summary> The FilterConditionNodeID property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FilterConditionNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterConditionNodeID { get; }
        /// <summary> The FlowSuccess property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowSuccess"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowSuccess { get; }
        /// <summary> The FlowSkipped property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowSkipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowSkipped { get; }
        /// <summary> The FlowError property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."FlowError"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowError { get; }
        /// <summary> The AttemptDate property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime AttemptDate { get; }
        /// <summary> The AttemptError property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttemptError { get; }
        /// <summary> The AttemptCount property of the Entity ActionQueueStep<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueStep"."AttemptCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AttemptCount { get; }
        
        
        IActionQueueEntity ActionQueue { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueStepEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueStepEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ActionQueueStep'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionQueueStepEntity : IActionQueueStepEntity
    {
        
        IActionQueueEntity IActionQueueStepEntity.ActionQueue => ActionQueue;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueStepEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionQueueStepEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionQueueStepEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionQueueStepEntity(this, objectMap);
        }

        
    }
}
