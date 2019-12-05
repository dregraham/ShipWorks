PRINT N'Adding columns to [dbo].[EbayOrder]'
GO
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('EbayOrder') AND [name] = 'ExtendedOrderID')
BEGIN
	ALTER TABLE EbayOrder
		ADD ExtendedOrderID VARCHAR(25) NOT NULL CONSTRAINT [DF_EbayOrder_ExtendedOrderID] DEFAULT('')
END
GO

PRINT N'Adding columns to [dbo].[EbayOrderItem]'
GO
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('EbayOrderItem') AND [name] = 'ExtendedOrderID')
BEGIN
	ALTER TABLE EbayOrderItem
		ADD ExtendedOrderID VARCHAR(25) NOT NULL CONSTRAINT [DF_EbayOrderItem_ExtendedOrderID] DEFAULT('')
END
GO

PRINT N'Adding columns to [dbo].[EbayOrderSearch]'
GO
IF NOT EXISTS(SELECT column_id FROM sys.all_columns WHERE object_id = OBJECT_ID('EbayOrderSearch') AND [name] = 'ExtendedOrderID')
BEGIN
	ALTER TABLE EbayOrderSearch
		ADD ExtendedOrderID VARCHAR(25) NOT NULL CONSTRAINT [DF_EbayOrderSearch_ExtendedOrderID] DEFAULT('')
END
GO

PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ExtendedOrderID' AND object_id = OBJECT_ID(N'[dbo].[EbayOrder]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_EbayOrder_ExtendedOrderID]', 'D'))
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF_EbayOrder_ExtendedOrderID]
GO

PRINT N'Dropping constraints from [dbo].[EbayOrderSearch]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ExtendedOrderID' AND object_id = OBJECT_ID(N'[dbo].[EbayOrderSearch]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_EbayOrderSearch_ExtendedOrderID]', 'D'))
ALTER TABLE [dbo].[EbayOrderSearch] DROP CONSTRAINT [DF_EbayOrderSearch_ExtendedOrderID]
GO

PRINT N'Dropping constraints from [dbo].[EbayOrderItem]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'ExtendedOrderID' AND object_id = OBJECT_ID(N'[dbo].[EbayOrderItem]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_EbayOrderItem_ExtendedOrderID]', 'D'))
ALTER TABLE [dbo].[EbayOrderItem] DROP CONSTRAINT [DF_EbayOrderItem_ExtendedOrderID]
GO