SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[OdbcStore]'
GO
CREATE TABLE [dbo].[OdbcStore]
(
	[StoreID] [bigint] NOT NULL,
	[ImportConnectionString] [nvarchar](2048) NOT NULL,
	[ImportMap] [nvarchar](max) NOT NULL,
	[ImportStrategy] [int] NOT NULL,
	[ImportColumnSourceType] [int] NOT NULL,
	[ImportColumnSource] [nvarchar](2048) NOT NULL,
	[UploadStrategy] [int] NOT NULL,
	[UploadMap] [nvarchar](max) NOT NULL,
	[UploadColumnSourceType] [int] NOT NULL,
	[UploadColumnSource] [nvarchar](2048) NOT NULL,
	[UploadConnectionString] [nvarchar](2048) NOT NULL,
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
PRINT N'Altering [dbo].[ShippingSettings]'
GO
ALTER TABLE [dbo].[ShippingSettings] ADD
[ShipmentEditLimit] [int] NOT NULL CONSTRAINT [DF_ShippingSettings_ShipmentEditLimit] DEFAULT ((100000))
GO
PRINT N'Dropping constraints from [dbo].[[ShippingSettings]]'
GO
ALTER TABLE [dbo].[ShippingSettings] DROP CONSTRAINT [DF_ShippingSettings_ShipmentEditLimit]
GO