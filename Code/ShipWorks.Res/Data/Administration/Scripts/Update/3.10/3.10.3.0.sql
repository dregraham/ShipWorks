SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'EndiciaAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'RefundFormID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'TransactionID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCity'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCompany'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerEmail'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerFirstName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerLastName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhone'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhoneExtension'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPostalCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet1'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet2'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet3'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAccountNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAddFreight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodChargeBasis'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCity'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCompany'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodFirstName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodLastName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodOriginID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPhone'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPostalCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet1'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet2'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet3'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTIN'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingFormID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoice'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceReference'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAdmissibilityPackaging'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAESEEI'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsDescription'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsExportFilingOption'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaDeterminationCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaEnabled'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaNetCostMethod'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaPreferenceType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaProducerId'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsDesription'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationValue'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientTIN'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyBroker'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExAccountID'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExHoldAtLocationEnabled'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightBookingNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsideDelivery'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsidePickup'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightLoadAndCount'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCity'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCompanyName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldContactId'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldEmailAddress'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldFaxNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationId'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPagerNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPersonName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneExtension'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPostalCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldResidential'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStateOrProvinceCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet1'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet2'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet3'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldTitle'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldUrbanizationCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryDate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryInstructions'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryPhone'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterAccount'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCity'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCompany'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterFirstName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterLastName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterOfRecord'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPhone'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPostalCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStateProvCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet1'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet2'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet3'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterTIN'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailEntryNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailForeignTradeZoneCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitExpirationDate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'MasterFormID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PackagingType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesAccount'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportAccount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportName'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ReturnType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaNumber'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaReason'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Service'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Signature'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostConfirmation'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostCustomerManifest'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostEndorsement'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostHubID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostIndicia'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'TrafficInArmsLicenseNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DeclaredValue'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Description'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'EquaShipAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'PackageType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ReferenceNumber'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Service'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ShippingNotes'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'iParcelAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'Service'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'TrackBySMS'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialPaperlessInvoice'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDescription'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'DeliveryConfirmation'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyFrom'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySubject'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'NegotiatedRate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorCountryCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorPostalCode'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PublishedCharges'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'Service'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'UpsAccountID'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'WorldShipStatus'
GO
PRINT N'Dropping foreign keys from [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] DROP CONSTRAINT [FK_BestRateShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] DROP CONSTRAINT [FK_EndiciaShipment_PostalShipment]
ALTER TABLE [dbo].[EndiciaShipment] DROP CONSTRAINT [FK_EndiciaShipment_ScanFormBatch]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] DROP CONSTRAINT [FK_FedExPackage_FedExShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [FK_FedExShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] DROP CONSTRAINT [FK_EquashipShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelPackage]'
GO
ALTER TABLE [dbo].[iParcelPackage] DROP CONSTRAINT [FK_iParcelPackage_iParcelShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] DROP CONSTRAINT [FK_iParcelShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] DROP CONSTRAINT [FK_OnTracShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_PostalShipment]
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [FK_StampsShipment_ScanFormBatch]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] DROP CONSTRAINT [FK_UpsPackage_UpsShipment]
GO
PRINT N'Dropping foreign keys from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [FK_UpsShipment_Shipment]
GO
PRINT N'Dropping foreign keys from [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] DROP CONSTRAINT [FK_WorldShipShipment_UpsShipment]
GO
PRINT N'Dropping constraints from [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] DROP CONSTRAINT [PK_BestRateShipment]
GO
PRINT N'Dropping constraints from [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] DROP CONSTRAINT [PK_EndiciaShipment]
GO
PRINT N'Dropping constraints from [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] DROP CONSTRAINT [PK_FedExShipment]
GO
PRINT N'Dropping constraints from [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] DROP CONSTRAINT [PK_EquashipShipment]
GO
PRINT N'Dropping constraints from [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] DROP CONSTRAINT [PK_iParcelShipment]
GO
PRINT N'Dropping constraints from [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] DROP CONSTRAINT [PK_OnTracShipment]
GO
PRINT N'Dropping constraints from [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] DROP CONSTRAINT [PK_StampsShipment]
GO
PRINT N'Dropping constraints from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [PK_UpsShipment]
GO
PRINT N'Rebuilding [dbo].[BestRateShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_BestRateShipment]
(
[ShipmentID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[ServiceLevel] [int] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_BestRateShipment]([ShipmentID], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [ServiceLevel], [InsuranceValue], [RequestedLabelFormat]) 
	SELECT brs.[ShipmentID], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [ServiceLevel], [InsuranceValue], s.RequestedLabelFormat
	FROM [dbo].[BestRateShipment] brs, Shipment s
	where s.ShipmentID = brs.ShipmentID
GO
DROP TABLE [dbo].[BestRateShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BestRateShipment]', N'BestRateShipment'
GO
PRINT N'Creating primary key [PK_BestRateShipment] on [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [PK_BestRateShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[EndiciaShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EndiciaShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EndiciaAccountID] [bigint] NOT NULL,
[OriginalEndiciaAccountID] [bigint] NULL,
[StealthPostage] [bit] NOT NULL,
[NoPostage] [bit] NOT NULL,
[ReferenceID] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RubberStamp3] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TransactionID] [int] NULL,
[RefundFormID] [int] NULL,
[ScanFormBatchID] [bigint] NULL,
[ScanBasedReturn] [bit] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_EndiciaShipment]([ShipmentID], [EndiciaAccountID], [OriginalEndiciaAccountID], [StealthPostage], [NoPostage], [ReferenceID], [RubberStamp1], [RubberStamp2], [RubberStamp3], [TransactionID], [RefundFormID], [ScanFormBatchID], 
[ScanBasedReturn], [RequestedLabelFormat]) 
		SELECT s.[ShipmentID], [EndiciaAccountID], [OriginalEndiciaAccountID], [StealthPostage], [NoPostage], [ReferenceID], [RubberStamp1], [RubberStamp2], [RubberStamp3], [TransactionID], [RefundFormID], [ScanFormBatchID], 
		[ScanBasedReturn], s.RequestedLabelFormat
		FROM [dbo].[EndiciaShipment] childShipment, Shipment s
		WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[EndiciaShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EndiciaShipment]', N'EndiciaShipment'
