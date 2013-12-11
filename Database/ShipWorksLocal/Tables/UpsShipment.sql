CREATE TABLE [dbo].[UpsShipment] (
    [ShipmentID]                       BIGINT         NOT NULL,
    [UpsAccountID]                     BIGINT         NOT NULL,
    [Service]                          INT            NOT NULL,
    [SaturdayDelivery]                 BIT            NOT NULL,
    [CodEnabled]                       BIT            NOT NULL,
    [CodAmount]                        MONEY          NOT NULL,
    [CodPaymentType]                   INT            NOT NULL,
    [DeliveryConfirmation]             INT            NOT NULL,
    [ReferenceNumber]                  NVARCHAR (300) NOT NULL,
    [ReferenceNumber2]                 NVARCHAR (300) NOT NULL,
    [PayorType]                        INT            NOT NULL,
    [PayorAccount]                     VARCHAR (10)   NOT NULL,
    [PayorPostalCode]                  NVARCHAR (20)  NOT NULL,
    [PayorCountryCode]                 NVARCHAR (50)  NOT NULL,
    [EmailNotifySender]                INT            NOT NULL,
    [EmailNotifyRecipient]             INT            NOT NULL,
    [EmailNotifyOther]                 INT            NOT NULL,
    [EmailNotifyOtherAddress]          NVARCHAR (100) NOT NULL,
    [EmailNotifyFrom]                  NVARCHAR (100) NOT NULL,
    [EmailNotifySubject]               INT            NOT NULL,
    [EmailNotifyMessage]               NVARCHAR (120) NOT NULL,
    [CustomsDocumentsOnly]             BIT            NOT NULL,
    [CustomsDescription]               NVARCHAR (150) NOT NULL,
    [CommercialPaperlessInvoice]       BIT            NOT NULL,
    [CommercialInvoiceTermsOfSale]     INT            NOT NULL,
    [CommercialInvoicePurpose]         INT            NOT NULL,
    [CommercialInvoiceComments]        NVARCHAR (200) NOT NULL,
    [CommercialInvoiceFreight]         MONEY          NOT NULL,
    [CommercialInvoiceInsurance]       MONEY          NOT NULL,
    [CommercialInvoiceOther]           MONEY          NOT NULL,
    [WorldShipStatus]                  INT            NOT NULL,
    [PublishedCharges]                 MONEY          NOT NULL,
    [NegotiatedRate]                   BIT            NOT NULL,
    [ReturnService]                    INT            NOT NULL,
    [ReturnUndeliverableEmail]         NVARCHAR (100) NOT NULL,
    [ReturnContents]                   NVARCHAR (300) NOT NULL,
    [UspsTrackingNumber]               NVARCHAR (50)  NOT NULL,
    [Endorsement]                      INT            NOT NULL,
    [Subclassification]                INT            NOT NULL,
    [PaperlessAdditionalDocumentation] BIT            NOT NULL,
    [ShipperRelease]                   BIT            NOT NULL,
    [CarbonNeutral]                    BIT            NOT NULL,
    [CostCenter]                       NVARCHAR (30)  NOT NULL,
    [IrregularIndicator]               INT            NOT NULL,
    [Cn22Number]                       NVARCHAR (255) NOT NULL,
    [ShipmentChargeType]               INT            NOT NULL,
    [ShipmentChargeAccount]            VARCHAR (10)   NOT NULL,
    [ShipmentChargePostalCode]         NVARCHAR (20)  NOT NULL,
    [ShipmentChargeCountryCode]        NVARCHAR (50)  NOT NULL,
    [UspsPackageID]                    NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_UpsShipment_Shipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[Shipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[FilterDirtyUpsShipment]
    ON [dbo].[UpsShipment]
    AFTER UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyUpsShipment]


GO
CREATE TRIGGER [dbo].[UpsShipmentAuditTrigger]
    ON [dbo].[UpsShipment]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[UpsShipmentAuditTrigger]


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'UpsAccountID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'115', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'Service';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'COD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CodEnabled';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CodAmount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'COD Amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CodAmount';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CodPaymentType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'116', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'DeliveryConfirmation';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'117', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'PayorType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'PayorPostalCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'PayorCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifySender';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyRecipient';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyOther';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyOtherAddress';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyFrom';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifySubject';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'EmailNotifyMessage';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CustomsDocumentsOnly';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CustomsDescription';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialPaperlessInvoice';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceTermsOfSale';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoicePurpose';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceComments';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceFreight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceInsurance';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'CommercialInvoiceOther';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'WorldShipStatus';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'PublishedCharges';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UpsShipment', @level2type = N'COLUMN', @level2name = N'NegotiatedRate';

