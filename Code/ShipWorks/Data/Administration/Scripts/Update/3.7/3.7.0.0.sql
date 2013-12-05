SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'BuyerFeedbackPrivate'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'BuyerFeedbackScore'
GO
PRINT N'Altering [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP
COLUMN [BuyerFeedbackScore],
COLUMN [BuyerFeedbackPrivate]
GO
PRINT N'Altering [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] DROP
COLUMN [SellingManagerProductName],
COLUMN [SellingManagerProductPart],
COLUMN [CheckoutStatus],
COLUMN [SellerPaidStatus]
GO
PRINT N'Altering [dbo].[EbayStore]'
GO
ALTER TABLE [dbo].[EbayStore] ADD
[FeedbackUpdatedThrough] [datetime] NULL
GO