GO
PRINT N'Creating primary key [PK_EndiciaShipment] on [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [PK_EndiciaShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[EquaShipShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EquaShipShipment]
(
[ShipmentID] [bigint] NOT NULL,
[EquaShipAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[PackageType] [int] NOT NULL,
[ReferenceNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShippingNotes] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[EmailNotification] [bit] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[Confirmation] [int] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_EquaShipShipment]([ShipmentID], [EquaShipAccountID], [Service], [PackageType], [ReferenceNumber], [Description], [ShippingNotes], [DimsProfileID], [DimsLength], [DimsHeight], [DimsWidth], [DimsWeight], [DimsAddWeight], [InsuranceValue], [DeclaredValue], [EmailNotification], [SaturdayDelivery], 
[Confirmation], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [EquaShipAccountID], [Service], [PackageType], [ReferenceNumber], [Description], [ShippingNotes], [DimsProfileID], [DimsLength], [DimsHeight], [DimsWidth], [DimsWeight], [DimsAddWeight], [InsuranceValue], [DeclaredValue], [EmailNotification], [SaturdayDelivery], 
	[Confirmation], s.RequestedLabelFormat
	FROM [dbo].[EquaShipShipment] childShipment, Shipment s
		WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[EquaShipShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EquaShipShipment]', N'EquaShipShipment'
GO
PRINT N'Creating primary key [PK_EquashipShipment] on [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] ADD CONSTRAINT [PK_EquashipShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[FedExShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FedExShipment]
(
[ShipmentID] [bigint] NOT NULL,
[FedExAccountID] [bigint] NOT NULL,
[MasterFormID] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Service] [int] NOT NULL,
[Signature] [int] NOT NULL,
[PackagingType] [int] NOT NULL,
[NonStandardContainer] [bit] NOT NULL,
[ReferenceCustomer] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceInvoice] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferencePO] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceShipmentIntegrity] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorTransportType] [int] NOT NULL,
[PayorTransportName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorTransportAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesType] [int] NOT NULL,
[PayorDutiesAccount] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesName] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorDutiesCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[HomeDeliveryType] [int] NOT NULL,
[HomeDeliveryInstructions] [varchar] (74) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HomeDeliveryDate] [datetime] NOT NULL,
[HomeDeliveryPhone] [varchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightInsidePickup] [bit] NOT NULL,
[FreightInsideDelivery] [bit] NOT NULL,
[FreightBookingNumber] [varchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FreightLoadAndCount] [int] NOT NULL,
[EmailNotifyBroker] [int] NOT NULL,
[EmailNotifySender] [int] NOT NULL,
[EmailNotifyRecipient] [int] NOT NULL,
[EmailNotifyOther] [int] NOT NULL,
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifyMessage] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodEnabled] [bit] NOT NULL,
[CodAmount] [money] NOT NULL,
[CodPaymentType] [int] NOT NULL,
[CodAddFreight] [bit] NOT NULL,
[CodOriginID] [bigint] NOT NULL,
[CodFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTrackingFormID] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodTIN] [nvarchar] (24) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CodChargeBasis] [int] NOT NULL,
[CodAccountNumber] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerEnabled] [bit] NOT NULL,
[BrokerAccount] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerPhoneExtension] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrokerEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsAdmissibilityPackaging] [int] NOT NULL,
[CustomsRecipientTIN] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDocumentsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsExportFilingOption] [int] NOT NULL,
[CustomsAESEEI] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsRecipientIdentificationType] [int] NOT NULL,
[CustomsRecipientIdentificationValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsOptionsType] [int] NOT NULL,
[CustomsOptionsDesription] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoice] [bit] NOT NULL,
[CommercialInvoiceFileElectronically] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
[CommercialInvoiceReference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterOfRecord] [bit] NOT NULL,
[ImporterAccount] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterTIN] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterFirstName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterLastName] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCompany] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet1] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet2] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStreet3] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCity] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterStateProvCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterPostalCode] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImporterPhone] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostIndicia] [int] NOT NULL,
[SmartPostEndorsement] [int] NOT NULL,
[SmartPostConfirmation] [bit] NOT NULL,
[SmartPostCustomerManifest] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostHubID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SmartPostUspsApplicationId] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DropoffType] [int] NOT NULL,
[OriginResidentialDetermination] [int] NOT NULL,
[FedExHoldAtLocationEnabled] [bit] NOT NULL,
[HoldLocationId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldLocationType] [int] NULL,
[HoldContactId] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPersonName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldTitle] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCompanyName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPhoneNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPhoneExtension] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPagerNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldFaxNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldEmailAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet1] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet2] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStreet3] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCity] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldStateOrProvinceCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldUrbanizationCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldCountryCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HoldResidential] [bit] NULL,
[CustomsNaftaEnabled] [bit] NOT NULL,
[CustomsNaftaPreferenceType] [int] NOT NULL,
[CustomsNaftaDeterminationCode] [int] NOT NULL,
[CustomsNaftaProducerId] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsNaftaNetCostMethod] [int] NOT NULL,
[ReturnType] [int] NOT NULL,
[RmaNumber] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RmaReason] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnSaturdayPickup] [bit] NOT NULL,
[TrafficInArmsLicenseNumber] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailType] [int] NOT NULL,
[IntlExportDetailForeignTradeZoneCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailEntryNumber] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailLicenseOrPermitNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IntlExportDetailLicenseOrPermitExpirationDate] [datetime] NULL,
[WeightUnitType] [int] NOT NULL,
[LinearUnitType] [int] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_FedExShipment]([ShipmentID], [FedExAccountID], [MasterFormID], [Service], [Signature], [PackagingType], [NonStandardContainer], [ReferenceCustomer], [ReferenceInvoice], [ReferencePO], [ReferenceShipmentIntegrity], [PayorTransportType], [PayorTransportName], [PayorTransportAccount], [PayorDutiesType], [PayorDutiesAccount], [PayorDutiesName], [PayorDutiesCountryCode], [SaturdayDelivery], [HomeDeliveryType], [HomeDeliveryInstructions], [HomeDeliveryDate], [HomeDeliveryPhone], [FreightInsidePickup], [FreightInsideDelivery], [FreightBookingNumber], [FreightLoadAndCount], [EmailNotifyBroker], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyMessage], [CodEnabled], [CodAmount], [CodPaymentType], [CodAddFreight], [CodOriginID], [CodFirstName], [CodLastName], [CodCompany], [CodStreet1], [CodStreet2], [CodStreet3], [CodCity], [CodStateProvCode], [CodPostalCode], [CodCountryCode], [CodPhone], [CodTrackingNumber], [CodTrackingFormID], [CodTIN], [CodChargeBasis], [CodAccountNumber], [BrokerEnabled], [BrokerAccount], [BrokerFirstName], [BrokerLastName], [BrokerCompany], [BrokerStreet1], [BrokerStreet2], [BrokerStreet3], [BrokerCity], [BrokerStateProvCode], [BrokerPostalCode], [BrokerCountryCode], [BrokerPhone], [BrokerPhoneExtension], [BrokerEmail], [CustomsAdmissibilityPackaging], [CustomsRecipientTIN], [CustomsDocumentsOnly], [CustomsDocumentsDescription], [CustomsExportFilingOption], [CustomsAESEEI], [CustomsRecipientIdentificationType], [CustomsRecipientIdentificationValue], [CustomsOptionsType], [CustomsOptionsDesription], [CommercialInvoice], [CommercialInvoiceFileElectronically], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [CommercialInvoiceReference], [ImporterOfRecord], [ImporterAccount], [ImporterTIN], [ImporterFirstName], [ImporterLastName], [ImporterCompany], [ImporterStreet1], [ImporterStreet2], [ImporterStreet3], [ImporterCity], [ImporterStateProvCode], [ImporterPostalCode], [ImporterCountryCode], [ImporterPhone], [SmartPostIndicia], [SmartPostEndorsement], [SmartPostConfirmation], [SmartPostCustomerManifest], [SmartPostHubID], [SmartPostUspsApplicationId], [DropoffType], [OriginResidentialDetermination], [FedExHoldAtLocationEnabled], [HoldLocationId], [HoldLocationType], [HoldContactId], [HoldPersonName], [HoldTitle], [HoldCompanyName], [HoldPhoneNumber], [HoldPhoneExtension], [HoldPagerNumber], [HoldFaxNumber], [HoldEmailAddress], [HoldStreet1], [HoldStreet2], [HoldStreet3], [HoldCity], [HoldStateOrProvinceCode], [HoldPostalCode], [HoldUrbanizationCode], [HoldCountryCode], [HoldResidential], [CustomsNaftaEnabled], [CustomsNaftaPreferenceType], [CustomsNaftaDeterminationCode], [CustomsNaftaProducerId], [CustomsNaftaNetCostMethod], [ReturnType], [RmaNumber], [RmaReason], [ReturnSaturdayPickup], [TrafficInArmsLicenseNumber], [IntlExportDetailType], [IntlExportDetailForeignTradeZoneCode], [IntlExportDetailEntryNumber], [IntlExportDetailLicenseOrPermitNumber], [IntlExportDetailLicenseOrPermitExpirationDate], [WeightUnitType], 
[LinearUnitType], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [FedExAccountID], [MasterFormID], [Service], [Signature], [PackagingType], [NonStandardContainer], [ReferenceCustomer], [ReferenceInvoice], [ReferencePO], [ReferenceShipmentIntegrity], [PayorTransportType], [PayorTransportName], [PayorTransportAccount], [PayorDutiesType], [PayorDutiesAccount], [PayorDutiesName], [PayorDutiesCountryCode], [SaturdayDelivery], [HomeDeliveryType], [HomeDeliveryInstructions], [HomeDeliveryDate], [HomeDeliveryPhone], [FreightInsidePickup], [FreightInsideDelivery], [FreightBookingNumber], [FreightLoadAndCount], [EmailNotifyBroker], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyMessage], [CodEnabled], [CodAmount], [CodPaymentType], [CodAddFreight], [CodOriginID], [CodFirstName], [CodLastName], [CodCompany], [CodStreet1], [CodStreet2], [CodStreet3], [CodCity], [CodStateProvCode], [CodPostalCode], [CodCountryCode], [CodPhone], [CodTrackingNumber], [CodTrackingFormID], [CodTIN], [CodChargeBasis], [CodAccountNumber], [BrokerEnabled], [BrokerAccount], [BrokerFirstName], [BrokerLastName], [BrokerCompany], [BrokerStreet1], [BrokerStreet2], [BrokerStreet3], [BrokerCity], [BrokerStateProvCode], [BrokerPostalCode], [BrokerCountryCode], [BrokerPhone], [BrokerPhoneExtension], [BrokerEmail], [CustomsAdmissibilityPackaging], [CustomsRecipientTIN], [CustomsDocumentsOnly], [CustomsDocumentsDescription], [CustomsExportFilingOption], [CustomsAESEEI], [CustomsRecipientIdentificationType], [CustomsRecipientIdentificationValue], [CustomsOptionsType], [CustomsOptionsDesription], [CommercialInvoice], [CommercialInvoiceFileElectronically], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [CommercialInvoiceReference], [ImporterOfRecord], [ImporterAccount], [ImporterTIN], [ImporterFirstName], [ImporterLastName], [ImporterCompany], [ImporterStreet1], [ImporterStreet2], [ImporterStreet3], [ImporterCity], [ImporterStateProvCode], [ImporterPostalCode], [ImporterCountryCode], [ImporterPhone], [SmartPostIndicia], [SmartPostEndorsement], [SmartPostConfirmation], [SmartPostCustomerManifest], [SmartPostHubID], [SmartPostUspsApplicationId], [DropoffType], [OriginResidentialDetermination], [FedExHoldAtLocationEnabled], [HoldLocationId], [HoldLocationType], [HoldContactId], [HoldPersonName], [HoldTitle], [HoldCompanyName], [HoldPhoneNumber], [HoldPhoneExtension], [HoldPagerNumber], [HoldFaxNumber], [HoldEmailAddress], [HoldStreet1], [HoldStreet2], [HoldStreet3], [HoldCity], [HoldStateOrProvinceCode], [HoldPostalCode], [HoldUrbanizationCode], [HoldCountryCode], [HoldResidential], [CustomsNaftaEnabled], [CustomsNaftaPreferenceType], [CustomsNaftaDeterminationCode], [CustomsNaftaProducerId], [CustomsNaftaNetCostMethod], [ReturnType], [RmaNumber], [RmaReason], [ReturnSaturdayPickup], [TrafficInArmsLicenseNumber], [IntlExportDetailType], [IntlExportDetailForeignTradeZoneCode], [IntlExportDetailEntryNumber], [IntlExportDetailLicenseOrPermitNumber], [IntlExportDetailLicenseOrPermitExpirationDate], [WeightUnitType], 
	[LinearUnitType], s.RequestedLabelFormat
	FROM [dbo].[FedExShipment] childShipment, Shipment s
		WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[FedExShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FedExShipment]', N'FedExShipment'
