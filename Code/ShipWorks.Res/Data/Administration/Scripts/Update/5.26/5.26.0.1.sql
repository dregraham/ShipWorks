PRINT N'Altering [dbo].[GrouponOrderItem]'
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'PONumber' AND Object_ID = Object_ID(N'GrouponOrderItem'))
BEGIN
	ALTER TABLE [dbo].[GrouponOrderItem] ADD
	[PONumber] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GrouponOrderItem_PONumber] DEFAULT ('')
	
	ALTER TABLE [dbo].[GrouponOrderItem] DROP CONSTRAINT [DF_GrouponOrderItem_PONumber]
END
GO