GO
PRINT N'Altering [dbo].[DhlExpressShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[DhlExpressShipment] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[DhlExpressProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressProfile]', N'CustomsRecipientTin') IS NULL
ALTER TABLE [dbo].[DhlExpressProfile] ADD [CustomsRecipientTin] [nvarchar] (25) NULL
GO

PRINT N'Altering [dbo].[DhlExpressShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'CustomsTaxIdType') IS NULL
ALTER TABLE [dbo].[DhlExpressShipment] ADD [CustomsTaxIdType] [int] NULL
GO

PRINT N'Altering [dbo].[DhlExpressProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressProfile]', N'CustomsTaxIdType') IS NULL
ALTER TABLE [dbo].[DhlExpressProfile] ADD [CustomsTaxIdType] [int] NULL
GO

PRINT N'Altering [dbo].[DhlExpressProfile]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressProfile]', N'CustomsTinIssuingAuthority') IS NULL
ALTER TABLE [dbo].[DhlExpressProfile] ADD
	[CustomsTinIssuingAuthority] [nvarchar] (2) NULL
GO

PRINT N'Altering [dbo].[DhlExpressShipment]'
GO
IF COL_LENGTH(N'[dbo].[DhlExpressShipment]', N'CustomsTinIssuingAuthority') IS NULL
ALTER TABLE [dbo].[DhlExpressShipment] ADD
	[CustomsTinIssuingAuthority] [nvarchar] (2) NULL
GO