GO
PRINT N'Creating primary key [PK_FedExShipment] on [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [PK_FedExShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[iParcelShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_iParcelShipment]
(
[ShipmentID] [bigint] NOT NULL,
[iParcelAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[Reference] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TrackByEmail] [bit] NOT NULL,
[TrackBySMS] [bit] NOT NULL,
[IsDeliveryDutyPaid] [bit] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_iParcelShipment]([ShipmentID], [iParcelAccountID], [Service], [Reference], [TrackByEmail], [TrackBySMS], 
[IsDeliveryDutyPaid], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [iParcelAccountID], [Service], [Reference], [TrackByEmail], [TrackBySMS], 
	[IsDeliveryDutyPaid], s.RequestedLabelFormat
	FROM [dbo].[iParcelShipment]childShipment, Shipment s
		WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[iParcelShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_iParcelShipment]', N'iParcelShipment'
GO
PRINT N'Creating primary key [PK_iParcelShipment] on [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [PK_iParcelShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[OnTracShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_OnTracShipment]
(
[ShipmentID] [bigint] NOT NULL,
[OnTracAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[IsCod] [bit] NOT NULL,
[CodType] [int] NOT NULL,
[CodAmount] [money] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[SignatureRequired] [bit] NOT NULL,
[PackagingType] [int] NOT NULL,
[Instructions] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Reference2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_OnTracShipment]([ShipmentID], [OnTracAccountID], [Service], [IsCod], [CodType], [CodAmount], [SaturdayDelivery], [SignatureRequired], [PackagingType], [Instructions], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Reference1], [Reference2], [InsuranceValue], [InsurancePennyOne], 
[DeclaredValue], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [OnTracAccountID], [Service], [IsCod], [CodType], [CodAmount], [SaturdayDelivery], [SignatureRequired], [PackagingType], [Instructions], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Reference1], [Reference2], [InsuranceValue], [InsurancePennyOne], 
	[DeclaredValue], s.RequestedLabelFormat
	FROM [dbo].[OnTracShipment] childShipment, Shipment s
	WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[OnTracShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_OnTracShipment]', N'OnTracShipment'
GO
PRINT N'Creating primary key [PK_OnTracShipment] on [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [PK_OnTracShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[StampsShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_StampsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[StampsAccountID] [bigint] NOT NULL,
[HidePostage] [bit] NOT NULL,
[RequireFullAddressValidation] [bit] NOT NULL,
[IntegratorTransactionID] [uniqueidentifier] NOT NULL,
[StampsTransactionID] [uniqueidentifier] NOT NULL,
[Memo] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalStampsAccountID] [bigint] NULL,
[ScanFormBatchID] [bigint] NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_StampsShipment]([ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], [Memo], [OriginalStampsAccountID], 
	[ScanFormBatchID], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [StampsAccountID], [HidePostage], [RequireFullAddressValidation], [IntegratorTransactionID], [StampsTransactionID], [Memo], [OriginalStampsAccountID], 
	[ScanFormBatchID], s.RequestedLabelFormat 
	FROM [dbo].[StampsShipment] childShipment, Shipment s
	WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[StampsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_StampsShipment]', N'StampsShipment'
GO
PRINT N'Creating primary key [PK_StampsShipment] on [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [PK_StampsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Rebuilding [dbo].[UpsShipment]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsShipment]
(
[ShipmentID] [bigint] NOT NULL,
[UpsAccountID] [bigint] NOT NULL,
[Service] [int] NOT NULL,
[SaturdayDelivery] [bit] NOT NULL,
[CodEnabled] [bit] NOT NULL,
[CodAmount] [money] NOT NULL,
[CodPaymentType] [int] NOT NULL,
[DeliveryConfirmation] [int] NOT NULL,
[ReferenceNumber] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReferenceNumber2] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorType] [int] NOT NULL,
[PayorAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorPostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayorCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifySender] [int] NOT NULL,
[EmailNotifyRecipient] [int] NOT NULL,
[EmailNotifyOther] [int] NOT NULL,
[EmailNotifyOtherAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifyFrom] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[EmailNotifySubject] [int] NOT NULL,
[EmailNotifyMessage] [nvarchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CustomsDocumentsOnly] [bit] NOT NULL,
[CustomsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialPaperlessInvoice] [bit] NOT NULL,
[CommercialInvoiceTermsOfSale] [int] NOT NULL,
[CommercialInvoicePurpose] [int] NOT NULL,
[CommercialInvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommercialInvoiceFreight] [money] NOT NULL,
[CommercialInvoiceInsurance] [money] NOT NULL,
[CommercialInvoiceOther] [money] NOT NULL,
[WorldShipStatus] [int] NOT NULL,
[PublishedCharges] [money] NOT NULL,
[NegotiatedRate] [bit] NOT NULL,
[ReturnService] [int] NOT NULL,
[ReturnUndeliverableEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ReturnContents] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Endorsement] [int] NOT NULL,
[Subclassification] [int] NOT NULL,
[PaperlessAdditionalDocumentation] [bit] NOT NULL,
[ShipperRelease] [bit] NOT NULL,
[CarbonNeutral] [bit] NOT NULL,
[CostCenter] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IrregularIndicator] [int] NOT NULL,
[Cn22Number] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargeType] [int] NOT NULL,
[ShipmentChargeAccount] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargePostalCode] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShipmentChargeCountryCode] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsPackageID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequestedLabelFormat] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsShipment]([ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], [ReferenceNumber2], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialPaperlessInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], [UspsTrackingNumber], [Endorsement], [Subclassification], [PaperlessAdditionalDocumentation], [ShipperRelease], [CarbonNeutral], [CostCenter], [IrregularIndicator], [Cn22Number], [ShipmentChargeType], [ShipmentChargeAccount], [ShipmentChargePostalCode], [ShipmentChargeCountryCode], 
	[UspsPackageID], [RequestedLabelFormat]) 
	SELECT s.[ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], [ReferenceNumber2], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialPaperlessInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], [UspsTrackingNumber], [Endorsement], [Subclassification], [PaperlessAdditionalDocumentation], [ShipperRelease], [CarbonNeutral], [CostCenter], [IrregularIndicator], [Cn22Number], [ShipmentChargeType], [ShipmentChargeAccount], [ShipmentChargePostalCode], [ShipmentChargeCountryCode], 
	[UspsPackageID], s.RequestedLabelFormat 
	FROM [dbo].[UpsShipment] childShipment, Shipment s
	WHERE s.ShipmentID = childShipment.ShipmentID
