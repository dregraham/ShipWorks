SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] DROP CONSTRAINT[FK_OrderMotionOrder_Order]
GO
PRINT N'Dropping constraints from [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] DROP CONSTRAINT [PK_OrderMotionOrder]
GO
PRINT N'Rebuilding [dbo].[OrderMotionOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_OrderMotionOrder]
(
[OrderID] [bigint] NOT NULL,
[OrderMotionShipmentID] [int] NOT NULL,
[OrderMotionPromotion] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OrderMotionInvoiceNumber] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO

-- Backfill the OrderMotionShipmentNumber field
INSERT INTO [dbo].[tmp_rg_xx_OrderMotionOrder]([OrderID], [OrderMotionShipmentID], [OrderMotionPromotion], [OrderMotionInvoiceNumber]) 
	SELECT 
		[OrderMotionOrder].[OrderID], 
		[OrderMotionShipmentID], 
		[OrderMotionPromotion],
		CAST([Order].OrderNumber as NVARCHAR(50)) + '-' + (CAST([OrderMotionShipmentID] as nvarchar(10)))
	FROM [dbo].[OrderMotionOrder]
	INNER JOIN [dbo].[Order]
		ON [dbo].[Order].OrderID = [dbo].[OrderMotionOrder].OrderID
GO
DROP TABLE [dbo].[OrderMotionOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_OrderMotionOrder]', N'OrderMotionOrder'
GO
PRINT N'Creating primary key [PK_OrderMotionOrder] on [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD CONSTRAINT [PK_OrderMotionOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[OrderMotionOrder]'
GO
ALTER TABLE [dbo].[OrderMotionOrder] ADD CONSTRAINT [FK_OrderMotionOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
