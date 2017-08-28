PRINT N'Creating [dbo].[JetOrderSearch]'
GO
CREATE TABLE [dbo].[JetOrderSearch]
(
[JetOrderSearchID] [bigint] NOT NULL IDENTITY(1, 1),
[OrderID] [bigint] NOT NULL,
[MerchantOrderID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OriginalOrderID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_JetOrderSearch] on [dbo].[JetOrderSearch]'
GO
ALTER TABLE [dbo].[JetOrderSearch] ADD CONSTRAINT [PK_JetOrderSearch] PRIMARY KEY CLUSTERED  ([JetOrderSearchID])
GO
PRINT N'Creating index [IX_JetOrderSearch_JetOrderID] on [dbo].[JetOrderSearch]'
GO
CREATE NONCLUSTERED INDEX [IX_JetOrderSearch_JetOrderID] ON [dbo].[JetOrderSearch] ([MerchantOrderID]) INCLUDE ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[JetOrderSearch]'
GO
ALTER TABLE [dbo].[JetOrderSearch] ADD CONSTRAINT [FK_JetOrderSearch_JetOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[JetOrder] ([OrderID]) ON DELETE CASCADE
GO
