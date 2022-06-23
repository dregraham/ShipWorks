using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    [Component]
    public class GetAmazonCarrierCredentialsViewModel : ViewModelBase, IGetAmazonCarrierCredentialsViewModel
    {
        bool carrierExists;
        private string selectedRegion;
        private bool loadingUrl;
        private string credentialsToken;
        private StoreEntity store;
        private ILog log;
        private readonly IMessageHelper messageHelper;
        private readonly IHubOrderSourceClient hubOrderSourceClient;
        private readonly IWebHelper webHelper;

        public GetAmazonCarrierCredentialsViewModel(IMessageHelper messageHelper, Func<Type, ILog> logFactory, IHubOrderSourceClient hubOrderSourceClient, IWebHelper webHelper)
        {
            Regions = CountryList.Countries.Where(c => (new string[] { "CA", "FR", "GR", "IT", "MX", "ES", "UK", "US" }).Contains(c.Key))
                .ToDictionary(c => c.Key, c => c.Value);

            log = logFactory(typeof(GetAmazonCarrierCredentialsViewModel));

            CancelCommand = new RelayCommand(Close);
            SaveCommand = new RelayCommand(Save);
            UpdateCredentialsCommand = new RelayCommand(async () => await UpdateCredentials().ConfigureAwait(true));
            CreateCredentialsCommand = new RelayCommand(async () => await CreateCredentials().ConfigureAwait(true));
            SelectedRegion = "US";
            this.messageHelper = messageHelper;
            this.hubOrderSourceClient = hubOrderSourceClient;
            this.webHelper = webHelper;
        }

        /// <summary>
        /// True if the loaded store associated has a carrier 
        /// </summary>
        [Obfuscation]
        public bool CarrierExists { get => carrierExists; set => Set(ref carrierExists, value); }

        /// <summary>
        /// Trigger process to update carrier credentials
        /// </summary>
        [Obfuscation]
        public ICommand UpdateCredentialsCommand { get; }

        /// <summary>
        /// Amazon Regions
        /// </summary>
        [Obfuscation]
        public Dictionary<string, string> Regions { get; }

        /// <summary>
        /// The selected Amazon Region
        /// </summary>
        [Obfuscation]
        public string SelectedRegion { get => selectedRegion; set => Set(ref selectedRegion, value); }

        /// <summary>
        /// Is a URL Loading?
        /// </summary>
        [Obfuscation]
        public bool LoadingUrl { get => loadingUrl; set => Set(ref loadingUrl, value); }

        /// <summary>
        /// Trigger process to create a carrier
        /// </summary>
        [Obfuscation]
        public ICommand CreateCredentialsCommand { get; }

        /// <summary>
        /// The token retrieved from the CreateCredentials process 
        /// </summary>
        [Obfuscation]
        public string CredentialsToken { get => credentialsToken; set => Set(ref credentialsToken, value); }

        /// <summary>
        /// Save carrier (if required)
        /// </summary>
        [Obfuscation]
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Cancel the process (not applicabile when updating credentials)
        /// </summary>
        [Obfuscation]
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Action to run when the editor is completed
        /// </summary>
        public Action OnComplete { get; set; }

        /// <summary>
        /// Close the view
        /// </summary>
        private void Close() => OnComplete?.Invoke();

        /// <summary>
        /// Save carrier (if required)
        /// </summary>
        public void Save()
        {
            // Only need to save if they are adding a new carrier
            if (CarrierExists || string.IsNullOrEmpty(CredentialsToken))
            {
                Close();
                return;
            }

            if(SaveNewCarrier())
            {
                Close();
                return;
            }

            messageHelper.ShowError("The token does not appear to be correct. Please copy and paste it again.");
        }

        /// <summary>
        /// Load the store
        /// </summary>
        public void Load(StoreEntity store)
        {
            this.store = store;
            CarrierExists = !string.IsNullOrWhiteSpace(store.PlatformAmazonCarrierID);
        }

        /// <summary>
        /// Decodes the provided value and if successful populates the store's data fields.
        /// </summary>
        private bool SaveNewCarrier()
        {
            try
            {
                var data = Convert.FromBase64String(credentialsToken);
                var decodedString = Encoding.UTF8.GetString(data);
                var splitString = decodedString.Split('_');

                if (splitString.Length != 3)
                {
                    log.Error("The provided Base64 string did not have 3 sections separated by the '_' character.");
                    return false;
                }

                IAmazonCredentials credentials = ((IAmazonCredentials) store);
                credentials.MerchantID = splitString[0];
                credentials.Region = selectedRegion;
                store.PlatformAmazonCarrierID = splitString[2];

                return true;
            }
            catch (Exception e)
            {
                log.Error(e);
                return false;
            }
        }

        /// <summary>
        /// Open the URL to kickoff the CreateCredentials process
        /// </summary>
        private Task CreateCredentials()
        {
            IAmazonCredentials credentials = ((IAmazonCredentials) store);
            return InitiateMonoauth(async () => await hubOrderSourceClient.GetCreateCarrierInitiateUrl("amazon", credentials.Region).ConfigureAwait(true));
        }

        /// <summary>
        /// Open the URL to kickoff the UpdateCredentials process
        /// </summary>
        private Task UpdateCredentials()
        {
            IAmazonCredentials credentials = ((IAmazonCredentials) store);
            return InitiateMonoauth(async () => await hubOrderSourceClient.GetUpdateCarrierInitiateUrl("amazon", store.PlatformAmazonCarrierID, credentials.Region, credentials.MerchantID).ConfigureAwait(true));
        }

        /// <summary>
        /// Given a function to get the URL that kicks off the Monoauth process, open the URL in the browser
        /// </summary>
        private async Task InitiateMonoauth(Func<Task<String>> getUrl)
        {
            LoadingUrl = true;
            try
            {
                webHelper.OpenUrl(await getUrl().ConfigureAwait(false));
            }
            catch (ObjectDisposedException ex)
            {
                //User cancelled the dialog before we could open the browser
                //Do nothing
            }
            catch (Exception ex)
            {
                messageHelper.ShowError(ex.GetBaseException().Message);
            }
            finally
            {
                LoadingUrl = false;
            }
        }
    }
}
