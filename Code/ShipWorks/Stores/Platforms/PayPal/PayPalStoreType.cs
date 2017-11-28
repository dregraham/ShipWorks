﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.PayPal.CoreExtensions.Filters;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Implementation of the PayPal store integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.PayPal)]
    [Component(RegistrationType.Self)]
    public class PayPalStoreType : StoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets the typecode for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.PayPal;

        /// <summary>
        /// Unique account identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((PayPalStoreEntity) Store).ApiUserName; }
        }

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        public override List<FilterEntity> CreateInitialFilters()
        {
            List<PayPalPaymentStatus> ShippingStatuses = EnumHelper.GetEnumList<PayPalPaymentStatus>()
              .Select(statusEnumEntry => statusEnumEntry.Value)
              .ToList();

            return CreateInitialFilters<PayPalPaymentStatus, PayPalPaymentStatusCondition>(ShippingStatuses);
        }

        /// <summary>
        /// Store settings control
        /// </summary>
        public override AccountSettingsControlBase CreateAccountSettingsControl()
        {
            return new PayPalAccountSettingsControl();
        }

        /// <summary>
        /// Create the PayPal specific order entity
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity CreateOrderInstance()
        {
            return new PayPalOrderEntity();
        }

        /// <summary>
        /// Create the PayPal specific store entity
        /// </summary>
        public override StoreEntity CreateStoreInstance()
        {
            PayPalStoreEntity storeEntity = new PayPalStoreEntity();

            InitializeStoreDefaults(storeEntity);

            // set defaults
            storeEntity.ApiCredentialType = (short) PayPalCredentialType.Signature;
            storeEntity.ApiUserName = "";
            storeEntity.ApiPassword = "";
            storeEntity.ApiSignature = "";
            storeEntity.LastTransactionDate = SqlDateTime.MinValue.Value;
            storeEntity.LastValidTransactionDate = SqlDateTime.MinValue.Value;

            return storeEntity;
        }

        /// <summary>
        /// Generate OrderMotion specific template xml elements
        /// </summary>
        public override void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource)
        {
            var order = new Lazy<PayPalOrderEntity>(() => (PayPalOrderEntity) orderSource());

            ElementOutline outline = container.AddElement("PayPal");
            outline.AddElement("PaymentStatus", () => EnumHelper.GetDescription((PayPalPaymentStatus) order.Value.PaymentStatus));
            outline.AddElement("TransactionID", () => order.Value.TransactionID);
            outline.AddElement("Fee", () => order.Value.PayPalFee);
            outline.AddElement("AddressStatus", () => EnumHelper.GetDescription((PayPalAddressStatus) order.Value.AddressStatus));

            container.AddElementLegacy2x("PayPalPaymentStatus", () => EnumHelper.GetDescription((PayPalPaymentStatus) order.Value.PaymentStatus));
            container.AddElementLegacy2x("PayPalTransactionID", () => order.Value.TransactionID);
            container.AddElementLegacy2x("PayPalFee", () => order.Value.PayPalFee);
        }

        /// <summary>
        /// Identifies orders
        /// </summary>
        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order)
        {
            IPayPalOrderEntity payPalOrder = order as IPayPalOrderEntity;
            if (payPalOrder == null)
            {
                throw new InvalidOperationException("A non PayPal Order was passed to CreateOrderIdentifier");
            }

            return new PayPalOrderIdentifier(payPalOrder.TransactionID);
        }

        /// <summary>
        /// Get a description for use when auditing an order
        /// </summary>
        public override string GetAuditDescription(IOrderEntity order) =>
            (order as IPayPalOrderEntity)?.TransactionID ?? string.Empty;

        /// <summary>
        /// Create the setup wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages(ILifetimeScope scope)
        {
            return new List<WizardPage>()
            {
                new WizardPages.PayPalCredentialPage()
            };
        }

        /// <summary>
        /// Policy for the initial download
        /// </summary>
        public override InitialDownloadPolicy InitialDownloadPolicy
        {
            get
            {
                return new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack);
            }
        }

        /// <summary>
        /// Gets the default validation setting.
        /// </summary>
        protected override AddressValidationStoreSettingType GetDefaultValidationSetting()
        {
            return AddressValidationStoreSettingType.ValidateAndNotify;
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public static string AccountSettingsHelpUrl =>
            "http://support.shipworks.com/support/solutions/articles/129331-ebay-setup-connecting-paypal-to-shipworks";
    }
}
