SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD
[GuaranteedDelivery] [bit] NOT NULL CONSTRAINT [DF_EbayOrder_GuaranteedDelivery] DEFAULT ((0))
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF_EbayOrder_GuaranteedDelivery]
GO
PRINT N'Creating index [IX_EbayOrder_GuaranteedDelivery] on [dbo].[EbayOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrder_GuaranteedDelivery] ON [dbo].[EbayOrder] ([GuaranteedDelivery])
GO