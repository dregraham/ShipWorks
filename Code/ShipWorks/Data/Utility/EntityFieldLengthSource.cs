﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Defines the field sources for field length information
    /// </summary>
    public enum EntityFieldLengthSource
    {
        None,

        ActionName,

        EmailAccountName,
        EmailAddress,
        EmailUsername,
        EmailPassword,
        EmailServer,
        EmailSubject,

        FilterName,

        TemplateName,
        TemplateEncoding,
        TemplateSaveFile,
        TemplateSaveFolder,

        LabelSheetName,

        OrderRequestedShipping,
        OrderItemName,
        OrderItemCode,
        OrderItemSku,
        OrderItemLocation,
        OrderAttributeName,
        OrderChargeType,
        OrderChargeDescription,
        OrderPaymentDetailLabel,
        OrderPaymentDetailValue,

        PersonNameFull,
        PersonFirst,
        PersonMiddle,
        PersonLast,
        PersonCompany,
        PersonStreet1,
        PersonStreet2,
        PersonStreet3,
        PersonStreetFull,
        PersonCity,
        PersonState,
        PersonPostal,
        PersonCountry,
        PersonPhone,
        PersonFax,
        PersonEmail,
        PersonWebsite,

        StoreName,
        StoreCompany,

        UserName,
        UserPassword,
        UserEmail,

        AmazonAccessKey,
        AmazonSellerUsername,
        AmazonSellerPassword,
        AmazonMerchantName,
        AmazonMerchantToken,

        ChannelAdvisorAccountKey,

        GenericUsername,
        GenericPassword,
        GenericModuleUrl,
        GenericStoreCode,

        InfopiaApiToken,

        PayPalUsername,
        PayPalPassword,
        PalPaySignature,

        ShopSiteUsername,
        ShopSitePassword,
        ShopSiteUrl,

        CustomsDescription,
        CustomsHarmonizedCode,

        DimensionsProfileName,

        ShippingProfileName,

        ShipmentOriginDescription,

        ShipmentPrintOutputGroupName,

        ShipmentTracking,

        ShipmentOtherCarrier,
        ShipmentOtherService,

        PostalCustomsDescription,

        StampsUsername,
        StampsPassword,

        EndiciaAccountDescription,

        EndiciaCustomsSigner,
        EndiciaReference,
        EndiciaRubberStamp,
        EndiciaApiPassword,
        EndiciaWebPassword,

        UpsAccountNumber,
        UpsAccountDescription,
        UpsReference,
        UpsPayorAccount,
        UpsQvnOtherAddress,
        UpsQvnFrom,
        UpsQvnMessage,
        UpsCustomsDescription,
        UpsCommercialInvoiceComments,

        FedExAccountNumber,
        FedExAccountDescription,
        FedExSignatureRelease,
        FedExReferenceCustomer,
        FedExReferenceInvoice,
        FedExReferencePO,
        FedExHomeDeliveryInstructions,
        FedExHomeDeliveryPhone,
        FedExFreightBookingNumber,
        FedExEmailNotifyOtherAddress,
        FedExEmailNotifyMessage,
        FedExCustomsTin,
        FedExCustomsDocumentsDescription,
        FedExCommercialInvoiceComments,
        FedExCommercialInvoiceReference,
        FedExSmartPostCustomerManifest,
        FedExPayorTransportName,
        FedExPackagePriorityAlertContentDetail,
        FedExCustomsAESEEI,
        FedExRmaNumber,
        FedExRmaReason,

        OnTracReference1,
        OnTracReference2,
        OnTracInstructions,
        OnTracAccountDescription
    }
}
