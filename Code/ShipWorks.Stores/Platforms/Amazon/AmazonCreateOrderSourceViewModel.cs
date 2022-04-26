using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Viewmodel to set up an Amazon Order Source
    /// </summary>
    [Component]
    public class AmazonCreateOrderSourceViewModel : ViewModelBase, IAmazonCreateOrderSourceViewModel
    {
        private const string orderSourceName = "amazon";
        
        private readonly IWebHelper webHelper;
        private readonly IHubMonoauthClient hubMonoauthClient;
        private readonly IMessageHelper messageHelper;
        private bool openingUrl;
        private AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateOrderSourceViewModel(IWebHelper webHelper, IHubMonoauthClient hubMonoauthClient, IMessageHelper messageHelper)
        {
            this.webHelper = webHelper;
            this.hubMonoauthClient = hubMonoauthClient;
            this.messageHelper = messageHelper;

            GetOrderSourceId = new RelayCommand(async () => await GetOrderSourceIdCommand().ConfigureAwait(true));
        }

        /// <summary>
        /// Grabs a URL and opens up the browser to that page
        /// </summary>
        private async Task GetOrderSourceIdCommand()
        {
            OpeningUrl = true;
            try
            {
                var url = await hubMonoauthClient.GetCreateOrderSourceInitiateUrl(orderSourceName, store.AmazonApiRegion).ConfigureAwait(true);
                webHelper.OpenUrl(url);
            }
            catch (Exception ex)
            {
                messageHelper.ShowError(ex.GetBaseException().Message);
            }
            finally
            {
                OpeningUrl = false;
            }
        }

        /// <summary>
        /// Open the browser to get a token
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetOrderSourceId { get; }

        /// <summary>
        /// The OrderSourceId
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderSourceId { get; set; }
        
        /// <summary>
        /// Are we currently trying to open the Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool OpeningUrl
        {
            get => openingUrl;
            private set => Set(ref openingUrl, value);
        }

        /// <summary>
        /// Load the order source
        /// </summary>
        public void Load(AmazonStoreEntity store)
        {
            OrderSourceId = store.OrderSourceID;
            this.store = store;
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public void Save(AmazonStoreEntity store)
        {
            store.OrderSourceID = OrderSourceId;
        }
    }
}