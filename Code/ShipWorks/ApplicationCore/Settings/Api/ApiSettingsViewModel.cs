using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
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
        private readonly IMessageHelper messageHelper;
        private readonly IApiPortRegistrationService apiPortRegistrationService;
        private ApiStatus status;
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
        public ApiStatus Status
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
            Status = apiService.Status;
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

                string success = "Successfully started the ShipWorks API.";
                string fail = "Failed to start the ShipWorks API.";

                WaitForStatusToUpdate(ApiStatus.Running, success, fail);
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

                string success = "Successfully stopped the ShipWorks API.";
                string fail = "Failed to stop the ShipWorks API.";

                WaitForStatusToUpdate(ApiStatus.Stopped, success, fail);
            }
        }

        /// <summary>
        /// Update the port number
        /// </summary>
        private void Update()
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                GenericResult<long> portValidationResult = ValidatePort();
                if (portValidationResult.Failure)
                {
                    return;
                }

                long portNumber = portValidationResult.Value;

                Status = ApiStatus.Updating;

                bool result = apiPortRegistrationService.RegisterAsAdmin(portNumber);

                if (!result)
                {
                    messageHelper.ShowError($"Failed to register port {portNumber}.");
                    return;
                }
                else
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

                ApiStatus expectedStatus = apiSettings.Enabled ? ApiStatus.Running : ApiStatus.Stopped;
                string success = "Successfully updated the ShipWorks API port number.";
                string fail = "Failed to update the ShipWorks API port number.";

                WaitForStatusToUpdate(expectedStatus, success, fail);
            }
        }

        /// <summary>
        /// Validate the port string and if valid, return it as a long
        /// </summary>
        private GenericResult<long> ValidatePort()
        {
            // validate port number
            if (!long.TryParse(Port, out long portNumber) || portNumber < MinPort || portNumber > MaxPort)
            {
                messageHelper.ShowError("Please enter a valid port number.");
                return GenericResult.FromError<long>(string.Empty);
            }

            // if the are trying to update the port to what the port is already set to return
            if (apiSettings.Port == portNumber)
            {
                return GenericResult.FromError<long>(string.Empty);
            }

            return GenericResult.FromSuccess(portNumber);
        }

        /// <summary>
        /// Wait for the api status to match the expected status
        /// </summary>
        private void WaitForStatusToUpdate(ApiStatus expectedStatus, string successMessage, string failureMessage)
        {
            for (int tries = 0; tries < 10; tries++)
            {
                if (CheckForStatusUpdate(expectedStatus))
                {
                    Status = apiService.Status;
                    messageHelper.ShowInformation(successMessage);
                    break;
                }

                if (tries == 9)
                {
                    messageHelper.ShowError(failureMessage);
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Check if the current api status matches the expected status
        /// </summary>
        private bool CheckForStatusUpdate(ApiStatus expectedStatus)
        {
            if (Status == ApiStatus.Updating)
            {
                // If we're updating the port and the api is disabled, just show success
                // otherwise make sure its not only running, but running on the new port
                return !apiSettings.Enabled ||
                       apiService.Status == ApiStatus.Running && apiService.Port.ToString() == Port;
            }

            return apiService.Status == expectedStatus;
        }
    }
}
