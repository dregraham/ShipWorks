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

namespace ShipWorks.Shipping.UI.Amazon.SWA
{
    /// <summary>
    /// Account settings page for ChannelAdvisor
    /// </summary>
    [Component]
    public class AmazonSWAAuthorizationViewModel : INotifyPropertyChanged
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IWin32Window window;
        private readonly IMessageHelper messageHelper;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private string accessCode;
        private const string AmazonLWAClientId = "amzn1.application-oa2-client.d95db1232b1b411a9bafe026647eca94";

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWAAuthorizationViewModel(
            IEncryptionProviderFactory encryptionProviderFactory,
            IWin32Window window,
            IMessageHelper messageHelper)
        {
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
            string authorizationUrl = $"https://www.amazon.com/ap/oa?client_id={AmazonLWAClientId}&scope=profile&response_type=token&redirect_uri=https://interapptive.com/amazon/authorize.php";
            WebHelper.OpenUrl(authorizationUrl, window);
        }

        /// <summary>
        /// Saves the specified store.
        /// </summary>
        /// <returns>True if successful</returns>
        public bool ConnectToShipEngine(AmazonSWAAccountEntity store)
        {
            // send the access code to ShipEngine to add the account, they should return the ShipEngine accountID
            // set that on the entity and return true

            return true;
        }
    }
}
