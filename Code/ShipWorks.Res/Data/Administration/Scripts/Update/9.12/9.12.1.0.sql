PRINT N'Altering [dbo].[OrderItem]'
GO

IF COL_LENGTH(N'[dbo].[OrderItem]', N'StoreOrderItemID') IS NULL
BEGIN
	ALTER TABLE [dbo].[OrderItem] ADD [StoreOrderItemID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO
