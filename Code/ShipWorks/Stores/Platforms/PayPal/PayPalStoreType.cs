using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.PayPal.CoreExtensions.Filters;
using ShipWorks.UI.Wizard;
using System.Data.SqlTypes;
using ShipWorks.Templates.Processing.TemplateXml;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Implemenation of the PayPal store integration
    /// </summary>
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
        public override StoreTypeCode TypeCode
        {
            get { return StoreTypeCode.PayPal; }
        }

        /// <summary>
        /// Unique account identifier
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get { return ((PayPalStoreEntity)Store).ApiUserName; }
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
            storeEntity.ApiCredentialType = (short)PayPalCredentialType.Signature;
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
        public override OrderIdentifier CreateOrderIdentifier(OrderEntity order)
        {
            PayPalOrderEntity payPalOrder = order as PayPalOrderEntity;
            if (payPalOrder == null)
            {
                throw new InvalidOperationException("A non PayPal Order was passed to CreateOrderIdentifier");
            }

            return new PayPalOrderIdentifier(payPalOrder.TransactionID);
        }

        /// <summary>
        /// Instantiate the order downloader
        /// </summary>
        public override StoreDownloader CreateDownloader()
        {
            return new PayPalDownloader(Store);
        }

        /// <summary>
        /// Create the setup wizard pages
        /// </summary>
        public override List<WizardPage> CreateAddStoreWizardPages()
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
        public static string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129331-ebay-setup-connecting-paypal-to-shipworks";
    }
}
