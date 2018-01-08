PRINT N'Creating table GenericModuleOrder'
GO
CREATE TABLE [dbo].[GenericModuleOrder](
	[OrderID] [bigint] NOT NULL,
	[AmazonOrderID] [varchar](32) NOT NULL,
	[IsFBA] [bit] NOT NULL,
	[IsPrime] [int] NOT NULL,
	[IsSameDay] bit NOT NULL
 CONSTRAINT [PK_GenericModuleOrder] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GenericModuleOrder]  WITH CHECK ADD  CONSTRAINT [FK_GenericModuleOrder_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO

ALTER TABLE [dbo].[GenericModuleOrder] CHECK CONSTRAINT [FK_GenericModuleOrder_Order]
GO

PRINT N'Creating table GenericModuleOrderItem'
GO
CREATE TABLE [dbo].[GenericModuleOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[AmazonOrderItemCode] [nvarchar](64) NOT NULL
 CONSTRAINT [PK_GenericModuleOrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GenericModuleOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_GenericModuleOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[GenericModuleOrderItem] CHECK CONSTRAINT [FK_GenericModuleOrderItem_OrderItem]
GO

PRINT N'Altering [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] ADD
[AmazonMerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericModuleStore_AmazonMerchantID] DEFAULT (''),
[AmazonAuthToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericModuleStore_AmazonAuthToken] DEFAULT (''),
[AmazonApiRegion] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_GenericModuleStore_AmazonApiRegion] DEFAULT ('')
GO

PRINT N'Dropping constraints from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [DF_GenericModuleStore_AmazonMerchantID]
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [DF_GenericModuleStore_AmazonAuthToken]
GO
PRINT N'Dropping constraints from [dbo].[GenericModuleStore]'
GO
ALTER TABLE [dbo].[GenericModuleStore] DROP CONSTRAINT [DF_GenericModuleStore_AmazonApiRegion]
GO