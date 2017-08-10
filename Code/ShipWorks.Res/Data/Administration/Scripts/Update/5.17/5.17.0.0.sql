PRINT N'Creating table to [dbo].[EtsyOrderItem]'
GO
CREATE TABLE [dbo].[EtsyOrderItem](
	[OrderItemID] [bigint] NOT NULL,
	[TransactionID] [int] NOT NULL,
	[ListingID] [int] NOT NULL
 CONSTRAINT [PK_EtsyOrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EtsyOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_EtsyOrderItem_OrderItem] FOREIGN KEY([OrderItemID])
REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO

ALTER TABLE [dbo].[EtsyOrderItem] CHECK CONSTRAINT [FK_EtsyOrderItem_OrderItem]
GO