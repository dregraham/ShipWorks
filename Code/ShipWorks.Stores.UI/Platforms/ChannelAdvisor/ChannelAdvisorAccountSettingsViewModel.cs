using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Account settings page for ChannelAdvisor
    /// </summary>
    [Component]
    public class ChannelAdvisorAccountSettingsViewModel : INotifyPropertyChanged, IChannelAdvisorAccountSettingsViewModel
    {
        private readonly IChannelAdvisorRestClient webClient;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IWin32Window window;
        private readonly IMessageHelper messageHelper;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private string accessCode;
        private string accessCodeForSavedRefreshToken = string.Empty;
        private string authorizeUrlParameters;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorAccountSettingsViewModel(IChannelAdvisorRestClient webClient,
            IEncryptionProviderFactory encryptionProviderFactory,
            IWin32Window window,
            IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.window = window;
            this.messageHelper = messageHelper;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            GetAccessCodeCommand = new RelayCommand(GetAccessCode);
        }

        /// <summary>
        /// Gets or sets the access code.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AccessCode
        {
            get { return accessCode; }
            set { handler.Set(nameof(AccessCode), ref accessCode, value); }
        }

        /// <summary>
        /// Command to GetAccessCode
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetAccessCodeCommand { get; }

        /// <summary>
        /// Opens a browser window which will lead the user to the access token
        /// </summary>
        private void GetAccessCode()
        {
            string authorizationUrl = $"{ChannelAdvisorRestClient.EndpointBase}/oauth2/authorize{authorizeUrlParameters}";
            WebHelper.OpenUrl(authorizationUrl, window);
        }

        /// <summary>
        /// Loads the specified store.
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public void Load(ChannelAdvisorStoreEntity store)
        {
            authorizeUrlParameters = new ChannelAdvisorStoreType(store).AuthorizeUrlParameters;
        }

        /// <summary>
        /// Saves the specified store.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool Save(ChannelAdvisorStoreEntity store, bool ignoreEmptyAccessCode)
        {
            if (string.IsNullOrWhiteSpace(AccessCode) && !ignoreEmptyAccessCode)
            {
                messageHelper.ShowMessage("Access code required");
                return false;
            }

            // If the access code has not changed since the last time we saved, continue.
            if (AccessCodeChanged && !string.IsNullOrWhiteSpace(AccessCode))
            {
                try
                {
                    string refreshToken = webClient.GetRefreshToken(AccessCode, new ChannelAdvisorStoreType(store).RedirectUrl).Value;
                    store.RefreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor")
                        .Encrypt(refreshToken);

                    UpdateStoreInfo(store, refreshToken);

                    accessCodeForSavedRefreshToken = AccessCode;
                    return true;
                }
                catch (ChannelAdvisorException ex)
                {
                    messageHelper.ShowMessage(
                        "An error occurred requesting access. Please get a new access code and try again." +
                        $"{Environment.NewLine}{Environment.NewLine}{ex.Message}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Update the store info from ChannelAdvisor
        /// </summary>
        private void UpdateStoreInfo(ChannelAdvisorStoreEntity store, string refreshToken)
        {
            // if we already have a profile id dont do anything
            if (store.ProfileID > 0)
            {
                return;
            }

            ChannelAdvisorProfile profile = webClient.GetProfiles(refreshToken)?.Profiles?.First();

            store.ProfileID = profile?.ProfileId ?? 0;
            store.StoreName = profile?.AccountName ?? string.Empty;
            store.Company = profile?.CompanyName ?? string.Empty;
        }

        /// <summary>
        /// Returns true if access code has not been changed since the last change
        /// </summary>
        private bool AccessCodeChanged => accessCodeForSavedRefreshToken != AccessCode;
    }
}
