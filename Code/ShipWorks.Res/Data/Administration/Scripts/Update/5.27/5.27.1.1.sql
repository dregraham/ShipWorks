PRINT N'Altering [dbo].[AmazonShipment]'
GO
IF COL_LENGTH(N'[dbo].[AmazonShipment]', N'Reference1') IS NULL
	ALTER TABLE [dbo].[AmazonShipment] ADD [Reference1] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AmazonShipment_ReferenceNumber] DEFAULT ('')
GO


