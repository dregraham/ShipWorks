PRINT N'Altering [dbo].[Order]'
GO
IF COL_LENGTH('Order','Custom6') IS NULL AND 
COL_LENGTH('Order','Custom7') IS NULL AND 
COL_LENGTH('Order','Custom8') IS NULL AND 
COL_LENGTH('Order','Custom9') IS NULL AND 
COL_LENGTH('Order','Custom10') IS NULL 
BEGIN
	ALTER TABLE [Order]
		ADD [Custom6] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom6] DEFAULT (''),
		    [Custom7] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom7] DEFAULT (''),
			[Custom8] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom8] DEFAULT (''),
			[Custom9] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom9] DEFAULT (''),
			[Custom10] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Order_Custom10] DEFAULT ('')
END
GO
PRINT N'Altering [dbo].[OrderItem]'
GO
IF COL_LENGTH('OrderItem','Custom6') IS NULL AND 
COL_LENGTH('OrderItem','Custom7') IS NULL AND 
COL_LENGTH('OrderItem','Custom8') IS NULL AND 
COL_LENGTH('OrderItem','Custom9') IS NULL AND 
COL_LENGTH('OrderItem','Custom10') IS NULL 
BEGIN
	ALTER TABLE [OrderItem]
		ADD [Custom6] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom6] DEFAULT (''),
		    [Custom7] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom7] DEFAULT (''),
			[Custom8] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom8] DEFAULT (''),
			[Custom9] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom9] DEFAULT (''),
			[Custom10] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_OrderItem_Custom10] DEFAULT ('')
END
GO