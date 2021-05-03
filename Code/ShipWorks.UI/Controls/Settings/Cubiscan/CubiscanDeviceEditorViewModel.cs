using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Hardware;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
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
        private readonly IHttpValidator httpValidator;
        private readonly IMessageHelper messageHelper;
        private ComputerEntity selectedComputer;
        private DeviceModel selectedModel;
        private string ipAddress;
        private string portNumber;

        public CubiscanDeviceEditorViewModel(DeviceEntity newDevice, IDeviceManager deviceManager,
            IComputerManager computerManager, ISqlAdapterFactory sqlAdapterFactory, IHttpValidator httpValidator, IMessageHelper messageHelper)
        {
            this.newDevice = newDevice;
            this.deviceManager = deviceManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.httpValidator = httpValidator;
            this.messageHelper = messageHelper;

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
            var validationResult = Validate();
            if (validationResult.Success)
            {
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    deviceManager.Save(validationResult.Value, adapter);
                    Close();
                }
            }
            else
            {
                messageHelper.ShowError(validationResult.Message);
            }
        }

        private GenericResult<DeviceEntity> Validate()
        {
            var errors = new List<string>();
            
            // Ensure computer selected
            if (SelectedComputer == null)
            {
                errors.Add("Please select a computer");
            }

            // Validate port number
            var portValidationResult = httpValidator.ValidatePort(PortNumber);
            if (portValidationResult.Failure)
            {
                errors.Add(portValidationResult.Message);
            }

            // Validate IP address
            var ipValidationResult = httpValidator.ValidateIPAddress(IPAddress);
            if (ipValidationResult.Failure)
            {
                errors.Add(ipValidationResult.Message);
            }

            // Ensure device with same port and IP address does not already exist
            if (deviceManager.DevicesReadOnly.Any(existingDevice =>
                existingDevice.PortNumber == portValidationResult.Value &&
                existingDevice.IPAddress == ipValidationResult.Value))
            {
                errors.Add("There is a device currently registered to this IP address and port number.");
            }

            return BuildValidationResult(errors, ipValidationResult, portValidationResult);
        }

        private GenericResult<DeviceEntity> BuildValidationResult(List<string> errors, GenericResult<string> ipValidationResult, GenericResult<long> portValidationResult)
        {
            if (errors.Count > 0)
            {
                string message;
                if (errors.Count == 1)
                {
                    message = errors[0];
                }
                else
                {
                    StringBuilder errorBuilder = new StringBuilder("The following errors occured:");
                    errorBuilder.AppendLine();
                    errors.ForEach(x => errorBuilder.AppendLine($"- {x}"));
                    message = errorBuilder.ToString();
                }

                return GenericResult.FromError<DeviceEntity>(message);
            }

            newDevice.Model = SelectedModel;
            newDevice.IPAddress = ipValidationResult.Value;
            newDevice.PortNumber = (short) portValidationResult.Value;
            newDevice.Computer = SelectedComputer;

            return GenericResult.FromSuccess(newDevice);
        }

        private void Close() => OnComplete?.Invoke();
    }
}