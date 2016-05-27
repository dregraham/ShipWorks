
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] DROP CONSTRAINT[FK_UpsPackage_UpsShipment]
GO
PRINT N'Dropping constraints from [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] DROP CONSTRAINT [PK_UpsPackage]
GO
PRINT N'Rebuilding [dbo].[UpsPackage]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_UpsPackage]
(
[UpsPackageID] [bigint] NOT NULL IDENTITY(1063, 1000),
[ShipmentID] [bigint] NOT NULL,
[PackagingType] [int] NOT NULL,
[Weight] [float] NOT NULL,
[DimsProfileID] [bigint] NOT NULL,
[DimsLength] [float] NOT NULL,
[DimsWidth] [float] NOT NULL,
[DimsHeight] [float] NOT NULL,
[DimsWeight] [float] NOT NULL,
[DimsAddWeight] [bit] NOT NULL,
[Insurance] [bit] NOT NULL,
[InsuranceValue] [money] NOT NULL,
[InsurancePennyOne] [bit] NOT NULL,
[DeclaredValue] [money] NOT NULL,
[TrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UspsTrackingNumber] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AdditionalHandlingEnabled] [bit] NOT NULL,
[VerbalConfirmationEnabled] [bit] NOT NULL,
[VerbalConfirmationName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VerbalConfirmationPhone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VerbalConfirmationPhoneExtension] [nvarchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DryIceEnabled] [bit] NOT NULL,
[DryIceRegulationSet] [int] NOT NULL,
[DryIceWeight] [float] NOT NULL,
[DryIceIsForMedicalUse] [bit] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsPackage] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_UpsPackage]([UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [UspsTrackingNumber], [AdditionalHandlingEnabled], [VerbalConfirmationName], [VerbalConfirmationPhone], [VerbalConfirmationPhoneExtension], [DryIceEnabled], [DryIceRegulationSet], [DryIceWeight], [DryIceIsForMedicalUse], [VerbalConfirmationEnabled]) SELECT [UpsPackageID], [ShipmentID], [PackagingType], [Weight], [DimsProfileID], [DimsLength], [DimsWidth], [DimsHeight], [DimsWeight], [DimsAddWeight], [Insurance], [InsuranceValue], [InsurancePennyOne], [DeclaredValue], [TrackingNumber], [UspsTrackingNumber], 0, '', '', '', 0, 0, 0, 0, 0 FROM [dbo].[UpsPackage]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_UpsPackage] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[UpsPackage]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_UpsPackage]', RESEED, @idVal)
GO
DROP TABLE [dbo].[UpsPackage]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_UpsPackage]', N'UpsPackage'
GO
PRINT N'Creating primary key [PK_UpsPackage] on [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [PK_UpsPackage] PRIMARY KEY CLUSTERED  ([UpsPackageID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackage]'
GO
ALTER TABLE [dbo].[UpsPackage] ADD CONSTRAINT [FK_UpsPackage_UpsShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[UpsShipment] ([ShipmentID]) ON DELETE CASCADE
GO

PRINT N'Altering [dbo].[WorldShipPackage]'
GO
ALTER TABLE [dbo].[WorldShipPackage] ADD
[AdditionalHandlingEnabled] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationContactName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationTelephone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceRegulationSet] [nvarchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceWeight] [float] NULL,
[DryIceMedicalPurpose] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceOption] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceWeightUnitOfMeasure] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'Altering [dbo].[UpsProfilePackage]'
GO
ALTER TABLE [dbo].[UpsProfilePackage] ADD
[AdditionalHandlingEnabled] [bit] NULL,
[VerbalConfirmationEnabled] [bit] NULL,
[VerbalConfirmationName] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationPhone] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerbalConfirmationPhoneExtension] [nvarchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DryIceEnabled] [bit] NULL,
[DryIceRegulationSet] [int] NULL,
[DryIceWeight] [float] NULL,
[DryIceIsForMedicalUse] [bit] NULL
GO

UPDATE ActionTask
SET InputSource = -1
WHERE TaskIdentifier IN ('PlaySound');
GO

UPDATE ActionQueueStep
SET InputSource = -1
WHERE TaskIdentifier IN ('PlaySound');
GO