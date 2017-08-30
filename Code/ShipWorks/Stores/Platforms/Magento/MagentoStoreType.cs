using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Platforms.Magento.WizardPages;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Magento)]
    [Component(RegistrationType.Self)]
    public class MagentoStoreType : GenericModuleStoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MagentoStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// Identifies the store type
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Magento;

        /// <summary>
        /// Log request/responses as Magento
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Magento;

        /// <summary>
        /// The url to support article
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000049745";

        /// <summary>
        /// Gets the magento version for this store instance
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Can not get Magento version for a non-Magento store</exception>
        private MagentoVersion MagentoVersion
        {
            get
            {
                MagentoStoreEntity magentoStore = Store as MagentoStoreEntity;

                if (magentoStore == null)
                {
                    throw new InvalidOperationException("Can not get Magento version for a non-Magento store");
                }

                return (MagentoVersion) magentoStore.MagentoVersion;
            }
        }

        /// <summary>
        /// Create the magento-specific store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            MagentoStoreEntity magentoStore = new MagentoStoreEntity
            {
                MagentoTrackingEmails = false,
                MagentoVersion = (int) MagentoVersion.PhpFile
            };

            InitializeStoreDefaults(magentoStore);

            return magentoStore;
        }

        /// <summary>
        /// Create an order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new MagentoOrderEntity
            {
                MagentoOrderID = 0
            };
        }

        /// <summary>
        /// Creates an order identifier that will locate the order provided in the database.
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            string[] splitCompleteOrderNumber = order.OrderNumberComplete.Split(new[] { order.OrderNumber.ToString() }, StringSplitOptions.None);

            return new MagentoOrderIdentifier(order.OrderNumber, splitCompleteOrderNumber[0], splitCompleteOrderNumber[1]);
        }

        /// <summary>
        /// Create the custom wizard pages for Magento
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope) => CreateAddStoreWizardPagesViaIoC(scope);

        /// <summary>
        /// Create the control used to create online update actions in the add store wizard
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl() =>
            new MagentoOnlineUpdateActionControl(MagentoVersion == MagentoVersion.MagentoTwo);

        /// <summary>
        /// Create the user control used in the Store Manager window.
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl() => CreateAccountSettingsControlViaIoC();

        /// <summary>
        /// Create the magento-custom control for store options
        /// </summary>
        public override StoreSettingsControlBase CreateStoreSettingsControl() => new MagentoStoreSettingsControl();

        /// <summary>
        /// Create a magento online updater
        /// </summary>
        public override GenericStoreOnlineUpdater CreateOnlineUpdater()
        {
            if (MagentoVersion == MagentoVersion.MagentoTwoREST)
            {
                return (GenericStoreOnlineUpdater)
                    IoC.UnsafeGlobalLifetimeScope.ResolveKeyed<Owned<IMagentoOnlineUpdater>>(
                        MagentoVersion.MagentoTwoREST,
                        new TypedParameter(typeof(GenericModuleStoreEntity), Store)).Value;
            }

            return new MagentoOnlineUpdater((GenericModuleStoreEntity) Store);
        }

        /// <summary>
        /// Create a custom web client for magento
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            MagentoStoreEntity magentoStore = Store as MagentoStoreEntity;

            if (magentoStore == null)
            {
                throw new InvalidOperationException("Not a magento store.");
            }

            switch (MagentoVersion)
            {
                case MagentoVersion.PhpFile:
                    return new MagentoWebClient(magentoStore);

                case MagentoVersion.MagentoConnect:
                    // for connecting to our Magento Connect Extension via SOAP
                    return new MagentoConnectWebClient(magentoStore);

                case MagentoVersion.MagentoTwo:
                    return new MagentoTwoWebClient(magentoStore);

                default:
                    throw new NotImplementedException("Magento Version not supported");
            }
        }

        /// <summary>
        /// If the store is Magento Two rest don't initialize from onlinemodule
        /// </summary>
        public override void InitializeFromOnlineModule()
        {
            MagentoStoreEntity magentoStore = Store as MagentoStoreEntity;

            if (magentoStore == null)
            {
                throw new InvalidOperationException("Not a magento store.");
            }

            if (MagentoVersion == MagentoVersion.MagentoTwoREST)
            {
                magentoStore.SchemaVersion = "1.1.0.0";
                return;
            }

            base.InitializeFromOnlineModule();
        }

        /// <summary>
        /// Customize what columns we support based on the configured properties of the generic store
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (MagentoVersion == MagentoVersion.MagentoTwoREST &&
                (column == OnlineGridColumnSupport.OnlineStatus || column == OnlineGridColumnSupport.LastModified))
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }
    }
}
