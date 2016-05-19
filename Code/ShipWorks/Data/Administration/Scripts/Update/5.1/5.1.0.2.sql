PRINT N'Altering [dbo].[ThreeDCartStore]'
GO
IF COL_LENGTH(N'[dbo].[ThreeDCartStore]', N'RestUser') IS NULL
ALTER TABLE [dbo].[ThreeDCartStore] ADD [RestUser] [bit] DEFAULT ((0))
GO

PRINT N'Creating [dbo].[ThreeDCartOrder]'
GO
CREATE TABLE [dbo].[ThreeDCartOrder]
(
[OrderID] [bigint] NOT NULL,
[ThreeDCartOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrder] on [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [PK_ThreeDCartOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrder]'
GO
ALTER TABLE [dbo].[ThreeDCartOrder] ADD CONSTRAINT [FK_ThreeDCartOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO