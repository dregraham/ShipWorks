using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseUniquenessSettingsDlg : Form
    {
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
        private void SaveShipSenseConfiguration()
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

            // Update the ShipSense configuration and save it back
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ShipSenseUniquenessXml = uniquenessXml.ToString();

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSave(object sender, EventArgs e)
        {
            // Present the user with a confirmation message box describing what will happen and only save the settings if Yes is chosen
            const string ConfirmationText = @"Adjusting the ShipSense configuration could result in ShipSense " +
                                            "not being able to recognize how orders should be shipped until it has recognized your shipping patterns based on the new configuration.\n\n" +
                                            "Do you wish to continue?";

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Question, MessageBoxButtons.YesNo, ConfirmationText);

            if (result == DialogResult.Yes)
            {
                SaveShipSenseConfiguration();
                Close();
            }
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
