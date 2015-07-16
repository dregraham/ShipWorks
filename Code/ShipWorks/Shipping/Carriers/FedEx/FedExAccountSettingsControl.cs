using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Api;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the settings of FedEx account
    /// </summary>
    public partial class FedExAccountSettingsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the account into the control
        /// </summary>
        public void LoadAccount(FedExAccountEntity account)
        {
            signatureAuth.Text = account.SignatureRelease;

            XElement root = XElement.Parse(account.SmartPostHubList);

            var hubs = root.Descendants("HubID");
            if (hubs.Count() > 0)
            {
                // The first is the default
                hubID.Text = (string) hubs.First();

                // Additional hubs
                additionalHubs.Lines = hubs.Skip(1).Select(e => (string) e).ToArray();
            }
        }

        /// <summary>
        /// Save the settings from the account into the control
        /// </summary>
        public void SaveToAccount(FedExAccountEntity account)
        {
            account.SignatureRelease = signatureAuth.Text;

            XElement root = new XElement("Root");

            if (hubID.Text.Trim().Length > 0)
            {
                if (!Regex.IsMatch(hubID.Text, @"^\d+$"))
                {
                    throw new CarrierException("Hub ID must be all numbers.");
                }
                
                root.Add(new XElement("HubID", hubID.Text.Trim()));
            }

            foreach (string hubLine in additionalHubs.Lines.Select(l => l.Trim()).Where(l => l.Length > 0))
            {
                if (!Regex.IsMatch(hubLine, @"^\d+$"))
                {
                    throw new CarrierException("Hub ID must be all numbers.");
                }
                root.Add(new XElement("HubID", hubLine));
            }

            account.SmartPostHubList = root.ToString();
        }
    }
}
