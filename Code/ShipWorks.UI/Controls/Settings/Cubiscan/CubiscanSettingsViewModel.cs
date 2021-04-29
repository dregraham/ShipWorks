using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    [Component(RegistrationType.Self)]
    public class CubiscanSettingsViewModel : ViewModelBase
    {
        private readonly IComputerManager computerManager;
        private readonly IDeviceManager deviceManager;
        private ObservableCollection<DeviceEntity> devices;
        private List<ComputerEntity> computers;

        public CubiscanSettingsViewModel(
            IComputerManager computerManager, 
            IDeviceManager deviceManager)
        {
            this.computerManager = computerManager;
            this.deviceManager = deviceManager;
        }

        public void Load()
        {
            computers = computerManager.GetComputers();
            var localDevices = deviceManager.Devices.ToList();
            foreach (var device in localDevices)
            {
                device.Computer = computers.SingleOrDefault(c => c.ComputerID == device.ComputerID);
            }
            
            devices = new ObservableCollection<DeviceEntity>(localDevices);
        }

        [Obfuscation]
        public ObservableCollection<DeviceEntity> Devices
        {
            get => devices;
            set => Set(ref devices, value);
        }
    }
}