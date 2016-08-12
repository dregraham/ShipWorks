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
    /// Entity interface which represents the entity 'ActionTask'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionTaskEntity
    {
        
        /// <summary> The ActionTaskID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."ActionTaskID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ActionTaskID { get; }
        /// <summary> The ActionID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ActionID { get; }
        /// <summary> The TaskIdentifier property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."TaskIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TaskIdentifier { get; }
        /// <summary> The TaskSettings property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."TaskSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TaskSettings { get; }
        /// <summary> The StepIndex property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."StepIndex"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 StepIndex { get; }
        /// <summary> The InputSource property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."InputSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InputSource { get; }
        /// <summary> The InputFilterNodeID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."InputFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 InputFilterNodeID { get; }
        /// <summary> The FilterCondition property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FilterCondition"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean FilterCondition { get; }
        /// <summary> The FilterConditionNodeID property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FilterConditionNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterConditionNodeID { get; }
        /// <summary> The FlowSuccess property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowSuccess"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowSuccess { get; }
        /// <summary> The FlowSkipped property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowSkipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowSkipped { get; }
        /// <summary> The FlowError property of the Entity ActionTask<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionTask"."FlowError"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlowError { get; }
        
        
        IActionEntity Action { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionTaskEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionTaskEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ActionTask'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionTaskEntity : IActionTaskEntity
    {
        
        IActionEntity IActionTaskEntity.Action => Action;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionTaskEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionTaskEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionTaskEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionTaskEntity(this, objectMap);
        }
    }
}
