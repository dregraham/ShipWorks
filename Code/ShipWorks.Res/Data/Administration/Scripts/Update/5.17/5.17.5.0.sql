PRINT N'Altering [dbo].[BuyDotComOrderItem]'
GO
ALTER TABLE [dbo].[BuyDotComOrderItem] ALTER COLUMN [ReceiptItemID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
