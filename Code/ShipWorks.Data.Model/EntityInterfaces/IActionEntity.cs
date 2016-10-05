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
    /// Entity interface which represents the entity 'Action'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionEntity
    {
        
        /// <summary> The ActionID property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ActionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ActionID { get; }
        /// <summary> The RowVersion property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Enabled property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."Enabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Enabled { get; }
        /// <summary> The ComputerLimitedType property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ComputerLimitedType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ComputerLimitedType { get; }
        /// <summary> The InternalComputerLimitedList property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."ComputerLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InternalComputerLimitedList { get; }
        /// <summary> The StoreLimited property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."StoreLimited"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean StoreLimited { get; }
        /// <summary> The InternalStoreLimitedList property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."StoreLimitedList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InternalStoreLimitedList { get; }
        /// <summary> The TriggerType property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TriggerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 TriggerType { get; }
        /// <summary> The TriggerSettings property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TriggerSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TriggerSettings { get; }
        /// <summary> The TaskSummary property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."TaskSummary"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TaskSummary { get; }
        /// <summary> The InternalOwner property of the Entity Action<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Action"."InternalOwner"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String InternalOwner { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Action'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionEntity : IActionEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionEntity(this, objectMap);
        }
    }
}
