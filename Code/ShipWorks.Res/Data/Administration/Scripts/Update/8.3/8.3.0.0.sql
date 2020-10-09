PRINT N'Altering [dbo].[UspsAccount]'
GO
IF COL_LENGTH(N' [dbo].[UspsAccount]', N'HubVersion' ) IS NULL
ALTER TABLE [dbo].[UspsAccount] ADD [HubVersion] [int] NULL
IF COL_LENGTH(N' [dbo].[UspsAccount]', N'HubCarrierId' ) IS NULL
ALTER TABLE [dbo].[UspsAccount] ADD [HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
IF COL_LENGTH(N' [dbo].[UpsAccount]', N'HubVersion' ) IS NULL
ALTER TABLE [dbo].[UpsAccount] ADD [HubVersion] [int] NULL
IF COL_LENGTH(N' [dbo].[UpsAccount]', N'HubCarrierId' ) IS NULL
ALTER TABLE [dbo].[UpsAccount] ADD [HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
IF COL_LENGTH(N' [dbo].[FedExAccount]', N'HubVersion' ) IS NULL
ALTER TABLE [dbo].[FedExAccount] ADD [HubVersion] [int] NULL
IF COL_LENGTH(N' [dbo].[FedExAccount]', N'HubCarrierId' ) IS NULL
ALTER TABLE [dbo].[FedExAccount] ADD [HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
IF COL_LENGTH(N' [dbo].[EndiciaAccount]', N'HubVersion') IS NULL
ALTER TABLE [dbo].[EndiciaAccount] ADD [HubVersion] [int] NULL
IF COL_LENGTH(N' [dbo].[EndiciaAccount]', N'HubCarrierId') IS NULL
ALTER TABLE [dbo].[EndiciaAccount] ADD [HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[OnTracAccount]'
GO
IF COL_LENGTH(N' [dbo].[OnTracAccount]', N'HubVersion') IS NULL
ALTER TABLE [dbo].OnTracAccount ADD [HubVersion] [int] NULL
IF COL_LENGTH(N' [dbo].[OnTracAccount]', N'HubCarrierId') IS NULL
ALTER TABLE [dbo].[OnTracAccount] ADD [HubCarrierId] [uniqueidentifier] NULL
GO