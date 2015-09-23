PRINT N'Creating [dbo].[LemonStandOrder]'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LemonStandOrder](
	[OrderID] [bigint] NOT NULL,
	[LemonStandOrderID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_LemonStandOrder] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LemonStandOrder]  WITH CHECK ADD  CONSTRAINT [FK_LemonStandOrder_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO

ALTER TABLE [dbo].[LemonStandOrder] CHECK CONSTRAINT [FK_LemonStandOrder_Order]
GO


PRINT N'Creating [dbo].[LemonStandStore]'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LemonStandStore](
	[StoreID] [bigint] NOT NULL,
	[Token] [varchar](255) NOT NULL,
	[StoreURL] [varchar](255) NOT NULL,
 CONSTRAINT [PK_LemonStandStore] PRIMARY KEY CLUSTERED 
(
	[StoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LemonStandStore]  WITH CHECK ADD  CONSTRAINT [FK_LemonStandStore_Store] FOREIGN KEY([StoreID])
REFERENCES [dbo].[Store] ([StoreID])
GO

ALTER TABLE [dbo].[LemonStandStore] CHECK CONSTRAINT [FK_LemonStandStore_Store]
GO


PRINT N'Creating [dbo].[LemonStandOrderItem]'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LemonStandOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[UrlName] [nvarchar](255) NOT NULL,
	[Cost] [nvarchar](255) NOT NULL,
	[IsOnSale] [nvarchar](255) NOT NULL,
	[SalePriceOrDiscount] [nvarchar](255),
	[ShortDescription] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_LemonStandOrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LemonStandOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_LemonStandOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[LemonStandOrderItem] CHECK CONSTRAINT [FK_LemonStandOrderItem_OrderItem]
GO




