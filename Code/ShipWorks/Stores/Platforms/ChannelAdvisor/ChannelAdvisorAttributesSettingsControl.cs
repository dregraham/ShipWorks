using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// CA-specific store settings
    /// </summary>
    [ToolboxItem(true)]
    public partial class ChannelAdvisorAttributesSettingsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorAttributesSettingsControl()
        {
            InitializeComponent();
            
            attributes.AddButtonText = "Add Attributes";
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public void LoadStore(ChannelAdvisorStoreEntity caStore)
        {
            XDocument attributesToDownload = XDocument.Parse(caStore.AttributesToDownload);
            attributes.LoadValues(attributesToDownload.Descendants("Attribute").Select(a => a.Value));
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public void SaveToEntity(ChannelAdvisorStoreEntity caStore)
        {
            caStore.AttributesToDownload = GenerateAttributeXml();
        }

        /// <summary>
        /// Generates the attribute XML.
        /// </summary>
        private string GenerateAttributeXml()
        {
            XElement attributesXml = new XElement("Attributes");

            foreach (string attributeName in attributes.Values)
            {
                attributesXml.Add(new XElement("Attribute", attributeName));
            }

            return attributesXml.ToString();
        }
    }
}
