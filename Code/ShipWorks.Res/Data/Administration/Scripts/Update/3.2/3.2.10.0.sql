SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] DROP CONSTRAINT[FK_NeweggStore_Store]
GO
PRINT N'Dropping constraints from [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] DROP CONSTRAINT [PK_NeweggStore]
GO
PRINT N'Rebuilding [dbo].[NeweggStore]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_NeweggStore]
(
[StoreID] [bigint] NOT NULL,
[SellerID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SecretKey] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExcludeFulfilledByNewegg] [bit] NOT NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_NeweggStore]([StoreID], [SellerID], [SecretKey], [ExcludeFulfilledByNewegg]) SELECT [StoreID], [SellerID], [SecretKey], 0 FROM [dbo].[NeweggStore]
GO
DROP TABLE [dbo].[NeweggStore]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_NeweggStore]', N'NeweggStore'
GO
PRINT N'Creating primary key [PK_NeweggStore] on [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [PK_NeweggStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[NeweggStore]'
GO
ALTER TABLE [dbo].[NeweggStore] ADD CONSTRAINT [FK_NeweggStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
