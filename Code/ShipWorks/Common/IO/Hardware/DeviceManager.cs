using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.IO.Hardware;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;

namespace ShipWorks.Common.IO.Hardware
{
    /// <summary>
    /// Manage devices
    /// </summary>
    [Component(SingleInstance = true)]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class DeviceManager : ManagerBase<DeviceEntity, IDeviceEntity>, IDeviceManager
    {
        /// <summary>
        /// All the devices
        /// </summary>
        public IEnumerable<IDeviceEntity> DevicesReadOnly => EntitiesReadOnly;
        
        /// <summary>
        /// Used by the base class to create the readonly version of the entity 
        /// </summary>
        protected override IDeviceEntity AsReadOnly(DeviceEntity entity) => entity.AsReadOnly();
    }
}
