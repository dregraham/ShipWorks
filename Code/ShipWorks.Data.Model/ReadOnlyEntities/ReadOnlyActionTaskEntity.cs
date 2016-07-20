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
    /// Entity interface which represents the entity 'ActionTask'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionTaskEntity : IActionTaskEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionTaskEntity(IActionTaskEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionTaskID = source.ActionTaskID;
            ActionID = source.ActionID;
            TaskIdentifier = source.TaskIdentifier;
            TaskSettings = source.TaskSettings;
            StepIndex = source.StepIndex;
            InputSource = source.InputSource;
            InputFilterNodeID = source.InputFilterNodeID;
            FilterCondition = source.FilterCondition;
            FilterConditionNodeID = source.FilterConditionNodeID;
            FlowSuccess = source.FlowSuccess;
            FlowSkipped = source.FlowSkipped;
            FlowError = source.FlowError;
            
            
            Action = source.Action?.AsReadOnly(objectMap);
            

            CopyCustomActionTaskData(source);
        }

        
        /// <summary> The ActionTaskID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."ActionTaskID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ActionTaskID { get; }
        /// <summary> The ActionID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ActionID { get; }
        /// <summary> The TaskIdentifier property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."TaskIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TaskIdentifier { get; }
        /// <summary> The TaskSettings property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."TaskSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TaskSettings { get; }
        /// <summary> The StepIndex property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."StepIndex"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 StepIndex { get; }
        /// <summary> The InputSource property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."InputSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InputSource { get; }
        /// <summary> The InputFilterNodeID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."InputFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 InputFilterNodeID { get; }
        /// <summary> The FilterCondition property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FilterCondition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FilterCondition { get; }
        /// <summary> The FilterConditionNodeID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FilterConditionNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterConditionNodeID { get; }
        /// <summary> The FlowSuccess property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowSuccess"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowSuccess { get; }
        /// <summary> The FlowSkipped property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowSkipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowSkipped { get; }
        /// <summary> The FlowError property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowError"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlowError { get; }
        
        
        public IActionEntity Action { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionTaskEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionTaskEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionTaskData(IActionTaskEntity source);
    }
}
