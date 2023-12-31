SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoice'
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
PRINT N'Dropping constraints from [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] DROP CONSTRAINT [PK_UpsShipment]
GO
PRINT N'Altering [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD
[ShipperRelease] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
[CommercialInvoice] [bit] NOT NULL,
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
[PaperlessInternational] [bit] NOT NULL,
[ShipperRelease] [bit] NOT NULL,
[CarbonNeutral] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsShipment]([ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], [ReferenceNumber2], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], [UspsTrackingNumber], Endorsement, Subclassification, PaperlessInternational, ShipperRelease, CarbonNeutral) 
                                   SELECT [ShipmentID], [UpsAccountID], [Service], [SaturdayDelivery], [CodEnabled], [CodAmount], [CodPaymentType], [DeliveryConfirmation], [ReferenceNumber], [ReferenceNumber2], [PayorType], [PayorAccount], [PayorPostalCode], [PayorCountryCode], [EmailNotifySender], [EmailNotifyRecipient], [EmailNotifyOther], [EmailNotifyOtherAddress], [EmailNotifyFrom], [EmailNotifySubject], [EmailNotifyMessage], [CustomsDocumentsOnly], [CustomsDescription], [CommercialInvoice], [CommercialInvoiceTermsOfSale], [CommercialInvoicePurpose], [CommercialInvoiceComments], [CommercialInvoiceFreight], [CommercialInvoiceInsurance], [CommercialInvoiceOther], [WorldShipStatus], [PublishedCharges], [NegotiatedRate], [ReturnService], [ReturnUndeliverableEmail], [ReturnContents], [UspsTrackingNumber], 0,           0,                 0,                      0,              0 FROM [dbo].[UpsShipment]
GO
DROP TABLE [dbo].[UpsShipment]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsShipment]', N'UpsShipment'
GO
PRINT N'Creating primary key [PK_UpsShipment] on [dbo].[UpsShipment]'
GO
ALTER TABLE [dbo].[UpsShipment] ADD CONSTRAINT [PK_UpsShipment] PRIMARY KEY CLUSTERED  ([ShipmentID])
GO
PRINT N'Altering [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD
[Endorsement] [int] NULL,
[Subclassification] [int] NULL,
[PaperlessInternational] [bit] NULL,
[ShipperRelease] [bit] NULL,
[CarbonNeutral] [bit] NULL
GO
PRINT N'Altering [dbo].[WorldShipShipment]'
GO
ALTER TABLE [dbo].[WorldShipShipment] ADD
[UspsEndorsement] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CarbonNeutral] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [CustomsDescriptionOfGoods] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [CustomsDocumentsOnly] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceTermsOfSale] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceReasonForExport] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceComments] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceChargesFreight] [money] NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceChargesInsurance] [money] NULL
ALTER TABLE [dbo].[WorldShipShipment] ALTER COLUMN [InvoiceChargesOther] [money] NULL
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
EXEC sp_addextendedproperty N'AuditFormat', N'2', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD Amount', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodAmount'
GO
EXEC sp_addextendedproperty N'AuditName', N'COD', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodEnabled'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CodPaymentType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'UpsShipment', 'COLUMN', N'CommercialInvoice'
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


PRINT N'Updating primary UPS Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	Endorsement = 0,
	Subclassification = 0,
	PaperlessInternational = 0,
	ShipperRelease = 0,
	CarbonNeutral = 0
WHERE ShippingProfileID in (SELECT ShippingProfileID FROM ShippingProfile WHERE ShipmentType IN (0,1) AND ShipmentTypePrimary = 1)