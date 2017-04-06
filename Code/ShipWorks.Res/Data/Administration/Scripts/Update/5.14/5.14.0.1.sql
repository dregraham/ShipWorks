SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

PRINT N'Creating [dbo].[UpsRateTable]'
GO
CREATE TABLE [dbo].[UpsRateTable](
	[UpsRateTableID] [bigint] NOT NULL,
	[UpsAccountID][bigint] NOT NULL,
	[UploadDate][DateTime] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateTable] on [dbo].[UpsRateTable]'
GO
ALTER TABLE [UpsRateTable] ADD CONSTRAINT [PK_UpsRateTable] PRIMARY KEY CLUSTERED ([UpsRateTableID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRateTable]'
ALTER TABLE [dbo].[UpsRateTable] ADD CONSTRAINT [FK_UpsRateTable_UpsAccount] FOREIGN KEY ([UpsAccountID]) REFERENCES [dbo].[UpsAccount] ([UpsAccountID])
GO

PRINT N'Creating [dbo].[UpsLocalRates]'
GO
CREATE TABLE [dbo].[UpsLocalRates](
	[UpsLocalRatesID] [bigint] NOT NULL,
	[UpsRateTableID][bigint] NOT NULL,
	[Zone][int] NOT NULL,
	[Weight][int] NOT NULL,
	[Service][int] NOT NULL,
	[Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRates] on [dbo].[UpsLocalRates]'
GO
ALTER TABLE [dbo].[UpsLocalRates] ADD CONSTRAINT [PK_UpsLocalRates] PRIMARY KEY CLUSTERED ([UpsLocalRatesID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRates]'
ALTER TABLE [dbo].[UpsLocalRates] ADD CONSTRAINT [FK_UpsLocalRates_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
GO

PRINT N'Creating [dbo].[UpsLocalRateSurcharge]'
GO
CREATE TABLE [dbo].[UpsLocalRateSurcharge](
	[UpsLocalRateSurchargeID] [bigint] NOT NULL,
	[UpsRateTableID][bigint] NOT NULL,
	[SurchargeType][int] NOT NULL,
	[Value][float] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsLocalRateSurcharge] on [dbo].[UpsLocalRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRateSurcharge] ADD CONSTRAINT [PK_UpsLocalRateSurcharge] PRIMARY KEY CLUSTERED ([UpsLocalRateSurchargeID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsLocalRateSurcharge]'
GO
ALTER TABLE [dbo].[UpsLocalRateSurcharge] ADD CONSTRAINT [FK_UpsLocalRateSurcharge_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
GO

PRINT N'Adding [UpsRateTableID] to [dbo].[UpsAccount]'
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'UpsRateTableID' AND Object_ID = Object_ID(N'UpsAccount'))
BEGIN
	ALTER TABLE UpsAccount
	Add [UpsRateTableID][bigint] NULL
END