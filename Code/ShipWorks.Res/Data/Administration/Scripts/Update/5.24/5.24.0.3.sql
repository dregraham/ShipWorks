PRINT N'Dropping constraints from [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] DROP CONSTRAINT [FK_BigCommerceOrderItem_OrderItem]
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] DROP CONSTRAINT [PK_BigCommerceOrderItem]
GO

PRINT N'Rebuilding [dbo].[BigCommerceOrderItem]'
GO

CREATE TABLE [dbo].[tmp_rg_xx_BigCommerceOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[OrderAddressID] [bigint] NOT NULL,
	[OrderProductID] [bigint] NOT NULL,
	[ParentOrderProductID] [bigint] NULL,
	[IsDigitalItem] [bit] NOT NULL,
	[EventDate] [datetime] NULL,
	[EventName] [nvarchar](255) NULL,
)
GO

INSERT INTO [dbo].[tmp_rg_xx_BigCommerceOrderItem]([OrderItemID], [OrderAddressID],[OrderProductID],[ParentOrderProductID],[IsDigitalItem],[EventDate],[EventName]) 
SELECT [OrderItemID], [OrderAddressID], [OrderProductID], NULL, [IsDigitalItem], [EventDate], [EventName] FROM [dbo].[BigCommerceOrderItem]
GO

DROP TABLE [dbo].[BigCommerceOrderItem]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BigCommerceOrderItem]', N'BigCommerceOrderItem'
GO
PRINT N'Creating primary key [PK_BigCommerceOrderItem] on [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] ADD  CONSTRAINT [PK_BigCommerceOrderItem] PRIMARY KEY CLUSTERED ([OrderItemID])
GO
PRINT N'Creating foreign key [PK_BigCommerceOrderItem] on [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_BigCommerceOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO