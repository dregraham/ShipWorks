using System;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.UI;

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
            UspsShipmentType shipmentType = lifetimeScope.Resolve<UspsShipmentType>();
            UspsWebClient webClient = (UspsWebClient) shipmentType.CreateWebClient();

            string url = webClient.GetUrl(account, UrlType.SetTermsGeneral);

            Type type = Type.GetTypeFromProgID("InternetExplorer.Application");

            dynamic ie = Activator.CreateInstance(type);

            ie.AddressBar = false;
            ie.MenuBar = false;
            ie.ToolBar = false;

            ie.Visible = true;
            ie.Navigate(url);
        }
    }
}
