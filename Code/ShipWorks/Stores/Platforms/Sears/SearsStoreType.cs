using System;
using System.Collections.Generic;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Sears.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Sears.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Sears specific ShipWorks store type implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Sears)]
    [Component(RegistrationType.Self)]
    public class SearsStoreType : StoreType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SearsStoreType));

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsStoreType(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Returns the identifying code for Infopia stores
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Sears;

        /// <summary>
        /// Create a new instance of a sears store
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            SearsStoreEntity store = new SearsStoreEntity();
            InitializeStoreDefaults(store);

            store.StoreName = "Sears";
            store.SearsEmail = "";
            store.Password = "";

            return store;
        }

        /// <summary>
        /// Create a Sears specific order instance
        /// </summary>
        protected override OrderEntity CreateOrderInstance() => new SearsOrderEntity();

        /// <summary>
        /// Create a sears specific item instance
        /// </summary>
        public override OrderItemEntity CreateOrderItemInstance() => new SearsOrderItemEntity();

        /// <summary>
        /// Create an identifier to uniquely identify a sears order
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            ISearsOrderEntity searsOrder = order as ISearsOrderEntity;
            if (searsOrder == null)
            {
                throw new InvalidOperationException("A non sears order was passed to the SearsStoreType.");
            }

            return new SearsOrderIdentifier(searsOrder.OrderNumber, searsOrder.PoNumber);
        }

        /// <summary>
        /// Get a description for use when auditing an order
        /// </summary>
        public override string GetAuditDescription(IOrderEntity order) =>
            (order as ISearsOrderEntity)?.PoNumber ?? string.Empty;

        /// <summary>
        /// Create the wizard pages for adding sears.com stores to ShipWorks
        /// </summary>
        /// <param name="scope"></param>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
                {
                    new WizardPages.SearsAccountPage()
                };
        }

        /// <summary>
        /// Create the account settings control for managing sears.com account settings
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new SearsAccountSettingsControl();
        }

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new OnlineUpdateShipmentUpdateActionControl(typeof(SearsShipmentUploadTask));
        }

        /// <summary>
        /// The initial download restriction policy for Sears
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy =>
            new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack) { MaxDaysBack = 90 };

        /// <summary>
        /// Determines if certain columns should be visible or not in the grid
        /// </summary>
        public override bool GridOnlineColumnSupported(OnlineGridColumnSupport column) =>
            column == OnlineGridColumnSupport.OnlineStatus || base.GridOnlineColumnSupported(column);

        /// <summary>
        /// Create the license identifier for uniquely identifying Sears.com stores
        /// </summary>
        protected override string InternalLicenseIdentifier => ((SearsStoreEntity) Store).SearsEmail;

        /// <summary>
        /// Create the condition group for searching on Amazon Order ID
        /// </summary>
        public override ConditionGroup CreateBasicSearchOrderConditions(string search)
        {
            ConditionGroup group = new ConditionGroup();

            SearsPoNumberCondition condition = new SearsPoNumberCondition();
            condition.TargetValue = search;
            condition.Operator = StringOperator.BeginsWith;
            group.Conditions.Add(condition);

            return group;
        }

        /// <summary>
        /// Create the order elements for the order provided
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<SearsOrderEntity>(() => (SearsOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("Sears");
            outline.AddElement("PoNumber", () => order.Value.PoNumber);
            outline.AddElement("PoNumberWithDate", () => order.Value.PoNumberWithDate);
            outline.AddElement("LocationID", () => order.Value.LocationID);
            outline.AddElement("Commission", () => order.Value.Commission);
            outline.AddElement("CustomerPickup", () => order.Value.CustomerPickup);
        }

        /// <summary>
        /// Create the Order Item Xml for the order item provided
        /// </summary>
        public override void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource)
        {
            var item = new Lazy<SearsOrderItemEntity>(() => (SearsOrderItemEntity) itemSource());

            ElementOutline outline = container.AddElement("Sears");
            outline.AddElement("OnlineStatus", () => item.Value.OnlineStatus);
            outline.AddElement("Shipping", () => item.Value.Shipping);
            outline.AddElement("Commission", () => item.Value.Commission);
        }

        /// <summary>
        /// Return all the Online Status options that apply to this store. This is used to populate the drop-down in the
        /// Online Status filter.
        /// </summary>
        public override ICollection<string> GetOnlineStatusChoices() =>
            new[] { "New", "Open", "Closed", "Overdue" };
    }
}
