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
    /// Entity interface which represents the entity 'Device'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDeviceEntity
    {
        
        /// <summary> The DeviceID property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."DeviceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DeviceID { get; }
        /// <summary> The ComputerID property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The Model property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."Model"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int16 Model { get; }
        /// <summary> The IPAddress property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."IPAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String IPAddress { get; }
        /// <summary> The PortNumber property of the Entity Device<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Device"."PortNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int16 PortNumber { get; }
        
        
        IComputerEntity Computer { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDeviceEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDeviceEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Device'. <br/><br/>
    /// 
    /// </summary>
    public partial class DeviceEntity : IDeviceEntity
    {
        
        IComputerEntity IDeviceEntity.Computer => Computer;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDeviceEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDeviceEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDeviceEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDeviceEntity(this, objectMap);
        }

        
    }
}
