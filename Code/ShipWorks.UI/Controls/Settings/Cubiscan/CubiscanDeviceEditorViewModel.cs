using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Hardware;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    [Component]
    public class CubiscanDeviceEditorViewModel : ViewModelBase, ICubiscanDeviceEditorViewModel
    {
        private readonly DeviceEntity newDevice;
        private readonly IDeviceManager deviceManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private ComputerEntity selectedComputer;
        private DeviceModel selectedModel;
        private string ipAddress;
        private string portNumber;

        public CubiscanDeviceEditorViewModel(DeviceEntity newDevice, IDeviceManager deviceManager,
            IComputerManager computerManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.newDevice = newDevice;
            this.deviceManager = deviceManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Close);

            Computers = computerManager.GetComputers();
            SelectedComputer = computerManager.GetComputer();
            IPAddress = "127.0.0.1";
            PortNumber = "1050";

            Models = EnumHelper.GetEnumList<DeviceModel>()
                .Select(x => x.Value).ToDictionary(s => s, s => EnumHelper.GetDescription(s));
        }

        public Action OnComplete { get; set; }
        
        [Obfuscation]
        public IEnumerable<ComputerEntity> Computers { get; set; }

        [Obfuscation]
        public ComputerEntity SelectedComputer
        {
            get => selectedComputer;
            set => Set(ref selectedComputer, value);
        }

        [Obfuscation]
        public Dictionary<DeviceModel, string> Models { get; set; }

        [Obfuscation]
        public DeviceModel SelectedModel
        {
            get => selectedModel;
            set => Set(ref selectedModel, value);
        }

        [Obfuscation]
        public string IPAddress
        {
            get => ipAddress;
            set => Set(ref ipAddress, value);
        }

        [Obfuscation]
        public string PortNumber
        {
            get => portNumber;
            set => Set(ref portNumber, value);
        }

        [Obfuscation]
        public ICommand CancelCommand { get; }
        
        [Obfuscation]
        public ICommand SaveCommand { get; }

        private void Save()
        {
            newDevice.Model = SelectedModel;
            newDevice.IPAddress = IPAddress;
            newDevice.PortNumber = short.Parse(PortNumber);
            newDevice.Computer = SelectedComputer;
            
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                deviceManager.Save(newDevice, adapter);
                Close();
            }
        }

        private void Close() => OnComplete?.Invoke();
    }
}