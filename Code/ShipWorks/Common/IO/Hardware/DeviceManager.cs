using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Common.IO.Hardware
{
    /// <summary>
    /// Manage devices
    /// </summary>
    [Component(SingleInstance = true)]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class DeviceManager : ManagerBase<DeviceEntity, IDeviceEntity>, IDeviceManager, IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        /// <summary>
        /// All the devices - Readonly
        /// </summary>
        public IEnumerable<IDeviceEntity> DevicesReadOnly => EntitiesReadOnly;

        /// <summary>
        /// All the devices
        /// </summary>
        public IEnumerable<DeviceEntity> Devices => Entities;

        /// <summary>
        /// Save a new device to DB
        /// </summary>
        public void Save(DeviceEntity device, ISqlAdapter adapter) => base.Save(device, adapter);

        /// <summary>
        /// Delete a device from DB
        /// </summary>
        public void Delete(DeviceEntity device, ISqlAdapter adapter) => base.Delete(device, adapter);

        /// <summary>
        /// Used by the base class to create the readonly version of the entity 
        /// </summary>
        protected override IDeviceEntity AsReadOnly(DeviceEntity entity) => entity.AsReadOnly();
    }
}
