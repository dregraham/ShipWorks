SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

PRINT N'Creating [dbo].[UpsRateTable]'
GO
CREATE TABLE [dbo].[UpsRateTable](
	[UpsRateTableID] [bigint] NOT NULL IDENTITY(1, 1),
	[UploadDate][DateTime2] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateTable] on [dbo].[UpsRateTable]'
GO
ALTER TABLE [UpsRateTable] ADD CONSTRAINT [PK_UpsRateTable] PRIMARY KEY CLUSTERED ([UpsRateTableID])
GO
PRINT N'Creating [dbo].[UpsPackageRate]'
GO
CREATE TABLE [dbo].[UpsPackageRate](
	[UpsPackageRateID] [bigint] NOT NULL IDENTITY(1, 1),
	[UpsRateTableID][bigint] NOT NULL,
	[Zone][int] NOT NULL,
	[WeightInPounds][int] NOT NULL,
	[Service][int] NOT NULL,
	[Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsPackageRate] on [dbo].[UpsPackageRate]'
GO
ALTER TABLE [dbo].[UpsPackageRate] ADD CONSTRAINT [PK_UpsPackageRate] PRIMARY KEY CLUSTERED ([UpsPackageRateID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsPackageRate]'
ALTER TABLE [dbo].[UpsPackageRate] ADD CONSTRAINT [FK_UpsPackageRate_UpsPackageRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsLetterRate]'
GO
CREATE TABLE [dbo].[UpsLetterRate](
	[UpsLetterRateID] [bigint] NOT NULL IDENTITY(1, 1),
	[UpsRateTableID][bigint] NOT NULL,
	[Zone][int] NOT NULL,
	[Service][int] NOT NULL,
	[Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLetterRate] on [dbo].[UpsLetterRate]'
GO
ALTER TABLE [dbo].[UpsLetterRate] ADD CONSTRAINT [PK_UpsLetterRate] PRIMARY KEY CLUSTERED ([UpsLetterRateID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLetterRate]'
ALTER TABLE [dbo].[UpsLetterRate] ADD CONSTRAINT [FK_UpsLetterRate_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsPricePerPound]'
GO
CREATE TABLE [dbo].[UpsPricePerPound](
	[UpsPricePerPoundID] [bigint] NOT NULL IDENTITY(1, 1),
	[UpsRateTableID][bigint] NOT NULL,
	[Zone][int] NOT NULL,
	[Service][int] NOT NULL,
	[Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsPricePerPound] on [dbo].[UpsPricePerPound]'
GO
ALTER TABLE [dbo].[UpsPricePerPound] ADD CONSTRAINT [PK_UpsPricePerPound] PRIMARY KEY CLUSTERED ([UpsPricePerPoundID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRate]'
ALTER TABLE [dbo].[UpsPricePerPound] ADD CONSTRAINT [FK_UpsPricePerPound_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsRateSurcharge]'
GO
CREATE TABLE [dbo].[UpsRateSurcharge](
	[UpsRateSurchargeID] [bigint] NOT NULL IDENTITY(1, 1),
	[UpsRateTableID][bigint] NOT NULL,
	[SurchargeType][int] NOT NULL,
	[Amount][float] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateSurcharge] on [dbo].[UpsRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsRateSurcharge] ADD CONSTRAINT [PK_UpsRateSurcharge] PRIMARY KEY CLUSTERED ([UpsRateSurchargeID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsRateSurcharge] ADD CONSTRAINT [FK_UpsRateSurcharge_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO


PRINT N'Adding [UpsRateTableID] to [dbo].[UpsAccount]'
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'UpsRateTableID' AND Object_ID = Object_ID(N'UpsAccount'))
BEGIN
	ALTER TABLE [dbo].[UpsAccount]
	Add [UpsRateTableID][bigint] NULL
	ALTER TABLE [dbo].[UpsAccount] ADD CONSTRAINT [FK_UpsAccount_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
END
