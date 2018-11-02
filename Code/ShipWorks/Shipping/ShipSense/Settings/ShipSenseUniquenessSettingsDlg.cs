using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseUniquenessSettingsDlg : Form
    {
        private XElement originalUniquenessXml;

        // Present the user with a confirmation message box describing what will happen and only save the settings if Yes is chosen
        private const string ConfirmationText = @"Adjusting the ShipSense configuration could result in ShipSense " +
                                        "not being able to know how orders should be shipped until it has recognized your shipping patterns based on the new configuration.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseUniquenessSettingsDlg"/> class.
        /// </summary>
        public ShipSenseUniquenessSettingsDlg()
        {
            InitializeComponent();

            LoadShipSenseConfiguration();
        }

        /// <summary>
        /// Loads the ShipSense configuration.
        /// </summary>
        private void LoadShipSenseConfiguration()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            XElement uniquenessXml = XElement.Parse(settings.ShipSenseUniquenessXml);
            originalUniquenessXml = XElement.Parse(settings.ShipSenseUniquenessXml);

            // Load the selected properties
            XElement propertyElement = uniquenessXml.Descendants("ItemProperty").FirstOrDefault();
            if (propertyElement != null)
            {
                configurationControl.LoadItemPropertyControl(propertyElement.Descendants("Name").Select(e => e.Value));
            }

            // Load the attribute names
            XElement attributeElement = uniquenessXml.Descendants("ItemAttribute").FirstOrDefault();
            if (attributeElement != null)
            {
                configurationControl.LoadAttributeControls(attributeElement.Descendants("Name").Select(e => e.Value));
            }
        }

        /// <summary>
        /// Builds an XML document based on the data elements in the configuration control that is
        /// stored in the ShippingSettings.ShipSenseUniquenessXml  and saves the configuration
        /// to the database. The XML is in the format of:
        /// <ShipSenseUniqueness><ItemAttribute><Name>...</Name><Name>...</Name></ItemAttribute></ShipSenseUniqueness>
        /// </summary>
        private XElement GenerateShipSenseConfiguration()
        {
            // The XML will be in the following format: <ShipSenseUniqueness><ItemAttribute><Name>...</Name><Name>...</Name></ItemAttribute></ShipSenseUniqueness>
            XElement uniquenessXml = new XElement("ShipSenseUniqueness");

            // Build up the item property node with the selected property values
            XElement itemPropertyElement = new XElement("ItemProperty");
            uniquenessXml.Add(itemPropertyElement);
            foreach (string propertyname in configurationControl.SelectedPropertyNames)
            {
                itemPropertyElement.Add(new XElement("Name", propertyname));
            }

            // Build up the item attribute node with the attribute names
            XElement itemAttributeElement = new XElement("ItemAttribute");
            uniquenessXml.Add(itemAttributeElement);

            foreach (string attributeName in configurationControl.AttributeNames)
            {
                itemAttributeElement.Add(new XElement("Name", attributeName));
            }

            return uniquenessXml;
        }

        /// <summary>
        /// Save the ShipSense configuration to the ShippingSettings table.
        /// </summary>
        private static void SaveShipSenseConfiguration(XElement uniquenessXml)
        {
            // Update the ShipSense configuration and save it back
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ShipSenseUniquenessXml = uniquenessXml.ToString();
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Determines if the original settings and newly saved settings are equal
        /// </summary>
        private bool AreSettingsEqual(XElement newUniquenessElement)
        {
            IEnumerable<XElement> newSettings = newUniquenessElement.Descendants("Name").ToList();
            IEnumerable<XElement> oldSettings = originalUniquenessXml.Descendants("Name").ToList();

            IEnumerable<XElement> same = newSettings.Join(oldSettings, newFrom => newFrom.Value.ToUpperInvariant(),
                                                          original => original.Value.ToUpperInvariant(),
                                                          (newFrom, original) => newFrom);

            int newCount = newSettings.Count();
            int origCount = oldSettings.Count();

            return (newCount == origCount) && (origCount == same.Count());
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSave(object sender, EventArgs e)
        {
            // If the user changed the settings, go through the save routine.
            XElement newUniquenessElement = GenerateShipSenseConfiguration();
            if (!AreSettingsEqual(newUniquenessElement))
            {
                DialogResult result = DialogResult.No;
                bool isReloadRequested = false;

                // Confirm that the user actually wants to change the ShipSense settings
                using (ShipSenseConfirmationDlg shipSenseConfirmationDlg = new ShipSenseConfirmationDlg(ConfirmationText))
                {
                    result = shipSenseConfirmationDlg.ShowDialog(this);
                    isReloadRequested = shipSenseConfirmationDlg.IsReloadRequested;
                }

                if (result == DialogResult.Yes)
                {
                    SaveShipSenseConfiguration(newUniquenessElement);

                    if (isReloadRequested)
                    {
                        // A reload was requested, so setup the progress provider and dialog for the
                        // loader to attach to
                        ProgressProvider progressProvider = new ProgressProvider();
                        using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
                        {
                            progressDialog.Title = "Reload ShipSense";
                            progressDialog.Description = "ShipSense is learning from your shipment history.";

                            progressDialog.AutoCloseWhenComplete = false;
                            progressDialog.AllowCloseWhenRunning = false;

                            progressDialog.ActionColumnHeaderText = "ShipSense";
                            progressDialog.CloseTextWhenComplete = "Close";

                            // Kick off the reload process and show the progress dialog
                            ReloadKnowledgebase(progressProvider);
                            progressDialog.ShowDialog(this);
                        }
                    }
                }
            }

            Close();
        }

        /// <summary>
        /// Reloads the ShipSense knowledge base with the latest shipment history. This overloaded
        /// version allows the reload process to attach a progress item to an existing
        /// progress provider.
        /// </summary>
        private static void ReloadKnowledgebase(ProgressProvider progressProvider)
        {
            // Record an entry in the audit log that the KB reload was started
            AuditUtility.Audit(AuditActionType.ReloadShipSenseStarted);

            // Setup dependencies for the progress dialog
            var progressItem = progressProvider.AddItem("Reloading ShipSense");

            // Start the load asynchronously now that everything should be ready to load
            // We MUST ContinueWith and dispose the loader so that the sql connection
            // gets disposed.
            Functional.UsingAsync(IoC.BeginLifetimeScope(),
                scope =>
                {
                    var shippingSettings = scope.Resolve<IShippingSettings>();
                    ShipSenseLoader loader = new ShipSenseLoader(progressItem, shippingSettings);

                    // Indicate that we want to reset the hash keys and prepare the environment for the
                    // load process to begin
                    loader.ResetOrderHashKeys = true;
                    loader.PrepareForLoading();

                    return Task.Factory.StartNew(loader.LoadData).ContinueWith(_ => Unit.Default);
                });
        }

        /// <summary>
        /// Called when the Cancel button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }
    }
}
