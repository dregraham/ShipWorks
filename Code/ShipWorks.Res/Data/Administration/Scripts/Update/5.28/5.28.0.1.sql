SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[iParcelPackage]'
GO
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_iParcelPackage_iParcelShipment]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[iParcelPackage]', 'U'))
	ALTER TABLE [dbo].[iParcelPackage] DROP CONSTRAINT [FK_iParcelPackage_iParcelShipment]
GO
PRINT N'Dropping constraints from [dbo].[iParcelPackage]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_iParcelPackage]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[iParcelPackage]', 'U'))
	ALTER TABLE [dbo].[iParcelPackage] DROP CONSTRAINT [PK_iParcelPackage]
GO
PRINT N'Dropping index [IX_SWDefault_iParcelPackage_ShipmentID] from [dbo].[iParcelPackage]'
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SWDefault_iParcelPackage_ShipmentID' AND object_id = OBJECT_ID(N'[dbo].[iParcelPackage]'))
	DROP INDEX [IX_SWDefault_iParcelPackage_ShipmentID] ON [dbo].[iParcelPackage]
GO
PRINT N'Rebuilding [dbo].[iParcelPackage]'
GO
	CREATE TABLE [dbo].[RG_Recovery_1_iParcelPackage](
		[iParcelPackageID] [BIGINT] IDENTITY(1092,1000) NOT NULL,
		[ShipmentID] [BIGINT] NOT NULL,
		[Weight] [FLOAT] NOT NULL,
		[DimsProfileID] [BIGINT] NOT NULL,
		[DimsLength] [FLOAT] NOT NULL,
		[DimsWidth] [FLOAT] NOT NULL,
		[DimsHeight] [FLOAT] NOT NULL,
		[DimsAddWeight] [BIT] NOT NULL,
		[DimsWeight] [FLOAT] NOT NULL,
		[Insurance] [BIT] NOT NULL,
		[InsuranceValue] [MONEY] NOT NULL,
		[InsurancePennyOne] [BIT] NOT NULL,
		[DeclaredValue] [MONEY] NOT NULL,
		[TrackingNumber] [VARCHAR](50) NOT NULL,
		[ParcelNumber] [NVARCHAR](50) NOT NULL,
		[SkuAndQuantities] [NVARCHAR](500) NOT NULL,
	 CONSTRAINT [PK_iParcelPackageNew] PRIMARY KEY CLUSTERED 
	(
		[iParcelPackageID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
INSERT INTO [dbo].[RG_Recovery_1_iParcelPackage]([ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsAddWeight], [DimsWeight], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [ParcelNumber], [SkuAndQuantities]) --, [OldiParcelPackageID]) 
	SELECT  [ShipmentID], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsAddWeight], [DimsWeight], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [ParcelNumber], [SkuAndQuantities] /*, [iParcelPackageID]*/ FROM [dbo].[iParcelPackage]
GO
DROP TABLE [dbo].[iParcelPackage]
GO
IF (OBJECT_ID(N'[dbo].[RG_Recovery_1_iParcelPackage]', 'U') IS NOT NULL) AND (OBJECT_ID(N'[dbo].[iParcelPackage]', 'U') IS NULL)
	EXEC sp_rename N'[dbo].[RG_Recovery_1_iParcelPackage]', N'iParcelPackage', N'OBJECT'
GO
PRINT N'Adding foreign keys to [dbo].[iParcelPackage]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_iParcelPackage_iParcelShipment]', 'F') AND parent_object_id = OBJECT_ID(N'[dbo].[iParcelPackage]', 'U'))
	ALTER TABLE [dbo].[iParcelPackage] ADD CONSTRAINT [FK_iParcelPackage_iParcelShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[iParcelShipment] ([ShipmentID]) ON DELETE CASCADE
GO

PRINT N'Truncating table [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
TRUNCATE TABLE [dbo].[QuickFilterNodeUpdateCheckpoint]

PRINT N'Dropping constraints from [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_QuickFilterNodeUpdateCheckpoint]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]', 'U'))
	ALTER TABLE [dbo].[QuickFilterNodeUpdateCheckpoint] DROP CONSTRAINT [PK_QuickFilterNodeUpdateCheckpoint]
GO
PRINT N'Rebuilding [dbo].[QuickFilterNodeUpdateCheckpoint]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_QuickFilterNodeUpdateCheckpoint]
(
	[CheckpointID] [bigint] NOT NULL IDENTITY(1080, 1000),
	[MaxDirtyID] [bigint] NOT NULL,
	[DirtyCount] [int] NOT NULL,
	[State] [int] NOT NULL,
	[Duration] [int] NOT NULL
)
GO
DROP TABLE [dbo].[QuickFilterNodeUpdateCheckpoint]
GO
IF (OBJECT_ID(N'[dbo].[RG_Recovery_1_QuickFilterNodeUpdateCheckpoint]', 'U') IS NOT NULL) AND (OBJECT_ID(N'[dbo].[QuickFilterNodeUpdateCheckpoint]', 'U') IS NULL)
	EXEC sp_rename N'[dbo].[RG_Recovery_1_QuickFilterNodeUpdateCheckpoint]', N'QuickFilterNodeUpdateCheckpoint', N'OBJECT'
GO

