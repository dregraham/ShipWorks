SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [GlobalPostAvailability] to [dbo].[UspsAccount]'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'GlobalPostAvailability' AND Object_ID = Object_ID(N'UspsAccount'))
BEGIN
   	ALTER TABLE [dbo].[UspsAccount]	ADD [GlobalPostAvailability] [int] NOT NULL	CONSTRAINT [DF_GP_Availability] DEFAULT 0
	ALTER TABLE [dbo].[UspsAccount]	DROP CONSTRAINT [DF_GP_Availability]
END
