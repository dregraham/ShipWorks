CREATE TABLE [dbo].[FedExShipment] (
    [ShipmentID]                                    BIGINT         NOT NULL,
    [FedExAccountID]                                BIGINT         NOT NULL,
    [MasterFormID]                                  VARCHAR (4)    NOT NULL,
    [Service]                                       INT            NOT NULL,
    [Signature]                                     INT            NOT NULL,
    [PackagingType]                                 INT            NOT NULL,
    [NonStandardContainer]                          BIT            NOT NULL,
    [ReferenceCustomer]                             NVARCHAR (300) NOT NULL,
    [ReferenceInvoice]                              NVARCHAR (300) NOT NULL,
    [ReferencePO]                                   NVARCHAR (300) NOT NULL,
    [PayorTransportType]                            INT            NOT NULL,
    [PayorTransportName]                            NVARCHAR (60)  NOT NULL,
    [PayorTransportAccount]                         VARCHAR (12)   NOT NULL,
    [PayorDutiesType]                               INT            NOT NULL,
    [PayorDutiesAccount]                            VARCHAR (12)   NOT NULL,
    [PayorDutiesName]                               NVARCHAR (60)  NOT NULL,
    [PayorDutiesCountryCode]                        NVARCHAR (50)  NOT NULL,
    [SaturdayDelivery]                              BIT            NOT NULL,
    [HomeDeliveryType]                              INT            NOT NULL,
    [HomeDeliveryInstructions]                      VARCHAR (74)   NOT NULL,
    [HomeDeliveryDate]                              DATETIME       NOT NULL,
    [HomeDeliveryPhone]                             VARCHAR (24)   NOT NULL,
    [FreightInsidePickup]                           BIT            NOT NULL,
    [FreightInsideDelivery]                         BIT            NOT NULL,
    [FreightBookingNumber]                          VARCHAR (12)   NOT NULL,
    [FreightLoadAndCount]                           INT            NOT NULL,
    [EmailNotifyBroker]                             INT            NOT NULL,
    [EmailNotifySender]                             INT            NOT NULL,
    [EmailNotifyRecipient]                          INT            NOT NULL,
    [EmailNotifyOther]                              INT            NOT NULL,
    [EmailNotifyOtherAddress]                       NVARCHAR (100) NOT NULL,
    [EmailNotifyMessage]                            VARCHAR (120)  NOT NULL,
    [CodEnabled]                                    BIT            NOT NULL,
    [CodAmount]                                     MONEY          NOT NULL,
    [CodPaymentType]                                INT            NOT NULL,
    [CodAddFreight]                                 BIT            NOT NULL,
    [CodOriginID]                                   BIGINT         NOT NULL,
    [CodFirstName]                                  NVARCHAR (30)  NOT NULL,
    [CodLastName]                                   NVARCHAR (30)  NOT NULL,
    [CodCompany]                                    NVARCHAR (35)  NOT NULL,
    [CodStreet1]                                    NVARCHAR (60)  NOT NULL,
    [CodStreet2]                                    NVARCHAR (60)  NOT NULL,
    [CodStreet3]                                    NVARCHAR (60)  NOT NULL,
    [CodCity]                                       NVARCHAR (50)  NOT NULL,
    [CodStateProvCode]                              NVARCHAR (50)  NOT NULL,
    [CodPostalCode]                                 NVARCHAR (20)  NOT NULL,
    [CodCountryCode]                                NVARCHAR (50)  NOT NULL,
    [CodPhone]                                      NVARCHAR (25)  NOT NULL,
    [CodTrackingNumber]                             VARCHAR (50)   NOT NULL,
    [CodTrackingFormID]                             VARCHAR (4)    NOT NULL,
    [CodTIN]                                        NVARCHAR (24)  NOT NULL,
    [CodChargeBasis]                                INT            NOT NULL,
    [CodAccountNumber]                              NVARCHAR (25)  NOT NULL,
    [BrokerEnabled]                                 BIT            NOT NULL,
    [BrokerAccount]                                 NVARCHAR (12)  NOT NULL,
    [BrokerFirstName]                               NVARCHAR (30)  NOT NULL,
    [BrokerLastName]                                NVARCHAR (30)  NOT NULL,
    [BrokerCompany]                                 NVARCHAR (35)  NOT NULL,
    [BrokerStreet1]                                 NVARCHAR (60)  NOT NULL,
    [BrokerStreet2]                                 NVARCHAR (60)  NOT NULL,
    [BrokerStreet3]                                 NVARCHAR (60)  NOT NULL,
    [BrokerCity]                                    NVARCHAR (50)  NOT NULL,
    [BrokerStateProvCode]                           NVARCHAR (50)  NOT NULL,
    [BrokerPostalCode]                              NVARCHAR (20)  NOT NULL,
    [BrokerCountryCode]                             NVARCHAR (50)  NOT NULL,
    [BrokerPhone]                                   NVARCHAR (25)  NOT NULL,
    [BrokerPhoneExtension]                          NVARCHAR (8)   NOT NULL,
    [BrokerEmail]                                   NVARCHAR (100) NOT NULL,
    [CustomsAdmissibilityPackaging]                 INT            NOT NULL,
    [CustomsRecipientTIN]                           VARCHAR (15)   NOT NULL,
    [CustomsDocumentsOnly]                          BIT            NOT NULL,
    [CustomsDocumentsDescription]                   NVARCHAR (150) NOT NULL,
    [CustomsExportFilingOption]                     INT            NOT NULL,
    [CustomsAESEEI]                                 NVARCHAR (100) NOT NULL,
    [CustomsRecipientIdentificationType]            INT            NOT NULL,
    [CustomsRecipientIdentificationValue]           NVARCHAR (50)  NOT NULL,
    [CustomsOptionsType]                            INT            NOT NULL,
    [CustomsOptionsDesription]                      NVARCHAR (32)  NOT NULL,
    [CommercialInvoice]                             BIT            NOT NULL,
    [CommercialInvoiceTermsOfSale]                  INT            NOT NULL,
    [CommercialInvoicePurpose]                      INT            NOT NULL,
    [CommercialInvoiceComments]                     NVARCHAR (200) NOT NULL,
    [CommercialInvoiceFreight]                      MONEY          NOT NULL,
    [CommercialInvoiceInsurance]                    MONEY          NOT NULL,
    [CommercialInvoiceOther]                        MONEY          NOT NULL,
    [CommercialInvoiceReference]                    NVARCHAR (300) NOT NULL,
    [ImporterOfRecord]                              BIT            NOT NULL,
    [ImporterAccount]                               NVARCHAR (12)  NOT NULL,
    [ImporterTIN]                                   NVARCHAR (15)  NOT NULL,
    [ImporterFirstName]                             NVARCHAR (30)  NOT NULL,
    [ImporterLastName]                              NVARCHAR (30)  NOT NULL,
    [ImporterCompany]                               NVARCHAR (35)  NOT NULL,
    [ImporterStreet1]                               NVARCHAR (60)  NOT NULL,
    [ImporterStreet2]                               NVARCHAR (60)  NOT NULL,
    [ImporterStreet3]                               NVARCHAR (60)  NOT NULL,
    [ImporterCity]                                  NVARCHAR (50)  NOT NULL,
    [ImporterStateProvCode]                         NVARCHAR (50)  NOT NULL,
    [ImporterPostalCode]                            NVARCHAR (10)  NOT NULL,
    [ImporterCountryCode]                           NVARCHAR (50)  NOT NULL,
    [ImporterPhone]                                 NVARCHAR (25)  NOT NULL,
    [SmartPostIndicia]                              INT            NOT NULL,
    [SmartPostEndorsement]                          INT            NOT NULL,
    [SmartPostConfirmation]                         BIT            NOT NULL,
    [SmartPostCustomerManifest]                     NVARCHAR (300) NOT NULL,
    [SmartPostHubID]                                VARCHAR (10)   NOT NULL,
    [DropoffType]                                   INT            NOT NULL,
    [OriginResidentialDetermination]                INT            NOT NULL,
    [FedExHoldAtLocationEnabled]                    BIT            NOT NULL,
    [HoldLocationId]                                NVARCHAR (50)  NULL,
    [HoldLocationType]                              INT            NULL,
    [HoldContactId]                                 NVARCHAR (50)  NULL,
    [HoldPersonName]                                NVARCHAR (100) NULL,
    [HoldTitle]                                     NVARCHAR (50)  NULL,
    [HoldCompanyName]                               NVARCHAR (50)  NULL,
    [HoldPhoneNumber]                               NVARCHAR (30)  NULL,
    [HoldPhoneExtension]                            NVARCHAR (10)  NULL,
    [HoldPagerNumber]                               NVARCHAR (30)  NULL,
    [HoldFaxNumber]                                 NVARCHAR (30)  NULL,
    [HoldEmailAddress]                              NVARCHAR (100) NULL,
    [HoldStreet1]                                   NVARCHAR (250) NULL,
    [HoldStreet2]                                   NVARCHAR (250) NULL,
    [HoldStreet3]                                   NVARCHAR (250) NULL,
    [HoldCity]                                      NVARCHAR (100) NULL,
    [HoldStateOrProvinceCode]                       NVARCHAR (50)  NULL,
    [HoldPostalCode]                                NVARCHAR (20)  NULL,
    [HoldUrbanizationCode]                          NVARCHAR (20)  NULL,
    [HoldCountryCode]                               NVARCHAR (20)  NULL,
    [HoldResidential]                               BIT            NULL,
    [CustomsNaftaEnabled]                           BIT            NOT NULL,
    [CustomsNaftaPreferenceType]                    INT            NOT NULL,
    [CustomsNaftaDeterminationCode]                 INT            NOT NULL,
    [CustomsNaftaProducerId]                        NVARCHAR (20)  NOT NULL,
    [CustomsNaftaNetCostMethod]                     INT            NOT NULL,
    [ReturnType]                                    INT            NOT NULL,
    [RmaNumber]                                     NVARCHAR (30)  NOT NULL,
    [RmaReason]                                     NVARCHAR (60)  NOT NULL,
    [ReturnSaturdayPickup]                          BIT            NOT NULL,
    [TrafficInArmsLicenseNumber]                    NVARCHAR (32)  NOT NULL,
    [IntlExportDetailType]                          INT            NOT NULL,
    [IntlExportDetailForeignTradeZoneCode]          NVARCHAR (50)  NOT NULL,
    [IntlExportDetailEntryNumber]                   NVARCHAR (20)  NOT NULL,
    [IntlExportDetailLicenseOrPermitNumber]         NVARCHAR (50)  NOT NULL,
    [IntlExportDetailLicenseOrPermitExpirationDate] DATETIME       NULL,
    [WeightUnitType]                                INT            NOT NULL,
    [LinearUnitType]                                INT            NOT NULL,
    CONSTRAINT [PK_FedExShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_FedExShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[FedExShipmentAuditTrigger]
    ON [dbo].[FedExShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FedExShipmentAuditTrigger]


GO
CREATE TRIGGER [dbo].[FilterDirtyFedExShipment]
    ON [dbo].[FedExShipment]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyFedExShipment]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FedExAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'MasterFormID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'108', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'Service';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'114', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'Signature';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PackagingType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'110', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorTransportType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill Transporation To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorTransportType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill Transportation Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorTransportName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill Transportation Account', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorTransportAccount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'110', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill duties/fees To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill duties/fees Account', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesAccount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill duties/fees Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Bill duties/fees Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'PayorDutiesCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'113', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HomeDeliveryType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Home Delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HomeDeliveryType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HomeDeliveryInstructions';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HomeDeliveryDate';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HomeDeliveryPhone';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FreightInsidePickup';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FreightInsideDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FreightBookingNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FreightLoadAndCount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyBroker';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifySender';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyRecipient';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyOther';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyOtherAddress';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyMessage';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'COD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodEnabled';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodAmount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'COD Amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodAmount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodPaymentType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodAddFreight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodOriginID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodFirstName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodLastName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodCompany';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodStreet1';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodStreet2';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodStreet3';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodCity';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodPostalCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodPhone';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodTrackingNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodTrackingFormID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodTIN';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodChargeBasis';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CodAccountNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerFirstName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerLastName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerCompany';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerStreet1';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerStreet2';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerStreet3';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerCity';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerPostalCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerPhone';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerPhoneExtension';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'BrokerEmail';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsAdmissibilityPackaging';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsRecipientTIN';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsDocumentsOnly';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsDocumentsDescription';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsExportFilingOption';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsAESEEI';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsRecipientIdentificationType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsRecipientIdentificationValue';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsOptionsType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsOptionsDesription';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoice';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceTermsOfSale';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoicePurpose';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceComments';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceFreight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceInsurance';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceOther';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceReference';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterOfRecord';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterAccount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterTIN';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterFirstName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterLastName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterCompany';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterStreet1';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterStreet2';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterStreet3';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterCity';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterPostalCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ImporterPhone';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'SmartPostIndicia';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'SmartPostEndorsement';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'SmartPostConfirmation';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'SmartPostCustomerManifest';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'SmartPostHubID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'123', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'DropoffType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Dropoff Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'DropoffType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'111', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'OriginResidentialDetermination';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Origin Residential \ Commercial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'OriginResidentialDetermination';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Hold At Location Selected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'FedExHoldAtLocationEnabled';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldLocationId';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldLocationType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldContactId';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldPersonName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldTitle';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldCompanyName';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldPhoneNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldPhoneExtension';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldPagerNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldFaxNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldEmailAddress';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldStreet1';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldStreet2';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldStreet3';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldCity';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldStateOrProvinceCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldPostalCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldUrbanizationCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'HoldResidential';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'NAFTA Selected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsNaftaEnabled';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsNaftaPreferenceType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsNaftaDeterminationCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsNaftaProducerId';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'CustomsNaftaNetCostMethod';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'124', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'ReturnType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'RMA Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'RmaNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'RMA Reason', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'RmaReason';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'TrafficInArmsLicenseNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'IntlExportDetailType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'IntlExportDetailForeignTradeZoneCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'IntlExportDetailEntryNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'IntlExportDetailLicenseOrPermitNumber';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'IntlExportDetailLicenseOrPermitExpirationDate';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'125', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'WeightUnitType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Weight Units', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'WeightUnitType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'126', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'LinearUnitType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Dimension Units', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FedExShipment', @level2type = N'COLUMN', @level2name = N'LinearUnitType';

