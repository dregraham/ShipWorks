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

PRINT N'Creating [dbo].[SparkPayStore]'
GO
CREATE TABLE [dbo].[SparkPayStore]
(
[StoreID] [bigint] NOT NULL,
[Token] [nvarchar] (70) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StoreUrl] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StatusCodes] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_SparkPayStore] on [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] ADD CONSTRAINT [PK_SparkPayStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO

PRINT N'Adding foreign keys to [dbo].[SparkPayStore]'
GO
ALTER TABLE [dbo].[SparkPayStore] ADD CONSTRAINT [FK_SparkPayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO