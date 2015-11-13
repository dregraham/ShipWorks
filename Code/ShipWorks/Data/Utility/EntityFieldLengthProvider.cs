using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using ShipWorks.Templates.Tokens;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// An extender provider that can be used to ensure text boxes are set to accept the appropriate amount of characters
    /// </summary>
    [ProvideProperty("MaxLengthSource", typeof(Control))]
    [ProvideProperty("MaxLength", typeof(Control))]
    public partial class EntityFieldLengthProvider : Component, IExtenderProvider, ISupportInitialize
    {
        Dictionary<Control, EntityFieldLengthSource> lengthMap = new Dictionary<Control, EntityFieldLengthSource>();

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityFieldLengthProvider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityFieldLengthProvider(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// ISupportInitialize - begin
        /// </summary>
        public void BeginInit()
        {

        }

        /// <summary>
        /// ISupportInitialize - end initialization and set max lengths
        /// </summary>
        public void EndInit()
        {
            foreach (var pair in lengthMap)
            {
                if (!DesignMode && !DesignModeDetector.IsDesignerHosted())
                {
                    SetControlMaxLength(pair.Key, GetMaxLength(pair.Value));
                }
            }
        }

        /// <summary>
        /// Indicates if this extender provider can extend the given object
        /// </summary>
        public bool CanExtend(object extendee)
        {
            if (extendee is TextBox)
            {
                return true;
            }

            if (extendee is ComboBox)
            {
                return true;
            }

            if (extendee is KryptonTextBox)
            {
                return true;
            }

            if (extendee is TemplateTokenTextBox)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [DefaultValue(EntityFieldLengthSource.None)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EntityFieldLengthSource GetMaxLengthSource(Control control)
        {
            EntityFieldLengthSource source;
            if (!lengthMap.TryGetValue(control, out source))
            {
                return EntityFieldLengthSource.None;
            }

            return source;
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [Category("ShipWorks")]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetMaxLength(Control control)
        {
            int maxLength = GetMaxLength(GetMaxLengthSource(control));

            return maxLength == 0 ? "" : maxLength.ToString();
        }

        /// <summary>
        /// Necessary piece of the extender provider plumbing
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetMaxLengthSource(Control control, EntityFieldLengthSource source)
        {
            if (source == EntityFieldLengthSource.None)
            {
                lengthMap.Remove(control);
            }
            else
            {
                lengthMap[control] = source;
            }
        }

        /// <summary>
        /// Set the max length of the given control
        /// </summary>
        private void SetControlMaxLength(Control control, int length)
        {
            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.MaxLength = length;
            }

            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.MaxLength = length;
            }

            KryptonTextBox kryptonBox = control as KryptonTextBox;
            if (kryptonBox != null)
            {
                kryptonBox.MaxLength = length;
            }

            TemplateTokenTextBox tokenBox = control as TemplateTokenTextBox;
            if (tokenBox != null)
            {
                tokenBox.MaxLength = 0;
            }
        }

        /// <summary>
        /// Get the max length associated with the given source
        /// </summary>
        public static int GetMaxLength(EntityFieldLengthSource source)
        {
            // One's that don't directly map to fields
            switch (source)
            {
                case EntityFieldLengthSource.None:
                    return 0;

                case EntityFieldLengthSource.PersonNameFull:
                    return GetMaxLength(EntityFieldLengthSource.PersonFirst) +
                           GetMaxLength(EntityFieldLengthSource.PersonLast) +
                           GetMaxLength(EntityFieldLengthSource.PersonMiddle);

                case EntityFieldLengthSource.PersonStreetFull:
                    return GetMaxLength(EntityFieldLengthSource.PersonStreet1) +
                           GetMaxLength(EntityFieldLengthSource.PersonStreet2) +
                           GetMaxLength(EntityFieldLengthSource.PersonStreet3);
            }
            
            // Look up based on LLBLGen metadata
            EntityField2 field = GetSourceField(source);
            int length = field.MaxLength;

            // If it's encrypted reduce by 1/3 to make room for encryption length
            switch (source)
            {
                case EntityFieldLengthSource.UserPassword:
                case EntityFieldLengthSource.EmailPassword:
                case EntityFieldLengthSource.AmazonSellerPassword:
                case EntityFieldLengthSource.GenericPassword:
                case EntityFieldLengthSource.UspsPassword:
                case EntityFieldLengthSource.EndiciaWebPassword:
                case EntityFieldLengthSource.EndiciaApiPassword:
                case EntityFieldLengthSource.OrderPaymentDetailValue:
                    length = (int) (length * .6667);
                    break;
            }

            return length;
        }

        /// <summary>
        /// Get the entity field associated with the given source
        /// </summary>
        private static EntityField2 GetSourceField(EntityFieldLengthSource source)
        {
            switch (source)
            {
                case EntityFieldLengthSource.ActionName: return ActionFields.Name;

                case EntityFieldLengthSource.EmailAccountName: return EmailAccountFields.AccountName;
                case EntityFieldLengthSource.EmailAddress: return EmailAccountFields.EmailAddress;
                case EntityFieldLengthSource.EmailServer: return EmailAccountFields.IncomingServer;
                case EntityFieldLengthSource.EmailUsername: return EmailAccountFields.IncomingUsername;
                case EntityFieldLengthSource.EmailPassword: return EmailAccountFields.IncomingPassword;

                case EntityFieldLengthSource.EmailSubject: return EmailOutboundFields.Subject;

                case EntityFieldLengthSource.FilterName: return FilterFields.Name;

                case EntityFieldLengthSource.TemplateName: return TemplateFields.Name;
                case EntityFieldLengthSource.TemplateEncoding: return TemplateFields.OutputEncoding;
                case EntityFieldLengthSource.TemplateSaveFile: return TemplateFields.SaveFileName;
                case EntityFieldLengthSource.TemplateSaveFolder: return TemplateFields.SaveFileFolder;

                case EntityFieldLengthSource.LabelSheetName: return LabelSheetFields.Name;

                case EntityFieldLengthSource.OrderRequestedShipping: return OrderFields.RequestedShipping;

                case EntityFieldLengthSource.OrderItemName: return OrderItemFields.Name;
                case EntityFieldLengthSource.OrderItemCode: return OrderItemFields.Code;
                case EntityFieldLengthSource.OrderItemSku: return OrderItemFields.SKU;
                case EntityFieldLengthSource.OrderItemLocation: return OrderItemFields.Location;
                case EntityFieldLengthSource.OrderItemIsbn: return OrderItemFields.ISBN;
                case EntityFieldLengthSource.OrderItemUpc: return OrderItemFields.UPC;
                case EntityFieldLengthSource.OrderAttributeName: return OrderItemAttributeFields.Name;
                case EntityFieldLengthSource.OrderChargeType: return OrderChargeFields.Type;
                case EntityFieldLengthSource.OrderChargeDescription: return OrderChargeFields.Description;
                case EntityFieldLengthSource.OrderPaymentDetailLabel: return OrderPaymentDetailFields.Label;
                case EntityFieldLengthSource.OrderPaymentDetailValue: return OrderPaymentDetailFields.Value;

                case EntityFieldLengthSource.PersonFirst: return OrderFields.ShipFirstName;
                case EntityFieldLengthSource.PersonMiddle: return OrderFields.ShipMiddleName;
                case EntityFieldLengthSource.PersonLast: return OrderFields.ShipLastName;
                case EntityFieldLengthSource.PersonCompany: return OrderFields.ShipCompany;
                case EntityFieldLengthSource.PersonStreet1: return OrderFields.ShipStreet1;
                case EntityFieldLengthSource.PersonStreet2: return OrderFields.ShipStreet2;
                case EntityFieldLengthSource.PersonStreet3: return OrderFields.ShipStreet3;
                case EntityFieldLengthSource.PersonCity: return OrderFields.ShipCity;
                case EntityFieldLengthSource.PersonState: return OrderFields.ShipStateProvCode;
                case EntityFieldLengthSource.PersonPostal: return OrderFields.ShipPostalCode;
                case EntityFieldLengthSource.PersonCountry: return OrderFields.ShipCountryCode;
                case EntityFieldLengthSource.PersonPhone: return OrderFields.ShipPhone;
                case EntityFieldLengthSource.PersonFax: return OrderFields.ShipFax;
                case EntityFieldLengthSource.PersonEmail: return OrderFields.ShipEmail;
                case EntityFieldLengthSource.PersonWebsite: return OrderFields.ShipWebsite;

                case EntityFieldLengthSource.StoreName: return StoreFields.StoreName;
                case EntityFieldLengthSource.StoreCompany: return StoreFields.Company;

                case EntityFieldLengthSource.UserName: return UserFields.Username;
                case EntityFieldLengthSource.UserPassword: return UserFields.Password;
                case EntityFieldLengthSource.UserEmail: return UserFields.Email;

                case EntityFieldLengthSource.AmazonAccessKey: return AmazonStoreFields.AccessKeyID;
                case EntityFieldLengthSource.AmazonSellerUsername: return AmazonStoreFields.SellerCentralUsername;
                case EntityFieldLengthSource.AmazonSellerPassword: return AmazonStoreFields.SellerCentralPassword;
                case EntityFieldLengthSource.AmazonMerchantName: return AmazonStoreFields.MerchantName;
                case EntityFieldLengthSource.AmazonMerchantToken: return AmazonStoreFields.MerchantToken;

                case EntityFieldLengthSource.ChannelAdvisorAccountKey: return ChannelAdvisorStoreFields.AccountKey;

                case EntityFieldLengthSource.GenericUsername: return GenericModuleStoreFields.ModuleUsername;
                case EntityFieldLengthSource.GenericPassword: return GenericModuleStoreFields.ModulePassword;
                case EntityFieldLengthSource.GenericModuleUrl: return GenericModuleStoreFields.ModuleUrl;
                case EntityFieldLengthSource.GenericStoreCode: return GenericModuleStoreFields.ModuleOnlineStoreCode;

                case EntityFieldLengthSource.InfopiaApiToken: return InfopiaStoreFields.ApiToken;

                case EntityFieldLengthSource.PayPalUsername: return PayPalStoreFields.ApiUserName;
                case EntityFieldLengthSource.PayPalPassword: return PayPalStoreFields.ApiPassword;
                case EntityFieldLengthSource.PalPaySignature: return PayPalStoreFields.ApiSignature;

                case EntityFieldLengthSource.ShopSiteUsername: return ShopSiteStoreFields.Username;
                case EntityFieldLengthSource.ShopSitePassword: return ShopSiteStoreFields.Password;
                case EntityFieldLengthSource.ShopSiteUrl: return ShopSiteStoreFields.CgiUrl;

                case EntityFieldLengthSource.CustomsDescription: return ShipmentCustomsItemFields.Description;
                case EntityFieldLengthSource.CustomsHarmonizedCode: return ShipmentCustomsItemFields.HarmonizedCode;

                case EntityFieldLengthSource.DimensionsProfileName: return DimensionsProfileFields.Name;

                case EntityFieldLengthSource.ShippingProfileName: return ShippingProfileFields.Name;

                case EntityFieldLengthSource.ShipmentOriginDescription: return ShippingOriginFields.Description;

                case EntityFieldLengthSource.ShipmentPrintOutputGroupName: return ShippingPrintOutputFields.Name;

                case EntityFieldLengthSource.ShipmentTracking: return ShipmentFields.TrackingNumber;
                case EntityFieldLengthSource.ShipmentOtherService: return OtherShipmentFields.Service;
                case EntityFieldLengthSource.ShipmentOtherCarrier: return OtherShipmentFields.Carrier;

                case EntityFieldLengthSource.PostalCustomsDescription: return PostalShipmentFields.CustomsContentDescription;

                case EntityFieldLengthSource.UspsUsername: return UspsAccountFields.Username;
                case EntityFieldLengthSource.UspsPassword: return UspsAccountFields.Password;

                case EntityFieldLengthSource.EndiciaAccountDescription: return EndiciaAccountFields.Description;
                case EntityFieldLengthSource.EndiciaCustomsSigner: return ShippingSettingsFields.EndiciaCustomsSigner;
                case EntityFieldLengthSource.EndiciaReference: return EndiciaShipmentFields.ReferenceID;
                case EntityFieldLengthSource.EndiciaRubberStamp: return PostalShipmentFields.Memo1;
                case EntityFieldLengthSource.EndiciaApiPassword: return EndiciaAccountFields.ApiUserPassword;
                case EntityFieldLengthSource.EndiciaWebPassword: return EndiciaAccountFields.WebPassword;

                case EntityFieldLengthSource.UpsAccountNumber: return UpsAccountFields.AccountNumber;
                case EntityFieldLengthSource.UpsAccountDescription: return UpsAccountFields.Description;
                case EntityFieldLengthSource.UpsReference: return UpsShipmentFields.ReferenceNumber;
                case EntityFieldLengthSource.UpsPayorAccount: return UpsShipmentFields.PayorAccount;
                case EntityFieldLengthSource.UpsQvnOtherAddress: return UpsShipmentFields.EmailNotifyOtherAddress;
                case EntityFieldLengthSource.UpsQvnFrom: return UpsShipmentFields.EmailNotifyFrom;
                case EntityFieldLengthSource.UpsQvnMessage: return UpsShipmentFields.EmailNotifyMessage;
                case EntityFieldLengthSource.UpsCustomsDescription: return UpsShipmentFields.CustomsDescription;
                case EntityFieldLengthSource.UpsCommercialInvoiceComments: return UpsShipmentFields.CommercialInvoiceComments;
                case EntityFieldLengthSource.UpsContactName: return UpsPackageFields.VerbalConfirmationName;
                case EntityFieldLengthSource.UpsContactPhoneNumber: return UpsPackageFields.VerbalConfirmationPhone;
                case EntityFieldLengthSource.UpsContactPhoneExtension: return UpsPackageFields.VerbalConfirmationPhoneExtension;

                case EntityFieldLengthSource.FedExAccountNumber: return FedExAccountFields.AccountNumber;
                case EntityFieldLengthSource.FedExAccountDescription: return FedExAccountFields.Description;
                case EntityFieldLengthSource.FedExSignatureRelease: return FedExAccountFields.SignatureRelease;
                case EntityFieldLengthSource.FedExReferenceCustomer: return FedExShipmentFields.ReferenceCustomer;
                case EntityFieldLengthSource.FedExReferenceInvoice: return FedExShipmentFields.ReferenceInvoice;
                case EntityFieldLengthSource.FedExReferencePO: return FedExShipmentFields.ReferencePO;
                case EntityFieldLengthSource.FedExReferenceShipmentIntegrity: return FedExShipmentFields.ReferenceShipmentIntegrity;
                case EntityFieldLengthSource.FedExHomeDeliveryInstructions: return FedExShipmentFields.HomeDeliveryInstructions;
                case EntityFieldLengthSource.FedExHomeDeliveryPhone: return FedExShipmentFields.HomeDeliveryPhone;
                case EntityFieldLengthSource.FedExFreightBookingNumber: return FedExShipmentFields.FreightBookingNumber;
                case EntityFieldLengthSource.FedExEmailNotifyOtherAddress: return FedExShipmentFields.EmailNotifyOtherAddress;
                case EntityFieldLengthSource.FedExEmailNotifyMessage: return FedExShipmentFields.EmailNotifyMessage;
                case EntityFieldLengthSource.FedExCustomsTin: return FedExShipmentFields.CustomsRecipientTIN;
                case EntityFieldLengthSource.FedExCustomsDocumentsDescription: return FedExShipmentFields.CustomsDocumentsDescription;
                case EntityFieldLengthSource.FedExCommercialInvoiceComments: return FedExShipmentFields.CommercialInvoiceComments;
                case EntityFieldLengthSource.FedExCommercialInvoiceReference:return FedExShipmentFields.CommercialInvoiceReference;
                case EntityFieldLengthSource.FedExCustomsAESEEI: return FedExShipmentFields.CustomsAESEEI;
                case EntityFieldLengthSource.FedExRmaNumber: return FedExShipmentFields.RmaNumber;
                case EntityFieldLengthSource.FedExRmaReason: return FedExShipmentFields.RmaReason;
                case EntityFieldLengthSource.FedExShipmentFimsAirWaybill: return FedExShipmentFields.FimsAirWaybill;

                case EntityFieldLengthSource.FedExSmartPostCustomerManifest: return FedExShipmentFields.SmartPostCustomerManifest;
                case EntityFieldLengthSource.FedExPayorTransportName: return FedExShipmentFields.PayorTransportName;
                case EntityFieldLengthSource.FedExPackagePriorityAlertContentDetail: return FedExPackageFields.PriorityAlertDetailContent;

                case EntityFieldLengthSource.OnTracInstructions: return OnTracShipmentFields.Instructions;
                case EntityFieldLengthSource.OnTracReference1: return OnTracShipmentFields.Reference1;
                case EntityFieldLengthSource.OnTracReference2: return OnTracShipmentFields.Reference2;

                case EntityFieldLengthSource.AmazonShipmentCarrierName: return AmazonShipmentFields.CarrierName;
                case EntityFieldLengthSource.AmazonShipmentShippingServiceID: return AmazonShipmentFields.ShippingServiceID;
                case EntityFieldLengthSource.AmazonShipmentShippingServiceName: return AmazonShipmentFields.ShippingServiceName;
                case EntityFieldLengthSource.AmazonShipmentShippingServiceOfferID: return AmazonShipmentFields.ShippingServiceOfferID;
            }

            throw new InvalidOperationException("Unmapped EntityFieldLengthSource: " + source);
        }
    }
}
