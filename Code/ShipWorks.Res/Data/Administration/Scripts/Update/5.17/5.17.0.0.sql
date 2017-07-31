SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[JetStore]'
GO
CREATE TABLE [dbo].[JetStore]
(
	[StoreID] [bigint] NOT NULL,
	[ApiUser] [nvarchar](100) NOT NULL,
	[Secret] [nvarchar](100) NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetStore] on [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [PK_JetStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[JetStore]'
GO
ALTER TABLE [dbo].[JetStore] ADD CONSTRAINT [FK_JetStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO