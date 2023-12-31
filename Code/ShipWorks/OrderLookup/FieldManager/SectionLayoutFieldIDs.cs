﻿using System.Reflection;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Field IDs used in section layouts (panels)
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum SectionLayoutFieldIDs
    {
        /* Common address fields */
        AccountSelector,
        FullName,
        Company,
        Street,
        City,
        StateProvince,
        PostalCode,
        Country,
        Email,
        Phone,
        Fax,
        Website,

        /* From address specific fields */
        Origin,
        FedExResidentialCommercialAddress,

        /* To address specific fields */
        USPSRequireFullAddressValidation,
        AddressType,

        /* Label Options */
        LabelOptionsShipDate,
        LabelOptionsUspsHideStealth,
        LabelOptionsNoPostage,
        LabelOptionsRequestedLabelFormat,

        /* Shipment details */
        ShipmentDetailsRequestedShipping,
        ShipmentDetailsProvider,
        ShipmentDetailsService,
        ShipmentDetailsConfirmation,
        ShipmentDetailsPackaging,
        ShipmentDetailsMultiPackageShipment,
        ShipmentDetailsNonStandardPackaging,
        ShipmentDetailsWeight,
        ShipmentDetailsDimensions,
        ShipmentDetailsAddToWeight,
        ShipmentDetailsInsurance,

        /* USPS Reference fields */
        USPSReferenceMemo1,
        USPSReferenceMemo2,
        USPSReferenceMemo3,

        /* FedEx Fields */
        FedExSignatureAndReferenceSignatureRequired,
        FedExSignatureAndReferenceReferenceNumber,
        FedExSignatureAndReferenceInvoiceNumber,
        FedExSignatureAndReferencePostOfficeNumber,
        FedExSignatureAndReferenceIntegrity,

        /* UPS Reference Fields */
        UPSReferenceReferenceNumber,
        UPSReferenceReferenceNumber2,

        /* DHL Ecommerce Reference Fields */
        DhlEcommerceReferenceNumber
    }
}
