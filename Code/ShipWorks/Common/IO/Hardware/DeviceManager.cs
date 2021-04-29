using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;
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
        /// Used by the base class to create the readonly version of the entity 
        /// </summary>
        protected override IDeviceEntity AsReadOnly(DeviceEntity entity) => entity.AsReadOnly();
    }
}
