using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    /// <summary>
    /// ViewModel for a CubiscanSettings View
    /// </summary>
    [Component]
    public class CubiscanSettingsViewModel : ViewModelBase, ICubiscanSettingsViewModel
    {
        private readonly IWin32Window owner;
        private readonly IComputerManager computerManager;
        private readonly IDeviceManager deviceManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<DeviceEntity, ICubiscanDeviceEditorViewModel> deviceEditorViewModelFactory;
        private readonly IMessageHelper messageHelper;
        private ObservableCollection<DeviceEntity> devices;
        private List<ComputerEntity> computers;
        private DeviceEntity selectedDevice;

        /// <summary>
        /// Constructor
        /// </summary>
        public CubiscanSettingsViewModel(
            IWin32Window owner,
            IComputerManager computerManager, 
            IDeviceManager deviceManager,
            ISqlAdapterFactory sqlAdapterFactory,
            Func<DeviceEntity, ICubiscanDeviceEditorViewModel> deviceEditorViewModelFactory,
            IMessageHelper messageHelper)
        {
            this.owner = owner;
            this.computerManager = computerManager;
            this.deviceManager = deviceManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.deviceEditorViewModelFactory = deviceEditorViewModelFactory;
            this.messageHelper = messageHelper;

            Devices = new ObservableCollection<DeviceEntity>();
            
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete,
                () => SelectedDevice != null);
        }

        /// <summary>
        /// Command to add a new device
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddCommand { get; }

        /// <summary>
        /// Command to delete a device
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Load the viewmodel
        /// </summary>
        public void Load()
        {
            computers = computerManager.GetComputers();
            var localDevices = deviceManager.Devices.ToList();
            foreach (var device in localDevices)
            {
                device.Computer = computers.SingleOrDefault(c => c.ComputerID == device.ComputerID);
            }
            
            Devices = new ObservableCollection<DeviceEntity>(localDevices);
        }

        /// <summary>
        /// Add a device
        /// </summary>
        private void Add()
        {
            var newDevice = new DeviceEntity();
            var deviceEditorViewModel = deviceEditorViewModelFactory(newDevice);
            CubiscanDeviceEditorDialog dialog = new CubiscanDeviceEditorDialog(owner, deviceEditorViewModel);
            deviceEditorViewModel.OnComplete = dialog.Close;
            dialog.ShowDialog();

            if (newDevice.DeviceID != 0)
            {
                Devices.Add(newDevice);
            }
        }
        
        /// <summary>
        /// Delete a device
        /// </summary>
        private void Delete()
        {
            if (messageHelper.ShowQuestion($"Do you want to delete the device for {SelectedDevice.Computer.Name}?") ==
                DialogResult.OK)
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    deviceManager.Delete(SelectedDevice, sqlAdapter);
                }

                Devices.Remove(SelectedDevice);
            }
        }

        /// <summary>
        /// List of configured devices
        /// </summary>
        [Obfuscation]
        public ObservableCollection<DeviceEntity> Devices
        {
            get => devices;
            set => Set(ref devices, value);
        }
        
        /// <summary>
        /// Currently selected device
        /// </summary>
        [Obfuscation]
        public DeviceEntity SelectedDevice
        {
            get => selectedDevice;
            set => Set(ref selectedDevice, value);
        }
    }
}