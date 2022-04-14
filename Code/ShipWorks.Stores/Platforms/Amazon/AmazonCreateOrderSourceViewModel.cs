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

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Viewmodel to set up an Amazon Order Source
    /// </summary>
    [Component]
    public class AmazonCreateOrderSourceViewModel : ViewModelBase, IAmazonCreateOrderSourceViewModel
    {
        private readonly IWebHelper webHelper;
        private readonly IAmazonSpClient amazonSpClient;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCreateOrderSourceViewModel(IWebHelper webHelper, IAmazonSpClient amazonSpClient, IMessageHelper messageHelper)
        {
            this.webHelper = webHelper;
            this.amazonSpClient = amazonSpClient;
            this.messageHelper = messageHelper;

            GetOrderSourceId = new RelayCommand(async () => await GetOrderSourceIdCommand().ConfigureAwait(true));
        }

        /// <summary>
        /// Grabs a URL and opens up the browser to that page
        /// </summary>
        private async Task GetOrderSourceIdCommand()
        {
            try
            {
                var url = await amazonSpClient.GetMonauthInitiateUrl().ConfigureAwait(true);
                webHelper.OpenUrl(url);
            }
            catch (Exception ex)
            {
                messageHelper.ShowError((ex.GetBaseException().Message));    
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
        /// Load the order source
        /// </summary>
        public void Load(AmazonStoreEntity store)
        {
            // implement in next story
            // OrderSourceId = store.OrderSourceId 
        }

        /// <summary>
        /// Save the order source
        /// </summary>
        public void Save(AmazonStoreEntity store)
        {
            // store.OrderSourceId = OrderSourceId
        }
    }
}