SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [RollupItemTotalWeight] [decimal] (29, 9) NOT NULL
GO
PRINT N'Altering [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] DROP COLUMN [TotalWeight]
GO
ALTER TABLE [dbo].[OrderItem] ALTER COLUMN [Weight] [decimal] (29, 9) NOT NULL
GO
ALTER TABLE [dbo].[OrderItem] ALTER COLUMN [Quantity] [decimal] (29, 9) NOT NULL
GO
ALTER TABLE [dbo].[OrderItem] ADD [TotalWeight] AS ([Weight]*[Quantity])
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ALTER COLUMN [TotalWeight] [decimal] (29, 9) NOT NULL
GO

