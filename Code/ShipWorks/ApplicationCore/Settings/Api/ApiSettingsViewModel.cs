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
using log4net;
using ShipWorks.Api;
using ShipWorks.Api.Configuration;
using ShipWorks.ApplicationCore.Interaction;
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
        private ILog log;
        private ApiStatus status;
        private string port;
        private bool useHttps;
        private ApiSettings apiSettings;
        private bool settingsChanged;
        private string saveButtonText;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsViewModel(
            IApiService apiService,
            IApiSettingsRepository settingsRepository,
            IMessageHelper messageHelper,
            IApiPortRegistrationService apiPortRegistrationService,
            Func<Type, ILog> logFactory)
        {
            this.apiService = apiService;
            this.settingsRepository = settingsRepository;
            this.messageHelper = messageHelper;
            this.apiPortRegistrationService = apiPortRegistrationService;
            this.log = logFactory(typeof(ApiSettingsViewModel));
            StartCommand = new RelayCommand(Start, () => Status == ApiStatus.Stopped);
            StopCommand = new RelayCommand(Stop, () => Status == ApiStatus.Running);
            UpdateCommand = new RelayCommand(Update, () => Status != ApiStatus.Updating);
            SaveButtonText = "Save";
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
                SettingsChanged = value != apiSettings.Port.ToString();
                Set(ref port, value);
            }
        }

        /// <summary>
        /// Should the API use HTTPS
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool UseHttps
        {
            get => useHttps;
            set
            {
                SettingsChanged = value != apiSettings.UseHttps;
                Set(ref useHttps, value);
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
        /// The root URL for the ShipWorks API
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ApiUrl
        {
            get
            {
                string s = UseHttps ? "s" : string.Empty;
                return $"http{s}://{Environment.MachineName}:{Port}";
            }
        }

        /// <summary>
        /// Url for the API Docs
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DocumentationUrl => $"{ApiUrl}/swagger/ui/index";

        [Obfuscation(Exclude = true)]
        public string StartButtonText
        {
            get
            {
                switch (Status)
                {
                    case ApiStatus.Running:
                        return "Stop";
                    case ApiStatus.Stopped:
                        return "Start";
                    case ApiStatus.Updating:
                    default:
                        return "Updating";
                }
            }
        }

        [Obfuscation(Exclude = true)]
        public string SaveButtonText
        {
            get => saveButtonText;
            set => Set(ref saveButtonText, value);
        }

        [Obfuscation(Exclude = true)]
        public bool SettingsChanged
        {
            get => settingsChanged;
            set => Set(ref settingsChanged, value);
        }

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

                string fail = "Failed to start the ShipWorks API.";

                WaitForStatusToUpdate(ApiStatus.Running, fail);
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

                string fail = "Failed to stop the ShipWorks API.";

                WaitForStatusToUpdate(ApiStatus.Stopped, fail);
            }
        }

        /// <summary>
        /// Update Port and Protocol
        /// </summary>
        private void Update()
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                var originalPort = apiSettings.Port.ToString();
                var originalUseHttps = apiSettings.UseHttps;

                GenericResult<long> portValidationResult = ValidatePort();
                if (portValidationResult.Failure)
                {
                    Port = originalPort;
                    return;
                }

                long portNumber = portValidationResult.Value;

                Status = ApiStatus.Updating;
                SaveButtonText = "Saving";
                bool result = apiPortRegistrationService.Register(portNumber, UseHttps);

                if (!result)
                {
                    Status = apiService.Status;
                    messageHelper.ShowError("Failed to update settings.");
                    Port = originalPort;
                    UseHttps = originalUseHttps;
                    return;
                }

                apiSettings.Port = portNumber;
                apiSettings.UseHttps = UseHttps;
                try
                {
                    settingsRepository.Save(apiSettings);
                }
                catch (Exception ex)
                {
                    Port = originalPort;
                    UseHttps = originalUseHttps;
                    Status = apiService.Status;
                    log.Error("An error occurred saving api settings.", ex);
                    messageHelper.ShowError("Failed to update settings.");
                    return;
                }
                
                ApiStatus expectedStatus = apiSettings.Enabled ? ApiStatus.Running : ApiStatus.Stopped;
                string fail = "Failed to update the ShipWorks API settings.";

                WaitForStatusToUpdate(expectedStatus, fail);

                SaveButtonText = "Saved";
                RaisePropertyChanged(nameof(ApiUrl));
                RaisePropertyChanged(nameof(DocumentationUrl));
            }
        }

        /// <summary>
        /// Validate the port string and if valid, return it as a long
        /// </summary>
        private GenericResult<long> ValidatePort()
        {
            // validate port number
            if (!long.TryParse(Port, out long portNumber) || portNumber <= MinPort || portNumber > MaxPort)
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
        private void WaitForStatusToUpdate(ApiStatus expectedStatus, string failureMessage)
        {
            for (int tries = 0; tries < 10; tries++)
            {
                if (CheckForStatusUpdate(expectedStatus))
                {
                    Status = apiService.Status;
                    return;
                }

                Thread.Sleep(1000);
            }

            Status = apiService.Status;
            messageHelper.ShowError(failureMessage);
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
