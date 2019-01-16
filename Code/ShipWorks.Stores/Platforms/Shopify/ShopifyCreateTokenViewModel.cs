using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// View model to create a Shopify token
    /// </summary>
    [Component]
    public class ShopifyCreateTokenViewModel : ViewModelBase, IShopifyCreateTokenViewModel
    {
        private readonly Func<IShopifyCreateTokenViewModel, IShopifyCreateTokenDialog> createDialog;
        private IShopifyCreateTokenDialog dialog;
        private readonly IMessageHelper messageHelper;
        private string name;
        private string code;
        private bool canInteract = true;
        private string token;
        private readonly IWebHelper webHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateTokenViewModel(
            Func<IShopifyCreateTokenViewModel, IShopifyCreateTokenDialog> createDialog,
            IMessageHelper messageHelper,
            IWebHelper webHelper)
        {
            this.webHelper = webHelper;
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;

            GetToken = new RelayCommand(GetTokenAction, () => !string.IsNullOrWhiteSpace(Name));
            Cancel = new RelayCommand(CancelAction);
            Save = new RelayCommand(SaveAction, () => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Code));
        }

        /// <summary>
        /// Create a Shopify token
        /// </summary>
        public GenericResult<(string name, string token)> CreateToken()
        {
            dialog = createDialog(this);

            return messageHelper.ShowDialog(dialog) == true ?
                GenericResult.FromSuccess((Name.Trim(), token)) :
                GenericResult.FromError<(string, string)>("Canceled");
        }

        /// <summary>
        /// Open the browser to get a token
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetToken { get; }

        /// <summary>
        /// Cancel the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; }

        /// <summary>
        /// Save the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Save { get; }

        /// <summary>
        /// Name of the Shopify store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        /// <summary>
        /// Code copied from Tango
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Code
        {
            get => code;
            set => Set(ref code, value);
        }

        /// <summary>
        /// Can the user interact with the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CanInteract
        {
            get => canInteract;
            set => Set(ref canInteract, value);
        }

        /// <summary>
        /// Open the browser to get a token
        /// </summary>
        private void GetTokenAction() =>
            Try(() => new ShopifyEndpoints(Name.Trim()).GetApiAuthorizeUrl())
                .Do(x => webHelper.OpenUrl(x))
                .OnFailure(ex => messageHelper.ShowError(ex.GetBaseException().Message));

        /// <summary>
        /// Cancel changes
        /// </summary>
        private void CancelAction() => dialog.Close();

        /// <summary>
        /// Save the results
        /// </summary>
        private void SaveAction()
        {
            CanInteract = false;

            Task.Run(() => ShopifyWebClient.GetAccessToken(Name.Trim(), Code.Trim()))
                .Do(SaveTokenAndClose,
                    HandleException,
                    ContinueOn.CurrentThread)
                .Forget();
        }

        /// <summary>
        /// Handle an exception
        /// </summary>
        private void HandleException(Exception ex)
        {
            messageHelper.ShowError(ex.GetBaseException().Message);
            CanInteract = true;
        }

        /// <summary>
        /// Save the token and close the window
        /// </summary>
        private void SaveTokenAndClose(string createdToken)
        {
            token = createdToken;
            dialog.DialogResult = true;
            dialog.Close();
        }
    }
}