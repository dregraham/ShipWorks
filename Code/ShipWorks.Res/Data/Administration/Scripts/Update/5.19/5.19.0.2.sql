SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[DhlExpressProfile]'
GO
CREATE TABLE [dbo].[DhlExpressProfile](
	[ShippingProfileID] [bigint] NOT NULL,
	[DhlExpressAccountID] [bigint] NULL,
	[Service] [int] NULL,
	[DeliveryDutyPaid] [bit] NULL,
	[NonMachinable] [bit] NULL,
	[SaturdayDelivery] [bit] NULL,
	[Contents][int] Null,
	[NonDelivery] [int] Null
)
GO
PRINT N'Creating primary key [PK_DhlExpressProfile] on [dbo].[DhlExpressProfile]'
GO
ALTER TABLE [dbo].[DhlExpressProfile] ADD CONSTRAINT [PK_DhlExpressProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressProfile]'
GO
ALTER TABLE [dbo].[DhlExpressProfile] ADD CONSTRAINT [FK_DhlExpressProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[DhlExpressProfilePackage]'
GO
CREATE TABLE [dbo].[DhlExpressProfilePackage](
	[DhlExpressProfilePackageID] [bigint] IDENTITY(1094,1000) NOT NULL,
	[ShippingProfileID] [bigint] NOT NULL,
	[Weight] [float] NULL,
	[DimsProfileID] [bigint] NULL,
	[DimsLength] [float] NULL,
	[DimsWidth] [float] NULL,
	[DimsHeight] [float] NULL,
	[DimsWeight] [float] NULL,
	[DimsAddWeight] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_DhlExpressPackageProfile] on [dbo].[DhlExpressProfilePackage]'
GO
ALTER TABLE [dbo].[DhlExpressProfilePackage] ADD CONSTRAINT [PK_DhlExpressPackageProfile] PRIMARY KEY CLUSTERED  ([DhlExpressProfilePackageID])
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressProfilePackage]'
GO
ALTER TABLE [dbo].[DhlExpressProfilePackage] ADD CONSTRAINT [FK_DhlExpressPackageProfile_DhlExpressProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[DhlExpressProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO

