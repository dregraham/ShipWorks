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
    public class DeviceManager : IDeviceManager, IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        private TableSynchronizer<DeviceEntity> tableSynchronizer;
        private bool needCheckForChanges;
        private ReadOnlyCollection<IDeviceEntity> readOnlyEntities;

        public IEnumerable<DeviceEntity> Devices
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return tableSynchronizer.EntityCollection;
                }
            }
        }

        public IEnumerable<IDeviceEntity> DevicesReadOnly
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return readOnlyEntities;
                }
            }
        }

        public void Delete(DeviceEntity device, ISqlAdapter adapter)
        {
            adapter.DeleteEntity(device);
            CheckForChangesNeeded();
        }

        public void Save(DeviceEntity device, ISqlAdapter adapter)
        {
            adapter.SaveAndRefetch(device);
            CheckForChangesNeeded();
        }

        public void CheckForChangesNeeded()
        {
            lock (tableSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        public void InitializeForCurrentSession()
        {
            tableSynchronizer = new TableSynchronizer<DeviceEntity>();
            CheckForChanges();
        }

        private void CheckForChanges()
        {
            lock (tableSynchronizer)
            {
                if (tableSynchronizer.Synchronize())
                {
                    readOnlyEntities = tableSynchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();
                }

                needCheckForChanges = false;
            }
        }
    }
}
