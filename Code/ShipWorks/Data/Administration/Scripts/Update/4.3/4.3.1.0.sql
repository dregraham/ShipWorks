SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

PRINT N'Altering [dbo].[AmazonOrder]'
ALTER TABLE [dbo].[AmazonOrder] ADD
	[EarliestExpectedDeliveryDate] [datetime] NULL,
	[LatestExpectedDeliveryDate] [datetime] NULL
GO
PRINT N'Creating index [IX_Auto_EarliestExpectedDeliveryDate] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_EarliestExpectedDeliveryDate] ON [dbo].[AmazonOrder] ([EarliestExpectedDeliveryDate])
GO
PRINT N'Creating index [IX_Auto_LatestExpectedDeliveryDate] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_LatestExpectedDeliveryDate] ON [dbo].[AmazonOrder] ([LatestExpectedDeliveryDate])
GO