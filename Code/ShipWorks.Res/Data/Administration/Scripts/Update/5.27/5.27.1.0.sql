PRINT N'Altering [dbo].[AmazonShipment]'
GO
IF COL_LENGTH(N'[dbo].[AmazonShipment]', N'RequestedLabelFormat') IS NULL
	ALTER TABLE [dbo].[AmazonShipment] ADD [RequestedLabelFormat] [INT] NOT NULL CONSTRAINT [DF_AmazonShipment_RequestedLabelFormat] DEFAULT (-1)
GO


