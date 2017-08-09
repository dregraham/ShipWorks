﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
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
        /// Gets the online store's order identifier
        /// </summary>
        public int GetOnlineOrderIdentifier(OrderEntity order) => (int) order.OrderNumber;

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        public async Task<IEnumerable<int>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers(order as OrderEntity, 
                "OrderSearch", 
                OrderSearchFields.OrderID == order.OrderID, 
                () => OrderSearchFields.OrderNumber.ToValue<int>()).ConfigureAwait(false);
        }

        /// <summary>
        /// Order identifier/locater
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
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

        #region Online Update Commands

        /// <summary>
        /// Create the menu commands for updating order status
        /// </summary>
        public override IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands()
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // get possible status codes from the provider
            AmeriCommerceStatusCodeProvider codeProvider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity) Store);

            // create a menu item for each status
            bool isOne = false;
            foreach (int codeValue in codeProvider.CodeValues)
            {
                isOne = true;

                MenuCommand command = new MenuCommand(codeProvider.GetCodeName(codeValue), new MenuCommandExecutor(OnSetOnlineStatus));
                command.Tag = codeValue;

                commands.Add(command);
            }

            MenuCommand uploadCommand = new MenuCommand("Upload Shipment Details", new MenuCommandExecutor(OnUploadShipmentDetails));
            uploadCommand.BreakBefore = isOne;
            commands.Add(uploadCommand);

            return commands;
        }

        /// <summary>
        /// Command handler for uploading shipment details
        /// </summary>
        private void OnUploadShipmentDetails(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Upload Shipment Details",
                "ShipWorks is uploading shipment information.",
                "Updating order {0} of {1}...");

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(ShipmentUploadCallback, context.SelectedKeys, null);
        }
        /// <summary>
        /// Worker thread method for uploading shipment details
        /// </summary>
        private void ShipmentUploadCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            try
            {
                // upload
                onlineUpdater.UploadOrderShipmentDetails(Store as IAmeriCommerceStoreEntity, new[]{ orderID });
            }
            catch (AmeriCommerceException ex)
            {
                // log it
                log.ErrorFormat("Error uploading shipment details for orderID {0}: {1}", orderID, ex.Message);

                // add the error to the issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        /// <summary>
        /// Command handler for setting online order status
        /// </summary>
        private void OnSetOnlineStatus(MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
               "Set Status",
               "ShipWorks is setting the online status.",
               "Updating order {0} of {1}...");

            IMenuCommand command = context.MenuCommand;
            int statusCode = (int) command.Tag;

            executor.ExecuteCompleted += (o, e) =>
            {
                context.Complete(e.Issues, MenuCommandResult.Error);
            };

            executor.ExecuteAsync(SetOnlineStatusCallback, context.SelectedKeys, statusCode);
        }

        /// <summary>
        /// Worker thread method for updating online order status
        /// </summary>
        private void SetOnlineStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            log.Debug(Store.StoreName);

            int statusCode = (int) userState;
            try
            {
                onlineUpdater.UpdateOrderStatus(Store as IAmeriCommerceStoreEntity, orderID, statusCode);
            }
            catch (AmeriCommerceException ex)
            {
                // log it
                log.ErrorFormat("Error updating online status of orderID {0}: {1}", orderID, ex.Message);

                // add the error to issues so we can react later
                issueAdder.Add(orderID, ex);
            }
        }

        #endregion
    }
}
