using System;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// The view model for the OneBalanceCarrierAccountsControl
    /// </summary>
    [KeyedComponent(typeof(IOneBalanceShowSetupDialogViewModel), ShipmentTypeCode.DhlExpress)]
    public class OneBalanceCarrierAccountsControlViewModel : OneBalanceShowSetupDialogViewModel
    {
        private bool upsEnabled;
        private bool dhlAccountEnabled;
        private readonly IOneBalanceAccountHelper accountHelper;
        private readonly IMessageHelper messageHelper;
        private IUspsWebClient webClient;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private bool remoteDhlEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceCarrierAccountsControlViewModel(IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory,
            IWin32Window window,
            IOneBalanceAccountHelper accountHelper,
            IMessageHelper messageHelper,
            IShipmentTypeManager shipmentTypeManager)
            : base(setupWizardFactory, window)
        {
            this.accountHelper = accountHelper;
            this.messageHelper = messageHelper;
            this.shipmentTypeManager = shipmentTypeManager;
            SetupWebClient();
            Refresh();
        }

        /// <summary>
        /// A flag to indicate if the Dhl Account is still enabled
        /// </summary>
        public bool RemoteDhlEnabled
        {
            get => remoteDhlEnabled;
            set => Set(ref remoteDhlEnabled, value);
        }

        /// <summary>
        /// A flag that indicated if Ups has been enabled on the one balance account
        /// </summary>
        public bool UpsEnabled
        {
            get => upsEnabled;
            set => Set(ref upsEnabled, value);
        }

        /// <summary>
        /// A flag to indicate if the Dhl Account is still enabled
        /// </summary>
        public bool LocalDhlAccountEnabled
        {
            get => dhlAccountEnabled;
            set => Set(ref dhlAccountEnabled, value);
        }

        /// <summary>
        /// Refreshes the account dependent attributes of the controls 
        /// </summary>
        public override void Refresh()
        {
            RemoteDhlEnabled = RemoteDhlAccountEnabled();
            LocalDhlAccountEnabled = accountHelper.LocalDhlAccountExists();
            UpsEnabled = accountHelper.LocalUpsAccountExists();
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClient()
        {
            UspsShipmentType shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.Usps) as UspsShipmentType;
            webClient = shipmentType.CreateWebClient();
        }

        /// <summary>
        /// Get the account from stamps
        /// </summary>
        private bool RemoteDhlAccountEnabled()
        {
            var result = accountHelper.GetUspsAccount();

            if (result.Success)
            {
                try
                {
                    var response = (AccountInfoV41) webClient.GetAccountInfo(result.Value);
                    return response.Capabilities.CanPrintDX;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Show the DHL Express setup wizard
        /// </summary>
        protected override void ShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            if ((!remoteDhlEnabled && AddDhlExpress()) || remoteDhlEnabled)
            {
                base.ShowSetupWizard(shipmentTypeCode);
                Refresh();
            }
        }

        /// <summary>
        /// Make an Api call to stamps to add the account
        /// </summary>
        private bool AddDhlExpress()
        {
            var result = accountHelper.GetUspsAccount();

            if (result.Failure)
            {
                messageHelper.ShowError(result.Exception.Message);
                return false;
            }

            try
            {
                webClient.AddDhlExpress(result.Value);
                return true;
            }
            catch (UspsException ex)
            {
                // We don't need to log the exception because the webclient already does it
                messageHelper.ShowError($"There was an error adding DHL Express to your One Balance account:\n{ex.Message}");
            }

            return false;
        }
    }
}