GO
DROP TABLE [dbo].[UpsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsShipment]', N'UpsShipment'
GO
PRINT N'Creating primary key [PK_UpsShipment] on [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Adding foreign keys to [dbo].[BestRateShipment]'
GO
ALTER TABLE [dbo].[BestRateShipment] ADD CONSTRAINT [FK_BestRateShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EndiciaShipment]'
GO
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [FK_EndiciaShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
ALTER TABLE [dbo].[EndiciaShipment] ADD CONSTRAINT [FK_EndiciaShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[FedExPackage]'
GO
ALTER TABLE [dbo].[FedExPackage] ADD CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExShipment]'
GO
ALTER TABLE [dbo].[FedExShipment] ADD CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EquaShipShipment]'
GO
ALTER TABLE [dbo].[EquaShipShipment] ADD CONSTRAINT [FK_EquashipShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelPackage]'
GO
ALTER TABLE [dbo].[iParcelPackage] ADD CONSTRAINT [FK_iParcelPackage_iParcelShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[iParcelShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelShipment]'
GO
ALTER TABLE [dbo].[iParcelShipment] ADD CONSTRAINT [FK_iParcelShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OnTracShipment]'
GO
ALTER TABLE [dbo].[OnTracShipment] ADD CONSTRAINT [FK_OnTracShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[StampsShipment]'
GO
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [FK_StampsShipment_PostalShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[PostalShipment] ([ShipmentID]) ON DELETE CASCADE
ALTER TABLE [dbo].[StampsShipment] ADD CONSTRAINT [FK_StampsShipment_ScanFormBatch] FOREIGN KEY ([ScanFormBatchID]) REFERENCES [dbo].[ScanFormBatch] ([ScanFormBatchID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ADD CONSTRAINT [FK_WorldShipShipment_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'EndiciaAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'RefundFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EndiciaShipment', 'COLUMN', N'TransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerEmail'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPhoneExtension'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'BrokerStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAccountNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAddFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodChargeBasis'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodOriginID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CodTrackingNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceReference'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAdmissibilityPackaging'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsAESEEI'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsExportFilingOption'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaDeterminationCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'NAFTA Selected', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaNetCostMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaPreferenceType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsNaftaProducerId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsDesription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsOptionsType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientIdentificationValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'CustomsRecipientTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'123', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Dropoff Type', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'DropoffType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyBroker'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExAccountID'
GO
EXEC sp_addextendedproperty N'AuditName', N'Hold At Location Selected', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FedExHoldAtLocationEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightBookingNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsideDelivery'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightInsidePickup'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'FreightLoadAndCount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCompanyName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldContactId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldEmailAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldFaxNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationId'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldLocationType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPagerNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPersonName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneExtension'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPhoneNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldResidential'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStateOrProvinceCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldTitle'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HoldUrbanizationCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryInstructions'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'113', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Home Delivery', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'HomeDeliveryType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCity'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCompany'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterFirstName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterLastName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterOfRecord'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPhone'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStateProvCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet1'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet2'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterStreet3'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ImporterTIN'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailEntryNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailForeignTradeZoneCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitExpirationDate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailLicenseOrPermitNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'IntlExportDetailType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'126', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Dimension Units', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'LinearUnitType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'MasterFormID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'111', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditName', N'Origin Residential \ Commercial', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'OriginResidentialDetermination'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'109', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PackagingType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesAccount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'6', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Country', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesCountryCode'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill duties/fees To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorDutiesType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Account', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportAccount'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transportation Name', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportName'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'110', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Bill Transporation To', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'PayorTransportType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'124', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'ReturnType'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Number', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaNumber'
GO
EXEC sp_addextendedproperty N'AuditName', N'RMA Reason', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'RmaReason'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'108', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'114', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'Signature'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostConfirmation'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostCustomerManifest'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostEndorsement'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostHubID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'SmartPostIndicia'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'TrafficInArmsLicenseNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'125', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_addextendedproperty N'AuditName', N'Weight Units', 'SCHEMA', N'dbo', 'TABLE', N'FedExShipment', 'COLUMN', N'WeightUnitType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DeclaredValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Description'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsProfileID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'3', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'DimsWeight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'EquaShipAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'InsuranceValue'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'119', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'PackageType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ReferenceNumber'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'118', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EquaShipShipment', 'COLUMN', N'ShippingNotes'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'iParcelAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'127', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'iParcelShipment', 'COLUMN', N'TrackBySMS'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'IntegratorTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'StampsShipment', 'COLUMN', N'StampsTransactionID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceFreight'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceInsurance'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoicePurpose'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoiceTermsOfSale'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialPaperlessInvoice'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDescription'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CustomsDocumentsOnly'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'116', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'DeliveryConfirmation'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyFrom'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyMessage'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOther'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyOtherAddress'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifyRecipient'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySender'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'EmailNotifySubject'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'NegotiatedRate'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorCountryCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorPostalCode'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'117', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PayorType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'PublishedCharges'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'115', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'Service'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'4', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'UpsAccountID'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'WorldShipStatus'
GO