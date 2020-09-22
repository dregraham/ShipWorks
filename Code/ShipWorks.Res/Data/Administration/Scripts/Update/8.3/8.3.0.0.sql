PRINT N'Altering [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] ADD
[HubVersion] [int] NULL,
[HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
ALTER TABLE [dbo].[UpsAccount] ADD
[HubVersion] [int] NULL,
[HubCarrierId] [uniqueidentifier] NULL
GO
PRINT N'Altering [dbo].[FedExAccount]'
GO
ALTER TABLE [dbo].[FedExAccount] ADD
[HubVersion] [int] NULL,
[HubCarrierId] [uniqueidentifier] NULL
GO
