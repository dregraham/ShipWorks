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