SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Altering [dbo].[BestRateShipment]'
GO
IF COL_LENGTH(N'[dbo].[BestRateShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[BestRateShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_BestRateShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[AmazonShipment]'
GO
IF COL_LENGTH(N'[dbo].[AmazonShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[AmazonShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_AmazonShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[PostalShipment]'
GO
IF COL_LENGTH(N'[dbo].[PostalShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[PostalShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_PostalShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[UspsShipment]'
GO
IF COL_LENGTH(N'[dbo].[UspsShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[UspsShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_UspsShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[WorldShipPackage]'
GO
IF COL_LENGTH(N'[dbo].[WorldShipPackage]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[WorldShipPackage] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_WorldShipPackage_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[EndiciaShipment]'
GO
IF COL_LENGTH(N'[dbo].[EndiciaShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[EndiciaShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_EndiciaShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[OtherShipment]'
GO
IF COL_LENGTH(N'[dbo].[OtherShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[OtherShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_OtherShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[OnTracShipment]'
GO
IF COL_LENGTH(N'[dbo].[OnTracShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[OnTracShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_OnTracShipment_Insurance] DEFAULT ((0))
GO
PRINT N'Altering [dbo].[AsendiaShipment]'
GO
IF COL_LENGTH(N'[dbo].[AsendiaShipment]', N'Insurance') IS NULL
	ALTER TABLE [dbo].[AsendiaShipment] ADD [Insurance] [bit] NOT NULL CONSTRAINT [DF_AsendiaShipment_Insurance] DEFAULT ((0))
GO


PRINT N'Updating [dbo].[AmazonShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM AmazonShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[BestRateShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM BestRateShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[EndiciaShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM EndiciaShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[OtherShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM OtherShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[PostalShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM PostalShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[UspsShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM UspsShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[WorldShipPackage].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM WorldShipPackage a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;
GO

PRINT N'Updating [dbo].[OnTracShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM OnTracShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;

PRINT N'Updating [dbo].[AsendiaShipment].[Insurance]'
GO
UPDATE a 
	SET a.Insurance = s.Insurance
	FROM AsendiaShipment a
	JOIN Shipment s ON s.ShipmentID = a.ShipmentID;

PRINT N'Dropping constraints from [dbo].[AmazonShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[AmazonShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_AmazonShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[AmazonShipment] DROP CONSTRAINT [DF_AmazonShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[BestRateShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[BestRateShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_BestRateShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[BestRateShipment] DROP CONSTRAINT [DF_BestRateShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[EndiciaShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[EndiciaShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_EndiciaShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[EndiciaShipment] DROP CONSTRAINT [DF_EndiciaShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[OtherShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[OtherShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_OtherShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[OtherShipment] DROP CONSTRAINT [DF_OtherShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[PostalShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[PostalShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_PostalShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[PostalShipment] DROP CONSTRAINT [DF_PostalShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[UspsShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[UspsShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_UspsShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[UspsShipment] DROP CONSTRAINT [DF_UspsShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[WorldShipPackage]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[WorldShipPackage]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_WorldShipPackage_Insurance]', 'D'))
ALTER TABLE [dbo].[WorldShipPackage] DROP CONSTRAINT [DF_WorldShipPackage_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[OnTracShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[OnTracShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_OnTracShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[OnTracShipment] DROP CONSTRAINT [DF_OnTracShipment_Insurance]
GO
PRINT N'Dropping constraints from [dbo].[AsendiaShipment]'
GO
IF EXISTS (SELECT 1 FROM sys.columns WHERE name = N'Insurance' AND object_id = OBJECT_ID(N'[dbo].[AsendiaShipment]', 'U') AND default_object_id = OBJECT_ID(N'[dbo].[DF_AsendiaShipment_Insurance]', 'D'))
ALTER TABLE [dbo].[AsendiaShipment] DROP CONSTRAINT [DF_AsendiaShipment_Insurance]
GO



