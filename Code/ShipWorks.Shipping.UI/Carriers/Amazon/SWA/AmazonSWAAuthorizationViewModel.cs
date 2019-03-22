using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.UI.Amazon.SWA
{
    /// <summary>
    /// Amazon SWA Authorization Control
    /// </summary>
    [Component]
    public class AmazonSWAAuthorizationViewModel : INotifyPropertyChanged, IAmazonSWAAuthorizationViewModel
    {
        private readonly IWin32Window window;
        private readonly IShipEngineWebClient shipEngineClient;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private string accessCode;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAAuthorizationViewModel(
            IWin32Window window,
            IShipEngineWebClient shipEngineClient)
        {
            this.window = window;
            this.shipEngineClient = shipEngineClient;
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
        /// Use the access code to connect to amazon shipping
        /// </summary>
        /// <returns></returns>
        public async Task<GenericResult<string>> ConnectToAmazonShipping() =>
            await shipEngineClient.ConnectAmazonShippingAccount(accessCode);

        /// <summary>
        /// Opens a browser window which will lead the user to the access token
        /// </summary>
        private void GetAccessCode()
        {
            string authorizationUrl = $"";
            WebHelper.OpenUrl(authorizationUrl, window);
        }
    }
}
