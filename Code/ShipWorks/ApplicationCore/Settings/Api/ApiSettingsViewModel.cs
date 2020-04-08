using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private readonly IMessageHelper messageHelper;
        private readonly IApiPortRegistrationService apiPortRegistrationService;
        private string status;
        private string port;
        private ApiSettings apiSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsViewModel(IApiService apiService, IApiSettingsRepository settingsRepository, IMessageHelper messageHelper, IApiPortRegistrationService apiPortRegistrationService)
        {
            this.apiService = apiService;
            this.settingsRepository = settingsRepository;
            this.messageHelper = messageHelper;
            this.apiPortRegistrationService = apiPortRegistrationService;
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            UpdateCommand = new RelayCommand(Update);
        }

        /// <summary>
        /// The API Status
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Status
        {
            get => status;
            set => Set(ref status, value);
        }

        /// <summary>
        /// The port currently being used by the api
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Port
        {
            get => port;
            set 
            {
                Set(ref port, value);
                RaisePropertyChanged(nameof(DocumentationUrl));
            }
        }

        /// <summary>
        /// Command to start API
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand StartCommand { get; }

        /// <summary>
        /// Command to stop API
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand StopCommand { get; }

        /// <summary>
        /// Command for updating port number
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// Url for the API Docs
        /// </summary>
        [Obfuscation(Exclude = true)]
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
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
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
        }

        /// <summary>
        /// Stop the API
        /// </summary>
        private void Stop()
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
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
        }

        /// <summary>
        /// Update the port number
        /// </summary>
        private void Update()
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                // validate port number
                if (!long.TryParse(Port, out long portNumber) || portNumber < MinPort || portNumber > MaxPort)
                {
                    messageHelper.ShowError("Please enter a valid port number.");
                    return;
                }

                // if the are trying to update the port to what the port is already set to return
                if (apiSettings.Port == portNumber)
                {
                    return;
                }

                Status = "Updating";
                long oldPort = apiSettings.Port;

                // save the setting to disk and then run a process as admin to open that port
                apiSettings.Port = portNumber;
                try
                {
                    settingsRepository.Save(apiSettings);
                }
                catch (Exception)
                {
                    messageHelper.ShowError($"Failed to update to port {portNumber}.");
                }

                bool result = apiPortRegistrationService.RegisterAsAdmin();

                if (!result)
                {
                    // if it failed to open the port we need to roll back the setting on disk 
                    apiSettings.Port = oldPort;
                    settingsRepository.Save(apiSettings);

                    messageHelper.ShowError($"Failed to register port {portNumber}.");

                    return;
                }

                for (int tries = 0; tries < 10; tries++)
                {
                    if (!apiSettings.Enabled || apiService.IsRunning && apiService.Port.ToString() == Port)
                    {
                        Status = apiService.IsRunning ? "Running" : "Stopped";
                        messageHelper.ShowInformation("Successfully updated the ShipWorks API port number.");
                        break;
                    }

                    if (tries == 9)
                    {
                        Status = apiService.IsRunning ? "Running" : "Stopped";
                        messageHelper.ShowError("Failed to update the ShipWorks API port number.");
                        break;
                    }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
