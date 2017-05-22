SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [CustomsDescription] to [dbo].[UpsProfile]'
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'CustomsDescription' AND Object_ID = Object_ID(N'UpsProfile'))
BEGIN
   	ALTER TABLE [dbo].[UpsProfile] ADD [CustomsDescription] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO
PRINT N'Updating primary UPS Profile'
GO
UPDATE [dbo].UpsProfile
SET 
	[CustomsDescription] = 'Goods'
GO
