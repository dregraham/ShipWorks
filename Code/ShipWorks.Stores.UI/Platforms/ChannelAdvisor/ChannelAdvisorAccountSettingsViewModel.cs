using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

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
        private readonly Dictionary<string,string> encryptedRefreshTokenCache = new Dictionary<string, string>();

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
            WebHelper.OpenUrl(webClient.AuthorizeUrl, window);
        }

        /// <summary>
        /// Saves the specified store.
        /// </summary>
        /// <returns>True if sucessfull</returns>
        public bool Save(ChannelAdvisorStoreEntity store)
        {
            // Get cached refresh token because we can't get the refresh token twice for the same AccessCode
            if (encryptedRefreshTokenCache.ContainsKey(AccessCode))
            {
                store.RefreshToken = encryptedRefreshTokenCache[AccessCode];
                return true;
            }

            try
            {
                string token = webClient.GetRefreshToken(AccessCode);
                store.RefreshToken = encryptionProviderFactory.CreateSecureTextEncryptionProvider("ChannelAdvisor").Encrypt(token);
                encryptedRefreshTokenCache.Add(AccessCode, store.RefreshToken);
                return true;
            }
            catch (ChannelAdvisorException ex)
            {
                messageHelper.ShowMessage("An error occured requesting access. Please get a new access code and try again." +
                                          $"{Environment.NewLine}{Environment.NewLine}{ex.Message}");
            }
            return false;
        }
    }
}
