SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

PRINT N'Creating [dbo].[UpsRateTable]'
GO
CREATE TABLE [dbo].[UpsRateTable](
	[UpsRateTableID] [bigint] NOT NULL,
	[UploadDate][DateTime2] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRateTable] on [dbo].[UpsRateTable]'
GO
ALTER TABLE [UpsRateTable] ADD CONSTRAINT [PK_UpsRateTable] PRIMARY KEY CLUSTERED ([UpsRateTableID])
GO

PRINT N'Creating [dbo].[UpsRate]'
GO
CREATE TABLE [dbo].[UpsRate](
	[UpsRateID] [bigint] NOT NULL,
	[UpsRateTableID][bigint] NOT NULL,
	[Zone][int] NOT NULL,
	[WeightInPounds][int] NOT NULL,
	[Service][int] NOT NULL,
	[Rate][Money] NOT NULL)
GO
PRINT N'Creating primary key [PK_UpsRate] on [dbo].[UpsRate]'
GO
ALTER TABLE [dbo].[UpsRate] ADD CONSTRAINT [PK_UpsRate] PRIMARY KEY CLUSTERED ([UpsRateID])
GO
PRINT N'Adding foreign keys to [dbo].[UpsRate]'
ALTER TABLE [dbo].[UpsRate] ADD CONSTRAINT [FK_UpsRate_UpsRateTable] FOREIGN KEY([UpsRateTableID]) REFERENCES [dbo].[UpsRateTable] ([UpsRateTableID])
ON DELETE CASCADE
GO

PRINT N'Creating [dbo].[UpsRateSurcharge]'
GO
CREATE TABLE [dbo].[UpsRateSurcharge](
	[UpsRateSurchargeID] [bigint] NOT NULL,
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
	ON DELETE CASCADE
END