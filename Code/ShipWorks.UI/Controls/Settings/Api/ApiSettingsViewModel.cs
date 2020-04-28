using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Api;
using ShipWorks.Api.Configuration;
using Cursors = System.Windows.Forms.Cursors;

namespace ShipWorks.UI.Controls.Settings.Api
{
    /// <summary>
    /// Api Settings logic
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ApiSettingsViewModel : ViewModelBase
    {
        private const int MinPort = 1024;
        private const int MaxPort = 65535;

        private readonly IApiService apiService;
        private readonly IApiSettingsRepository settingsRepository;
        private readonly IMessageHelper messageHelper;
        private readonly IApiPortRegistrationService apiPortRegistrationService;
        private readonly ILog log;
        private ApiStatus status;
        private string port;
        private bool useHttps;
        private ApiSettings apiSettings;
        private bool isSaveEnabled;
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
            StartCommand = new RelayCommand(async () => await ToggleEnabled().ConfigureAwait(true));
            UpdateCommand = new RelayCommand(async () => await Update().ConfigureAwait(true), () => Status != ApiStatus.Updating);
            SaveButtonText = "Save";
        }

        /// <summary>
        /// The API Status
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ApiStatus Status
        {
            get => status;
            set
            {
                Set(ref status, value);
                RaisePropertyChanged(nameof(StartButtonText));
            }
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
                UpdateSaveButton();
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
                Set(ref useHttps, value);
                UpdateSaveButton();
            }
        }

        /// <summary>
        /// Command to start API
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand StartCommand { get; }

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

        /// <summary>
        /// Text to display on the start button
        /// </summary>
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

        /// <summary>
        /// Text to display on the save button
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SaveButtonText
        {
            get => saveButtonText;
            set => Set(ref saveButtonText, value);
        }

        /// <summary>
        /// Whether or not saving is enabled
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSaveEnabled
        {
            get => isSaveEnabled;
            set => Set(ref isSaveEnabled, value);
        }

        /// <summary>
        /// Whether or not the user is on Windows 7
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsWindows7
        {
            get
            {
                Version osVersion = Environment.OSVersion.Version;
                return osVersion.Major == 6 && osVersion.Minor == 1;
            }
        }

        /// <summary>
        /// Load the current api settings
        /// </summary>
        public void Load()
        {
            apiSettings = settingsRepository.Load();
            Status = apiService.Status;
            Port = apiSettings.Port.ToString();
            UseHttps = apiSettings.UseHttps;
        }

        /// <summary>
        /// Update the save buttons text and whether or not it's enabled
        /// </summary>
        private void UpdateSaveButton()
        {
            IsSaveEnabled = Port != apiSettings.Port.ToString() || UseHttps != apiSettings.UseHttps;
            SaveButtonText = "Save";
        }

        /// <summary>
        /// Start the API
        /// </summary>
        private async Task ToggleEnabled()
        {
            using (messageHelper.SetCursor(Cursors.WaitCursor))
            {
                ApiStatus statusToCheckFor = ApiStatus.Running;
                string verb = "start";

                if (Status == ApiStatus.Running)
                {
                    apiSettings.Enabled = false;
                    verb = "stop";
                    statusToCheckFor = ApiStatus.Stopped;
                }
                else if (Status == ApiStatus.Stopped)
                {
                    apiSettings.Enabled = true;
                }

                Status = ApiStatus.Updating;

                settingsRepository.Save(apiSettings);

                string failureMessage = $"Failed to {verb} the ShipWorks API.";
                await WaitForStatusToUpdate(statusToCheckFor, failureMessage).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Update Port and Protocol
        /// </summary>
        private async Task Update()
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
                bool result = apiPortRegistrationService.RegisterAsAdmin(portNumber, UseHttps);

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
                string fail = "Failed to restart the ShipWorks API with the new settings. Please try restarting ShipWorks or changing your settings.";

                await WaitForStatusToUpdate(expectedStatus, fail).ConfigureAwait(true);

                SaveButtonText = "Saved";
                IsSaveEnabled = false;
                RaisePropertyChanged(nameof(ApiUrl));
                RaisePropertyChanged(nameof(DocumentationUrl));
            }
        }

        /// <summary>
        /// Validate the port string and if valid, return it as a long
        /// </summary>
        private GenericResult<long> ValidatePort()
        {
            Port = Port.TrimStart('0');

            // validate port number
            if (!long.TryParse(Port, out long portNumber) || portNumber < MinPort || portNumber > MaxPort)
            {
                messageHelper.ShowError($"Please enter a valid port number between {MinPort} and {MaxPort}.");
                return GenericResult.FromError<long>(string.Empty);
            }

            return GenericResult.FromSuccess(portNumber);
        }

        /// <summary>
        /// Wait for the api status to match the expected status
        /// </summary>
        private async Task WaitForStatusToUpdate(ApiStatus expectedStatus, string failureMessage)
        {
            for (int tries = 0; tries < 15; tries++)
            {
                if (CheckForStatusUpdate(expectedStatus))
                {
                    Status = apiService.Status;
                    return;
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }

            Status = apiService.Status;
            messageHelper.ShowError(failureMessage);
        }

        /// <summary>
        /// Check if the current api status matches the expected status
        /// </summary>
        private bool CheckForStatusUpdate(ApiStatus expectedStatus)
        {
            if (expectedStatus == ApiStatus.Running)
            {
                // Make sure its running and on the correct port
                return apiService.Status == ApiStatus.Running && apiService.Port == apiSettings.Port;
            }

            return apiService.Status == expectedStatus;
        }
    }
}
