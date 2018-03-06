﻿CREATE TABLE [dbo].[Shortcut](
	[ShortcutID] [bigint] NOT NULL IDENTITY(1105, 1000),
	[RowVersion] [timestamp] NOT NULL,
	[Barcode] [nvarchar](50) NOT NULL,
	[Hotkey] [int] NULL,
	[Action] [int] NOT NULL,
	[RelatedObjectID] [bigint] NULL,
 CONSTRAINT [PK_Shortcut] PRIMARY KEY CLUSTERED 
(
	[ShortcutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
PRINT N'Creating index [IX_Shortcut_Hotkey] on [dbo].[Shortcut]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Shortcut_Hotkey] ON [dbo].[Shortcut] ([Hotkey]) WHERE ([Shortcut].[Hotkey] IS NOT NULL)
GO

PRINT N'Creating index [IX_Shortcut_Barcode] on [dbo].[Shortcut]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Shortcut_Barcode] ON [dbo].[Shortcut] ([Barcode]) WHERE Barcode != ''
GO

ALTER TABLE [dbo].[Shortcut] ENABLE CHANGE_TRACKING
GO

CREATE TABLE dbo.Tmp_ShippingProfile
	(
	ShippingProfileID bigint NOT NULL IDENTITY (1053, 1000),
	RowVersion timestamp NOT NULL,
	Name nvarchar(50) NOT NULL,
	ShipmentType int NULL,
	ShipmentTypePrimary bit NOT NULL,
	OriginID bigint NULL,
	Insurance bit NULL,
	InsuranceInitialValueSource int NULL,
	InsuranceInitialValueAmount money NULL,
	ReturnShipment bit NULL,
	RequestedLabelFormat int NULL
	)  ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_ShippingProfile ON
GO
IF EXISTS(SELECT * FROM dbo.ShippingProfile)
	 EXEC('INSERT INTO dbo.Tmp_ShippingProfile (ShippingProfileID, Name, ShipmentType, ShipmentTypePrimary, OriginID, Insurance, InsuranceInitialValueSource, InsuranceInitialValueAmount, ReturnShipment, RequestedLabelFormat)
		SELECT ShippingProfileID, Name, ShipmentType, ShipmentTypePrimary, OriginID, Insurance, InsuranceInitialValueSource, InsuranceInitialValueAmount, ReturnShipment, RequestedLabelFormat FROM dbo.ShippingProfile WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ShippingProfile OFF
GO
ALTER TABLE dbo.iParcelProfile
	DROP CONSTRAINT FK_iParcelProfile_ShippingProfile
GO
ALTER TABLE dbo.OnTracProfile
	DROP CONSTRAINT FK_OnTracProfile_ShippingProfile
GO
ALTER TABLE dbo.OtherProfile
	DROP CONSTRAINT FK_OtherProfile_ShippingProfile
GO
ALTER TABLE dbo.PostalProfile
	DROP CONSTRAINT FK_PostalProfile_ShippingProfile
GO
ALTER TABLE dbo.UpsProfile
	DROP CONSTRAINT FK_UpsProfile_ShippingProfile
GO
ALTER TABLE dbo.PackageProfile
	DROP CONSTRAINT FK_PackageProfile_ShippingProfile
GO
ALTER TABLE dbo.BestRateProfile
	DROP CONSTRAINT FK_BestRateProfile_ShippingProfile
GO
ALTER TABLE dbo.FedExProfile
	DROP CONSTRAINT FK_FedExProfile_ShippingProfile
GO
ALTER TABLE dbo.AmazonProfile
	DROP CONSTRAINT FK_AmazonProfile_ShippingProfile
GO
ALTER TABLE dbo.DhlExpressProfile
	DROP CONSTRAINT FK_DhlExpressProfile_ShippingProfile
GO
ALTER TABLE dbo.AsendiaProfile
	DROP CONSTRAINT FK_AsendiaProfile_ShippingProfile
GO
DROP TABLE dbo.ShippingProfile
GO
EXECUTE sp_rename N'dbo.Tmp_ShippingProfile', N'ShippingProfile', 'OBJECT' 
GO
ALTER TABLE dbo.ShippingProfile ADD CONSTRAINT
	PK_ShippingProfile PRIMARY KEY CLUSTERED 
	(
	ShippingProfileID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ShippingProfile ENABLE CHANGE_TRACKING
GO
ALTER TABLE dbo.AsendiaProfile ADD CONSTRAINT
	FK_AsendiaProfile_ShippingProfile FOREIGN KEY
	(
	ShippingProfileID
	) REFERENCES dbo.ShippingProfile
	(
	ShippingProfileID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
GO
PRINT N'Adding foreign keys to [dbo].[PostalProfile]'
GO
ALTER TABLE [dbo].[PostalProfile] ADD CONSTRAINT [FK_PostalProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [FK_AmazonProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[BestRateProfile]'
GO
ALTER TABLE [dbo].[BestRateProfile] ADD CONSTRAINT [FK_BestRateProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[DhlExpressProfile]'
GO
ALTER TABLE [dbo].[DhlExpressProfile] ADD CONSTRAINT [FK_DhlExpressProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[FedExProfile]'
GO
ALTER TABLE [dbo].[FedExProfile] ADD CONSTRAINT [FK_FedExProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OnTracProfile]'
GO
ALTER TABLE [dbo].[OnTracProfile] ADD CONSTRAINT [FK_OnTracProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[OtherProfile]'
GO
ALTER TABLE [dbo].[OtherProfile] ADD CONSTRAINT [FK_OtherProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [FK_PackageProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[UpsProfile]'
GO
ALTER TABLE [dbo].[UpsProfile] ADD CONSTRAINT [FK_UpsProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
PRINT N'Adding foreign keys to [dbo].[iParcelProfile]'
GO
ALTER TABLE [dbo].[iParcelProfile] ADD CONSTRAINT [FK_iParcelProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO