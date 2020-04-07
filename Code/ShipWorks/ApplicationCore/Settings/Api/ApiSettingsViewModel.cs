using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Api;
using ShipWorks.Api.Configuration;
using Cursors = System.Windows.Forms.Cursors;

namespace ShipWorks.ApplicationCore.Settings.Api
{
    /// <summary>
    /// Api Settings logic
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ApiSettingsViewModel : ViewModelBase
    {
        private const int MinPort = 0;
        private const int MaxPort = 65535;

        private readonly IApiService apiService;
        private readonly IApiSettingsRepository settingsRepository;
        private readonly IApiPortRegistrationService portRegistrationService;
        private readonly IMessageHelper messageHelper;
        private string status;
        private string port;
        private ApiSettings apiSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsViewModel(IApiService apiService, IApiSettingsRepository settingsRepository, IApiPortRegistrationService portRegistrationService, IMessageHelper messageHelper)
        {
            this.apiService = apiService;
            this.settingsRepository = settingsRepository;
            this.portRegistrationService = portRegistrationService;
            this.messageHelper = messageHelper;

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            UpdateCommand = new RelayCommand(Update);
        }

        /// <summary>
        /// The API Status
        /// </summary>
        public string Status
        {
            get => status;
            set => Set(ref status, value);
        }

        /// <summary>
        /// The port currently being used by the api
        /// </summary>
        public string Port
        {
            get => port;
            set => Set(ref port, value);
        }

        /// <summary>
        /// Command to start API
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Command to stop API
        /// </summary>
        public ICommand StopCommand { get; }

        /// <summary>
        /// Command for updating port number
        /// </summary>
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// Url for the API Docs
        /// </summary>
        public string DocumentationUrl => $"http://{Environment.MachineName}:{Port}/swagger/ui/index";

        /// <summary>
        /// Load the current api settings
        /// </summary>
        public void Load()
        {
            apiSettings = settingsRepository.Load();

            Status = apiService.IsRunning ? "Running" : "Stopped";
            Port = apiSettings.Port.ToString();
        }

        /// <summary>
        /// Start the API
        /// </summary>
        private void Start()
        {
            messageHelper.SetCursor(Cursors.WaitCursor);

            apiSettings.Enabled = true;
            settingsRepository.Save(apiSettings);

            for (int tries = 0; tries < 10; tries++)
            {
                if (apiService.IsRunning)
                {
                    Status = "Running";
                    messageHelper.ShowInformation("Successfully started the ShipWorks API.");
                    break;
                }

                if (tries == 9)
                {
                    messageHelper.ShowError("Failed to start the ShipWorks API.");
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Stop the API
        /// </summary>
        private void Stop()
        {
            messageHelper.SetCursor(Cursors.WaitCursor);

            apiSettings.Enabled = false;
            settingsRepository.Save(apiSettings);

            for (int tries = 0; tries < 10; tries++)
            {
                if (!apiService.IsRunning)
                {
                    Status = "Stopped";
                    messageHelper.ShowInformation("Successfully stopped the ShipWorks API.");
                    break;
                }

                if (tries == 9)
                {
                    messageHelper.ShowError("Failed to stop the ShipWorks API.");
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Update the port number
        /// </summary>
        private void Update()
        {
            messageHelper.SetCursor(Cursors.WaitCursor);

            // validate port number
            if (!long.TryParse(Port, out long portNumber) || portNumber < MinPort || portNumber > MaxPort)
            {
                messageHelper.ShowError("Please enter a valid port number.");
                return;
            }

            // register port
            bool registrationResult = portRegistrationService.Register(portNumber);

            // if registration was successful save, else show error
            if (registrationResult)
            {
                apiSettings.Port = portNumber;
                try
                {
                    settingsRepository.Save(apiSettings);
                }
                catch (Exception)
                {
                    messageHelper.ShowError($"Failed to update to port {portNumber}.");
                }
            }
            else
            {
                messageHelper.ShowError($"Failed to register port {portNumber}.");
            }

            for (int tries = 0; tries < 10; tries++)
            {
                if (apiService.IsRunning && apiService.Port.ToString() == Port)
                {
                    Status = "Running";
                    messageHelper.ShowInformation("Successfully started the ShipWorks API.");
                    break;
                }

                if (tries == 9)
                {
                    messageHelper.ShowError("Failed to start the ShipWorks API.");
                    break;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
