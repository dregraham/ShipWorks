SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [SingleScanSettings] to [dbo].[UserSettings]'
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'SingleScanSettings' AND Object_ID = Object_ID(N'UserSettings'))
BEGIN
   	ALTER TABLE [dbo].[UserSettings]	ADD [SingleScanSettings] [int] NOT NULL	CONSTRAINT [DF_SingleScanSettings] DEFAULT 0
	ALTER TABLE [dbo].[UserSettings]	DROP CONSTRAINT [DF_SingleScanSettings]
END
