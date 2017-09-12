using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Implementation of the AmeriCommerce store integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.AmeriCommerce)]
    [Component(RegistrationType.Self)]
    public class AmeriCommerceStoreType : StoreType
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceStoreType));
        private readonly IAmeriCommerceOnlineUpdater onlineUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceStoreType(StoreEntity store, IAmeriCommerceOnlineUpdater onlineUpdater)
            : base(store)
        {
#pragma warning disable 168,219
            // This is just to prove RewardsPoints and PaymentOrder exists. If it doesn't the WSDL has been updated and RewardPoints and PaymentOrder aren't
            // hacked into the reference.cs file any more.
            var w = PaymentTypes.BillMeLater;
            var x = PaymentTypes.RewardPoints;
            var y = OrderType.PaymentOrder;
            var z = OrderItemType.OrderPayment;
#pragma warning restore 168,219

            this.onlineUpdater = onlineUpdater;
        }

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.AmeriCommerce; }
        }

        /// <summary>
        /// License Identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                AmeriCommerceStoreEntity amcStore = (AmeriCommerceStoreEntity) Store;

                // combination of the store url and chosen store code
                return String.Format("{0}?{1}", amcStore.StoreUrl, amcStore.StoreCode);
            }
        }

        /// <summary>
        /// The initial download restriction policy for AmeriCommerce
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Determines if certain columns should be visible or not in the grid
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column)
        {
            if (column == OnlineGridColumnSupport.OnlineStatus ||
                column == OnlineGridColumnSupport.LastModified)
            {
                return true;
            }

            return base.GridOnlineColumnSupported(column);
        }

        /// <summary>
        /// Create a new store entity instance
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            AmeriCommerceStoreEntity store = new AmeriCommerceStoreEntity();

            InitializeStoreDefaults(store);

            store.StoreCode = 0;
            store.StoreUrl = "";
            store.Username = "";
            store.Password = "";
            store.StatusCodes = "";

            return store;
        }

        /// <summary>
        /// Returns the collection of possible online status codes
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices()
        {
            AmeriCommerceStatusCodeProvider provider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity) Store);

            return provider.CodeNames;
        }

        /// <summary>
        /// Create the setup wizard pages needed to configure the store
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                new WizardPages.AmeriCommerceAccountPage(),
                new WizardPages.AmeriCommerceStoreSelectionPage()
            };
        }

        /// <summary>
        /// Create the online update action creation control that will get shown in the wizard
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new WizardPages.AmeriCommerceOnlineUpdateActionControl();
        }

        /// <summary>
        /// Order identifier/locater
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            return new OrderNumberIdentifier(order.OrderNumber);
        }

        /// <summary>
        /// Create the control for editing account information
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new AmeriCommerceAccountSettingsControl();
        }
    }
}
