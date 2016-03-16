using System;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Net;
using System.Xml;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Custom Magento account settings control
    /// </summary>
    public partial class MagentoAccountSettingsControl : GenericStoreAccountSettingsControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoAccountSettingsControl()
        {
            InitializeComponent();

            MagentoStoreType store = (MagentoStoreType) StoreTypeManager.GetType(StoreTypeCode.Magento);

            helpLink.Url = store.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// Load magento-specific features
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            base.LoadStore(store);

            MagentoStoreEntity magentoStore = (MagentoStoreEntity)store;
            storeCodeTextBox.Text = magentoStore.ModuleOnlineStoreCode;

           

            switch ((MagentoVersion)magentoStore.MagentoVersion)
            {
                case MagentoVersion.PhpFile:
                    radioModuleDirect.Checked = true;
                    break;

                case MagentoVersion.MagentoConnect:
                    radioMagentoConnect.Checked = true;
                    break;

                case MagentoVersion.MagentoTwo:
                    radioMagentoTwo.Checked = true;
                    break;

                default:
                    throw new NotImplementedException("Unknown Magento Version");
            }

        }

        /// <summary>
        /// Save magento-specific features
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            MagentoStoreEntity magentoStore = (MagentoStoreEntity) store;
            magentoStore.ModuleOnlineStoreCode = storeCodeTextBox.Text;

            if (radioModuleDirect.Checked)
            {
                magentoStore.MagentoVersion = (int) MagentoVersion.PhpFile;
            }
            else if (radioMagentoConnect.Checked)
            {
                magentoStore.MagentoVersion = (int) MagentoVersion.MagentoConnect;
            }
            else if (radioMagentoTwo.Checked)
            {
                magentoStore.MagentoVersion = (int) MagentoVersion.MagentoTwo;
            }

            return base.SaveToEntity(store);
        }

        /// <summary>
        /// Show a custom connectivity error message
        /// </summary>
        protected override void ShowConnectionException(GenericStoreException ex)
        {
            WebException webEx = ex.InnerException as WebException;
            HttpWebResponse webResponse = webEx?.Response as HttpWebResponse;
            if (webResponse?.StatusCode == HttpStatusCode.NotFound)
            {
                MessageHelper.ShowError(this, "The ShipWorks module was not found at the Module URL specified." +
                                              "\n\nEnsure the Add Secret Keys to URLs setting in your Magento store is set to No.  This is " + 
                                              "a common cause of this error.");

                return;
            }

            InvalidSoapException invalidSoapEx = ex.InnerException as InvalidSoapException;
            if (invalidSoapEx != null && radioMagentoConnect.Checked)
            {
                MessageHelper.ShowError(this, "ShipWorks could not connect to Magento with the URL provided.\n\n" +
                        "Things to try:\n\n" +
                        "1) Change the Magento Connection option to 'I use Magento Community or Enterprise Edition'.\n" +
                        "  or\n" +
                        "2) Ensure you installed the ShipWorks Magento Extension, and properly configured access roles.");

                return;
            }

            XmlException xmlEx = ex.InnerException as XmlException;
            if (xmlEx != null && radioModuleDirect.Checked)
            {
                MessageHelper.ShowError(this, "ShipWorks could not connect to Magento with the module URL provided.\n\n" +
                        "Things to try:\n\n" +
                        "1) Change the Magento Connection option to 'I use Magento Go'.\n" +
                        "  or\n" +
                        "2) Ensure you have uploaded the ShipWorks Magento module, and that the provided URL is correct.");

                return;
            }

            if (ex.Message == "Access denied.")
            {
                MessageHelper.ShowError(this, "The username and password are not correct.");

                return;
            }

            // fall back to base error message
            base.ShowConnectionException(ex);
        }

        /// <summary>
        /// Determines if the connection needs to be re-verified/tested
        /// </summary>
        protected override bool ConnectionVerificationNeeded(GenericModuleStoreEntity genericStore)
        {
            if (genericStore.Fields[(int)MagentoStoreFieldIndex.MagentoVersion].IsChanged)
            {
                return true;
            }
            return base.ConnectionVerificationNeeded(genericStore);
        }
    }
}
