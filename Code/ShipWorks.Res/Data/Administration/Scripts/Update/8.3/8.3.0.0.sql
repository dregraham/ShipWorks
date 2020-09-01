PRINT N'Altering [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] ADD
[HubVersion] [int] NULL,
[HubCarrierId] [uniqueidentifier] NULL,
[HubSequence] [bigint] NULL
GO