﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    /// <summary>
    /// Wizard page for entering MarketplaceAdvisor account credentials
    /// </summary>
    public partial class MarketplaceAdvisorAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            // Validate
            if (username.Text.Trim().Length == 0 || password.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter your MarketplaceAdvisor username and password.");
                e.NextPage = this;
                return;
            }

            store.Username = username.Text.Trim();
            store.Password = SecureText.Encrypt(password.Text, store.Username);
            store.AccountType = (int) GetAccountTypeFromUI();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Create the processing client
                MarketplaceAdvisorLegacyClient client = new MarketplaceAdvisorLegacyClient(store);


                // Process the request
                XmlDocument xmlResponse = client.GetUser();
                XPathNavigator xpath = xmlResponse.CreateNavigator();

                // Try to find the primary account
                XPathNavigator user = xpath.SelectSingleNode("//User");

                // If there are no accounts, it is an error
                if (user == null)
                {
                    throw new MarketplaceAdvisorException("You have no marketplace accounts registered in MarketplaceAdvisor.");
                }

                store.StoreName = "MarketplaceAdvisor Store";
                store.Company = XPathUtility.Evaluate(user, "FirstName", "") + " " + XPathUtility.Evaluate(user, "LastName", "");
                store.Email = XPathUtility.Evaluate(user, "EmailAddress", "");
                store.Street1 = XPathUtility.Evaluate(user, "Address1", "");
                store.Street2 = XPathUtility.Evaluate(user, "Address2", "");
                store.City = XPathUtility.Evaluate(user, "City", "");
                store.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(user, "State", ""));
                store.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(user, "Country", ""));
                store.PostalCode = XPathUtility.Evaluate(user, "ZipCode", "");
                store.Phone = XPathUtility.Evaluate(user, "PhoneNumber", "");
                store.Fax = XPathUtility.Evaluate(user, "FaxNumber", "");
            }
            catch (MarketplaceAdvisorException ex)
            {
                MessageHelper.ShowError(this,
                    "An error occurred while connecting to the server:\n\n" + ex.Message);

                e.NextPage = this;
            }
        }

        /// <summary>
        /// Get the account type based on what's selected in the UI
        /// </summary>
        private MarketplaceAdvisorAccountType GetAccountTypeFromUI()
        {
            if (radioCorporate.Checked)
            {
                return MarketplaceAdvisorAccountType.LegacyCorporate;
            }

            if (radioStandard.Checked)
            {
                return MarketplaceAdvisorAccountType.LegacyStandard;
            }

            return MarketplaceAdvisorAccountType.OMS;
        }
    }
}
