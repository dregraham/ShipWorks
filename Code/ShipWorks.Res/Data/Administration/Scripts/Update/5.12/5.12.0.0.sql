SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Adding [ApiKey] to [dbo].[ShopifyStore]'
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'ApiKey' AND Object_ID = Object_ID(N'ShopifyStore'))
BEGIN
   	ALTER TABLE [dbo].[ShopifyStore]	ADD [ApiKey] [nvarchar](100) NOT NULL CONSTRAINT [DF_ApiKey] DEFAULT ''
	ALTER TABLE [dbo].[ShopifyStore]	DROP CONSTRAINT [DF_ApiKey]
END
GO
PRINT N'Adding [Password] to [dbo].[ShopifyStore]'
GO
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name= N'Password' AND Object_ID = Object_ID(N'ShopifyStore'))
BEGIN
   	ALTER TABLE [dbo].[ShopifyStore]	ADD [Password] [nvarchar](100) NOT NULL CONSTRAINT [DF_Password] DEFAULT ''
	ALTER TABLE [dbo].[ShopifyStore]	DROP CONSTRAINT [DF_Password]
END
GO