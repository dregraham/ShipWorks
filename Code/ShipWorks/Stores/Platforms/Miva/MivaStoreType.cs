using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Platforms.Miva.WizardPages;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data.Model;
using ShipWorks.Data;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Miva Merchant store type
    /// </summary>
    public class MivaStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Minimum required module version for this version of ShipWorks
        /// </summary>
        public override Version GetRequiredModuleVersion()
        {
            return new Version("3.0.0");
        }

        /// <summary>
        /// Identifies the store type
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Miva;

        /// <summary>
        /// Log request/responses as Magento
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Miva;

        /// <summary>
        /// Create an instance of the MivaStoreType, which derives form Generic
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            MivaStoreEntity store = new MivaStoreEntity();

            InitializeStoreDefaults(store);

            // default ResponseEncoding to the most likely value for miva
            store.ModuleResponseEncoding = (int) GenericStoreResponseEncoding.Latin1;

            return store;
        }

        /// <summary>
        /// Creates a miva-specific OrderItemAttribute
        /// </summary>
        public override OrderItemAttributeEntity CreateOrderItemAttributeInstance()
        {
            MivaOrderItemAttributeEntity entity = new MivaOrderItemAttributeEntity();

            // defaults
            entity.MivaAttributeID = 0;
            entity.MivaAttributeCode = "";
            entity.MivaOptionCode = "";

            return entity;
        }

        /// <summary>
        /// Create the wizard pages used to create the store
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage> 
                {
                    new MivaModuleQuestionPage(),
                    new MivaModuleInstallPage(),
                    new MivaModuleUrlPage(),
                    new MivaModuleLoginPage(),
                    new MivaSelectStorePage(),
                    new MivaOptionsPage()
                };
        }

        /// <summary>
        /// Create the MivaWebClient
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            return new MivaWebClient((MivaStoreEntity) Store);
        }

        /// <summary>
        /// Create the downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new MivaDownloader((MivaStoreEntity) Store);
        }

        /// <summary>
        /// Read the capabilities of the given module
        /// </summary>
        protected override GenericModuleCapabilities ReadModuleCapabilities(GenericModuleResponse webResponse)
        {
            GenericModuleCapabilities caps = base.ReadModuleCapabilities(webResponse);
            MivaStoreEntity store = (MivaStoreEntity) Store;

            // Don't use what was read from the module for OnlineStatus and OnlineShipment... those come from the Sebenza setting for miva
            caps.OnlineStatusSupport = (GenericOnlineStatusSupport) store.ModuleOnlineStatusSupport;
            caps.OnlineStatusDataType = (GenericVariantDataType) store.ModuleOnlineStatusDataType;
            caps.OnlineShipmentDetails = store.ModuleOnlineShipmentDetails;

            return caps;
        }

        /// <summary>
        ///  Read the communications details of the given module
        /// </summary>
        protected override GenericModuleCommunications ReadModuleCommunications(GenericModuleResponse webResponse)
        {
            GenericModuleCommunications communications = base.ReadModuleCommunications(webResponse);

            // we ignore whatever ResponseEncoding may come down from the module, and go from the store configuration
            communications.ResponseEncoding = (GenericStoreResponseEncoding) ((GenericModuleStoreEntity) Store).ModuleResponseEncoding;

            return communications;
        }

        /// <summary>
        /// Create the UserControl used to display account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new MivaAccountSettingsControl();
        }

        /// <summary>
        /// Create the UserControl used to provide store settings
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl()
        {
            return new MivaStoreSettingsControl();
        }

        /// <summary>
        /// Create the UserControl used to provide manual order settings
        /// </summary>
        public override ManualOrderSettingsControl CreateManualOrderSettingsControl()
        {
            return new MivaManualOrderSettingsControl();
        }

        /// <summary>
        /// Generate a new manual order number and assign it to the given order
        /// </summary>
        public override void GenerateManualOrderNumber(OrderEntity order)
        {
            MivaStoreEntity miva = (MivaStoreEntity) Store;

            if (miva.LiveManualOrderNumbers)
            {
                try
                {
                    MivaWebClient webClient = new MivaWebClient(miva);
                    order.OrderNumber = webClient.GetNextOrderID();
                }
                catch (GenericStoreException ex)
                {
                    // What the method is documented as supposed to throw
                    throw new NotSupportedException(ex.Message, ex);
                }
            }
            else
            {
                base.GenerateManualOrderNumber(order);
            }
        }

        /// <summary>
        /// This is a string that uniquely identifies the store for licensing
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                MivaStoreEntity miva = (MivaStoreEntity) Store;

                string identifier = miva.ModuleUrl.ToLower();

                int index = identifier.IndexOf("merchant2");

                if (index == -1)
                {
                    index = identifier.IndexOf("mm5");
                }

                if (index != -1)
                {
                    identifier = identifier.Substring(0, index);
                }

                if (miva.ModuleOnlineStoreCode != "")
                {
                    identifier += "?" + miva.ModuleOnlineStoreCode;
                }

                return identifier;
            }
        }

        /// <summary>
        /// Generate Miva specific order item attribute output XML
        /// </summary>
        public override void GenerateTemplateOrderItemAttributeElements(ElementOutline container, Func<OrderItemAttributeEntity> optionSource)
        {
            var mivaOption = new Lazy<MivaOrderItemAttributeEntity>(() => optionSource() as MivaOrderItemAttributeEntity);

            ElementOutline miva = container.AddElement("Miva");
            miva.AddElement("OptionCode", () => mivaOption.Value.MivaOptionCode);
            miva.AddElement("AttributeCode", () => mivaOption.Value.MivaAttributeCode);
            miva.AddElement("AttributeID",() => mivaOption.Value.MivaAttributeID);
        }

        /// <summary>
        /// Generate Miva specific order output XML
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var notes = new Lazy<List<NoteEntity>>(() => DataProvider.GetRelatedEntities(orderSource().OrderID, EntityType.NoteEntity).Cast<NoteEntity>().ToList());

            ElementOutline miva = container.AddElement("Miva");
            ElementOutline sebenza = miva.AddElement("Sebenza");

            for (int i = 1; i <= 3; i++)
            {
                string notePrefix = string.Format("ACD{0}: ", i);

                sebenza.AddElement("Add" + i, () =>
                    {
                        string content = "";

                        NoteEntity note = notes.Value.FirstOrDefault(n => n.Text.StartsWith(notePrefix));
                        if (note != null)
                        {
                            content = note.Text.Replace(notePrefix, "");
                        }

                        return content;
                    });
            }
        }

        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129335";
    }
}
