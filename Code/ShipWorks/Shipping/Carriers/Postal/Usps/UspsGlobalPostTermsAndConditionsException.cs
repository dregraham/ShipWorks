using System;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Exception thrown when GlobalPost terms and conditions need to be accepted before processing
    /// </summary>
    public class UspsGlobalPostTermsAndConditionsException : UspsException, ITermsAndConditionsException
    {
        private readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsGlobalPostTermsAndConditionsException(UspsAccountEntity account, string message) :
            base(message)
        {
            this.account = account;
        }

        /// <summary>
        /// Create a dialog that will allow a customer to accept the terms and conditions for GlobalPost
        /// </summary>
        public void OpenTermsAndConditionsDlg(ILifetimeScope lifetimeScope)
        {
            UspsWebClient webClient = (UspsWebClient) lifetimeScope.Resolve<UspsShipmentType>().CreateWebClient();
            string url = webClient.GetUrl(account, UrlType.SetTermsGeneral);

            try
            {
                Thread thread = new Thread(ExceptionMonitor.WrapThread(() => OpenKioskBrowser(url)));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch (Exception ex)
            {
                IMessageHelper messageHelper = lifetimeScope.Resolve<IMessageHelper>();
                messageHelper.ShowError($"Unable to open Terms and Conditions page. {ex.Message}");
            }
        }

        /// <summary>
        /// Open the url in a browser in kiosk mode
        /// </summary>
        private static void OpenKioskBrowser(string url)
        {
            using (WebBrowser wb = new WebBrowser())
            {
                wb.Url = new Uri("about:blank");
                wb.Document?.Window?.OpenNew(url, "location=no,menubar=no,scrollbars=yes,status=yes,toolbar=no,resizable=yes");
            }
        }

    }
}
