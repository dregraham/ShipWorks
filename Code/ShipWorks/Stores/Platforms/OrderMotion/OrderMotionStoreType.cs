using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Email.Accounts;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.OrderMotion.WizardPages;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Store integration for OrderMotion
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.OrderMotion)]
    [Component(RegistrationType.Self)]
    public class OrderMotionStoreType : StoreType
    {
        static readonly ILog log = LogManager.GetLogger(typeof(OrderMotionStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Identifying typecode for OrderMotion
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.OrderMotion;

        /// <summary>
        /// Identifying account information for licensing
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {
                EmailAccountEntity account = EmailAccountManager.GetAccount(((OrderMotionStoreEntity) Store).OrderMotionEmailAccountID);

                // If the account was deleted we have to create a made up license that obviously will not be activated to them
                if (account == null)
                {
                    return string.Format("{0}@noaccount.com", Guid.NewGuid());
                }

                return account.EmailAddress;
            }
        }

        /// <summary>
        /// Creates a store entity for this store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            OrderMotionStoreEntity store = new OrderMotionStoreEntity();

            InitializeStoreDefaults(store);

            // set specific defaults
            store.OrderMotionBizID = "";
            store.OrderMotionEmailAccountID = 0;

            return store;
        }

        /// <summary>
        /// Create an OrderMotion order
        /// </summary>
        protected override OrderEntity CreateOrderInstance()
        {
            return new OrderMotionOrderEntity();
        }

        /// <summary>
        /// Create the order identifier to locate orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            IOrderMotionOrderEntity omOrder = order as IOrderMotionOrderEntity;

            return new OrderMotionOrderIdentifier(omOrder.OrderNumber, omOrder.OrderMotionShipmentID);
        }

        /// <summary>
        /// Create the Add Store wizard pages
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>
            {
                new OrderMotionAccountPage(),
                new OrderMotionEmailPage()
            };
        }

        /// <summary>
        /// Create Account Settings Control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new OrderMotionAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for configuring task creation during the setup wizard.
        /// </summary>
        /// <returns></returns>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OrderMotionOnlineUpdateActionControl();
        }

        /// <summary>
        /// Generate OrderMotion specific template xml elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<OrderMotionOrderEntity>(() => (OrderMotionOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("OrderMotion");

            // Outputting both elements despite using the same value for historical purposes so existing templates
            // do not break. The shipment number value was originally built by combining the order motion ID and
            // the order motion shipment ID values which were parsed out of the INVOICE_NO field from the order download.
            outline.AddElementLegacy2x("ShipmentNumber", () => order.Value.OrderMotionInvoiceNumber);
            outline.AddElement("InvoiceNumber", () => order.Value.OrderMotionInvoiceNumber);

            outline.AddElement("Promotion", () => order.Value.OrderMotionPromotion);
        }
    }
}
