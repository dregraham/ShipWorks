SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] DROP CONSTRAINT [FK_AmazonOrder_Order]
GO
PRINT N'Dropping constraints from [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] DROP CONSTRAINT [PK_AmazonOrder]
GO
PRINT N'Rebuilding [dbo].[AmazonOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_AmazonOrder]
(
[OrderID] [bigint] NOT NULL,
[AmazonOrderID] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AmazonCommission] [money] NOT NULL,
[FulfillmentChannel] [int] NOT NULL,
[IsPrime] [int] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_AmazonOrder]([OrderID], [AmazonOrderID], [AmazonCommission], [FulfillmentChannel], [IsPrime]) 
SELECT [OrderID], [AmazonOrderID], [AmazonCommission], [FulfillmentChannel], 0 FROM [dbo].[AmazonOrder]
GO
DROP TABLE [dbo].[AmazonOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_AmazonOrder]', N'AmazonOrder'
GO
PRINT N'Creating primary key [PK_AmazonOrder] on [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [PK_AmazonOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[AmazonOrder]'
GO
ALTER TABLE [dbo].[AmazonOrder] ADD CONSTRAINT [FK_AmazonOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Creating index [IX_Auto_AmazonOrderID] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_AmazonOrderID] ON [dbo].[AmazonOrder] ([AmazonOrderID])
GO
PRINT N'Creating index [IX_Auto_FulfillmentChannel] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_FulfillmentChannel] ON [dbo].[AmazonOrder] ([FulFillmentChannel])
GO
PRINT N'Creating index [IX_Auto_IsPrime] on [dbo].[AmazonOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_Auto_IsPrime] ON [dbo].[AmazonOrder] ([IsPrime])
GO