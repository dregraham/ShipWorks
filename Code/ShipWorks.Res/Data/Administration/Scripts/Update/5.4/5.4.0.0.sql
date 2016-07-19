SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD
	[PurchaseOrderNumber] [nvarchar](50) NOT NULL CONSTRAINT [DF_AmazonOrder_PurchaseOrderNumber] DEFAULT('')
GO
PRINT N'Dropping constraints from [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] DROP CONSTRAINT [DF_AmazonOrder_PurchaseOrderNumber]
GO