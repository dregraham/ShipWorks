SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[OdbcStore]'
GO
CREATE TABLE [dbo].[OdbcStore]
(
	[StoreID] [bigint] NOT NULL,
	[ConnectionString] [nvarchar](2048) NOT NULL
)
GO
PRINT N'Creating primary key [PK_OdbcStore] on [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [PK_OdbcStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[OdbcStore]'
GO
ALTER TABLE [dbo].[OdbcStore] ADD CONSTRAINT [FK_OdbcStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO