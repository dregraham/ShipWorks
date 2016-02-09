PRINT N'Altering [dbo].[UspsAccount]'
GO
ALTER TABLE [dbo].[UspsAccount] ADD
[PendingInitialAccount] [bit] NOT NULL DEFAULT ((0))
GO