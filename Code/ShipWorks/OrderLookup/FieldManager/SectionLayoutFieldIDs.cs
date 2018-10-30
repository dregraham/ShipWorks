using System.Reflection;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Field IDs used in section layouts (panels)
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum SectionLayoutFieldIDs
    {
        FromUSPSAccountSelector,
        FromOrigin,
        FromFullName,
        FromCompany,
        FromStreet,
        FromCity,
        FromStateProvince,
        FromPostalCode,
        FromCountry,
        FromEmail,
        FromPhone,
        FromFax,
        FromWebsite,
        FromFedExResidentialCommercialAddress,
        FromUspsHideStealth,

        ToFullName,
        ToCompany,
        ToStreet,
        ToCity,
        ToStateProvince,
        ToPostalCode,
        ToCountry,
        ToEmail,
        ToPhone,
        ToUSPSRequireFullAddressValidation,
        ToAddressType,

        LabelOptionsShipDate,
        LabelOptionsUspsHideStealth,
        LabelOptionsRequestedLabelFormat,


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
        ShipmentDetailsOnTracSaturdayDelivery,
        ShipmentDetailsOnTracSignatureRequired,

        USPSReferenceMemo1,
        USPSReferenceMemo2,
        USPSReferenceMemo3,

        FedExSignatureAndReferenceSignatureRequired,
        FedExSignatureAndReferenceReferenceNumber,
        FedExSignatureAndReferenceInvoiceNumber,
        FedExSignatureAndReferencePostOfficeNumber,
        FedExSignatureAndReferenceIntegrity,

        UPSReferenceReferenceNumber,
        UPSReferenceReferenceNumber2
    }
}
