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
    /// Read-only representation of the entity 'Device'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDeviceEntity : IDeviceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDeviceEntity(IDeviceEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DeviceID = source.DeviceID;
            ComputerID = source.ComputerID;
            Model = source.Model;
            IPAddress = source.IPAddress;
            PortNumber = source.PortNumber;
            RowVersion = source.RowVersion;
            
            
            Computer = (IComputerEntity) source.Computer?.AsReadOnly(objectMap);
            

            CopyCustomDeviceData(source);
        }

        
        /// <summary> The DeviceID property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."DeviceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DeviceID { get; }
        /// <summary> The ComputerID property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The Model property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."Model"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public Interapptive.Shared.IO.Hardware.DeviceModel Model { get; }
        /// <summary> The IPAddress property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."IPAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IPAddress { get; }
        /// <summary> The PortNumber property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."PortNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int16 PortNumber { get; }
        /// <summary> The RowVersion property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        
        
        public IComputerEntity Computer { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDeviceEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDeviceEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDeviceData(IDeviceEntity source);
    }
}
