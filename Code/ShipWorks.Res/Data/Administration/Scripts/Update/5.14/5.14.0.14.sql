SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

PRINT N'Creating index [IX_UpsPackageRate_WeightInPounds_Zone] on [dbo].[UpsPackageRate]'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME= N'IX_UpsPackageRate_WeightInPounds_Zone' AND Object_ID = Object_ID('[dbo].[UpsPackageRate]'))
CREATE NONCLUSTERED INDEX [IX_UpsPackageRate_WeightInPounds_Zone] ON [dbo].[UpsPackageRate] ([WeightInPounds],[Zone]) INCLUDE ([UpsPackageRateID], [UpsRateTableID], [Service], [Rate])
GO

PRINT N'Creating index [IX_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip] on [dbo].[UpsLocalRatingDeliveryAreaSurcharge]'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE NAME= N'IX_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip' AND Object_ID = Object_ID('[dbo].[UpsLocalRatingDeliveryAreaSurcharge]'))
CREATE NONCLUSTERED INDEX [IX_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip] ON [dbo].[UpsLocalRatingDeliveryAreaSurcharge] ([DestinationZip]) INCLUDE ([DeliveryAreaSurchargeID],	[ZoneFileID], [DeliveryAreaType])
GO
