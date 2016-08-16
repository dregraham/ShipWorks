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
    /// Read-only representation of the entity 'ServiceStatus'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyServiceStatusEntity : IServiceStatusEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyServiceStatusEntity(IServiceStatusEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ServiceStatusID = source.ServiceStatusID;
            RowVersion = source.RowVersion;
            ComputerID = source.ComputerID;
            ServiceType = source.ServiceType;
            LastStartDateTime = source.LastStartDateTime;
            LastStopDateTime = source.LastStopDateTime;
            LastCheckInDateTime = source.LastCheckInDateTime;
            ServiceFullName = source.ServiceFullName;
            ServiceDisplayName = source.ServiceDisplayName;
            
            
            Computer = source.Computer?.AsReadOnly(objectMap);
            

            CopyCustomServiceStatusData(source);
        }

        
        /// <summary> The ServiceStatusID property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceStatusID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ServiceStatusID { get; }
        /// <summary> The RowVersion property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ComputerID property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The ServiceType property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ServiceType { get; }
        /// <summary> The LastStartDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastStartDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> LastStartDateTime { get; }
        /// <summary> The LastStopDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastStopDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> LastStopDateTime { get; }
        /// <summary> The LastCheckInDateTime property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."LastCheckInDateTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> LastCheckInDateTime { get; }
        /// <summary> The ServiceFullName property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceFullName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 256<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ServiceFullName { get; }
        /// <summary> The ServiceDisplayName property of the Entity ServiceStatus<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServiceStatus"."ServiceDisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 256<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ServiceDisplayName { get; }
        
        
        public IComputerEntity Computer { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServiceStatusEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServiceStatusEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomServiceStatusData(IServiceStatusEntity source);
    }
}
