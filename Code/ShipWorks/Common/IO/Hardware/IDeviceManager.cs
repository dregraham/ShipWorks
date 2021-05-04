using System.Collections.Generic;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Common.IO.Hardware
{
    /// <summary>
    /// Manage devices
    /// </summary>
    public interface IDeviceManager : IManagerBase
    {
        /// <summary>
        /// All the devices - Read Only
        /// </summary>
        IEnumerable<IDeviceEntity> DevicesReadOnly { get; }
        
        /// <summary>
        /// All the devices
        /// </summary>
        IEnumerable<DeviceEntity> Devices { get; }

        /// <summary>
        /// Save a new device to DB
        /// </summary>
        void Save(DeviceEntity device, ISqlAdapter adapter);

        /// <summary>
        /// Delete a device from DB
        /// </summary>
        void Delete(DeviceEntity device, ISqlAdapter adapter);
    }
}