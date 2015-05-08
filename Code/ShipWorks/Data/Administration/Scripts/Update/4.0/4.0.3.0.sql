SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] DROP CONSTRAINT [FK_EbayCombinedOrderRelation_EbayStore]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] DROP CONSTRAINT [FK_EbayStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] DROP CONSTRAINT [PK_EbayStore]
GO
PRINT N'Rebuilding [dbo].[EbayStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EbayStore]
(
[StoreID] [bigint] NOT NULL,
[eBayUserID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[eBayToken] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[eBayTokenExpire] [datetime] NOT NULL,
[AcceptedPaymentList] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DownloadItemDetails] [bit] NOT NULL,
[DownloadOlderOrders] [bit] NOT NULL,
[DownloadPayPalDetails] [bit] NOT NULL,
[PayPalApiCredentialType] [smallint] NOT NULL,
[PayPalApiUserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiPassword] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiSignature] [nvarchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PayPalApiCertificate] [varbinary] (2048) NULL,
[DomesticShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InternationalShippingService] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FeedbackUpdatedThrough] [datetime] NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_EbayStore]([StoreID], [eBayUserID], [eBayToken], [eBayTokenExpire], [AcceptedPaymentList], [DownloadItemDetails], [DownloadPayPalDetails], [PayPalApiCredentialType], [PayPalApiUserName], [PayPalApiPassword], [PayPalApiSignature], [PayPalApiCertificate], [DomesticShippingService], [InternationalShippingService], [FeedbackUpdatedThrough], DownloadOlderOrders) 
SELECT [StoreID], [eBayUserID], [eBayToken], [eBayTokenExpire], [AcceptedPaymentList], [DownloadItemDetails], [DownloadPayPalDetails], [PayPalApiCredentialType], [PayPalApiUserName], [PayPalApiPassword], [PayPalApiSignature], [PayPalApiCertificate], [DomesticShippingService], [InternationalShippingService], [FeedbackUpdatedThrough], 0 FROM [dbo].[EbayStore]
GO
DROP TABLE [dbo].[EbayStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EbayStore]', N'EbayStore'
GO
PRINT N'Creating primary key [PK_EbayStore] on [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD CONSTRAINT [PK_EbayStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayCombinedOrderRelation]'
GO
ALTER TABLE [dbo].[EbayCombinedOrderRelation] ADD CONSTRAINT [FK_EbayCombinedOrderRelation_EbayStore] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[EbayStore] ([StoreID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
