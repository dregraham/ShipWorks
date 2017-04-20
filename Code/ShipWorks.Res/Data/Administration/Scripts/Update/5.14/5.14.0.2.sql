SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

PRINT N'Creating [dbo].[UpsLocalRatingZoneFile]'
GO
CREATE TABLE [dbo].[UpsLocalRatingZoneFile](
	[ZoneFileID] [bigint] NOT NULL IDENTITY(1, 1),
	[UploadDate] [DateTime2] NOT NULL,
	[FileContent] [varbinary](max) NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingZoneFile] on [dbo].[UpsLocalRatingZoneFile]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZoneFile] ADD CONSTRAINT [PK_UpsLocalRatingZoneFile] PRIMARY KEY CLUSTERED ([ZoneFileID])
GO

PRINT N'Creating [dbo].[UpsLocalRatingZone]'
GO
CREATE TABLE [dbo].[UpsLocalRatingZone](
	[ZoneID] [bigint] NOT NULL IDENTITY(1, 1),
	[ZoneFileID] [bigint] NOT NULL,
	[OriginZipFloor] [int] NOT NULL,
	[OriginZipCeiling] [int] NOT NULL,
	[DestinationZipFloor] [int] NOT NULL,
	[DestinationZipCeiling] [int] NOT NULL,
	[Service] [int] NOT NULL,
	[Zone] [varchar](3) NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingZone] on [dbo].[UpsLocalRatingZone]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZone] ADD CONSTRAINT [PK_UpsLocalRatingZone] PRIMARY KEY CLUSTERED ([ZoneID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRatingZone]'
GO
ALTER TABLE [dbo].[UpsLocalRatingZone] ADD CONSTRAINT [FK_UpsLocalRatingZone_UpsLocalRatingZoneFile] FOREIGN KEY([ZoneFileID]) REFERENCES [dbo].[UpsLocalRatingZoneFile] ([ZoneFileID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
CREATE TABLE [dbo].UpsLocalRatingDeliveryAreaSurcharge(
	[DeliveryAreaSurchargeID] [bigint] NOT NULL IDENTITY(1, 1),
	[ZoneFileID] [bigint] NOT NULL,
	[DestinationZip] [int] NOT NULL,
	[DeliveryAreaType] [int] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRatingDeliveryAreaSurcharge] on [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ADD CONSTRAINT [PK_UpsLocalRatingDeliveryAreaSurcharge] PRIMARY KEY CLUSTERED ([DeliveryAreaSurchargeID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ADD CONSTRAINT [FK_UpsLocalRatingDeliveryAreaSurcharge_UpsLocalRatingZoneFile] FOREIGN KEY([ZoneFileID]) REFERENCES [dbo].[UpsLocalRatingZoneFile] ([ZoneFileID])
ON DELETE CASCADE
GO