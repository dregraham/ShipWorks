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
    public partial class ChannelAdvisorStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreSettingsControl()
        {
            InitializeComponent();
            
            attributes.AddButtonText = "Add Attributes";
        }

        /// <summary>
        /// Populate the UI with values from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }

            XDocument attributesToDownload = XDocument.Parse(caStore.AttributesToDownload);
            attributes.LoadValues(attributesToDownload.Descendants("Attribute").Select(a => a.Value));
        }

        /// <summary>
        /// Save the UI settings to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new InvalidOperationException("A non Channel Advisor store was passed to the Channel Advisor store settings control.");
            }
            
            caStore.AttributesToDownload = GenerateAttributeXml();

            return true;
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
