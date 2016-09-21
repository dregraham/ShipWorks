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
    /// Read-only representation of the entity 'Action'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyActionEntity : IActionEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyActionEntity(IActionEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ActionID = source.ActionID;
            RowVersion = source.RowVersion;
            Name = source.Name;
            Enabled = source.Enabled;
            ComputerLimitedType = source.ComputerLimitedType;
            InternalComputerLimitedList = source.InternalComputerLimitedList;
            StoreLimited = source.StoreLimited;
            InternalStoreLimitedList = source.InternalStoreLimitedList;
            TriggerType = source.TriggerType;
            TriggerSettings = source.TriggerSettings;
            TaskSummary = source.TaskSummary;
            InternalOwner = source.InternalOwner;
            
            
            

            CopyCustomActionData(source);
        }

        
        /// <summary> The ActionID property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ActionID { get; }
        /// <summary> The RowVersion property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Enabled property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."Enabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Enabled { get; }
        /// <summary> The ComputerLimitedType property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ComputerLimitedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ComputerLimitedType { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalComputerLimitedList { get; }
        /// <summary> The StoreLimited property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."StoreLimited"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean StoreLimited { get; }
        /// <summary> The InternalStoreLimitedList property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."StoreLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalStoreLimitedList { get; }
        /// <summary> The TriggerType property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TriggerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 TriggerType { get; }
        /// <summary> The TriggerSettings property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TriggerSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TriggerSettings { get; }
        /// <summary> The TaskSummary property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TaskSummary"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TaskSummary { get; }
        /// <summary> The InternalOwner property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."InternalOwner"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InternalOwner { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomActionData(IActionEntity source);
    }
}
