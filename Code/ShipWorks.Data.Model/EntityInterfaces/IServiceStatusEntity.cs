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
    /// Entity interface which represents the entity 'ServiceStatus'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IServiceStatusEntity
    {
        
        /// <summary> The ServiceStatusID property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceStatusID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ServiceStatusID { get; }
        /// <summary> The RowVersion property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ComputerID property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The ServiceType property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ServiceType { get; }
        /// <summary> The LastStartDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastStartDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> LastStartDateTime { get; }
        /// <summary> The LastStopDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastStopDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> LastStopDateTime { get; }
        /// <summary> The LastCheckInDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastCheckInDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> LastCheckInDateTime { get; }
        /// <summary> The ServiceFullName property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceFullName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 256<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ServiceFullName { get; }
        /// <summary> The ServiceDisplayName property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceDisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 256<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ServiceDisplayName { get; }
        
        
        IComputerEntity Computer { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServiceStatusEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServiceStatusEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ServiceStatus'. <br/><br/>
    /// 
    /// </summary>
    public partial class ServiceStatusEntity : IServiceStatusEntity
    {
        
        IComputerEntity IServiceStatusEntity.Computer => Computer;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServiceStatusEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IServiceStatusEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IServiceStatusEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyServiceStatusEntity(this, objectMap);
        }
    }
}
