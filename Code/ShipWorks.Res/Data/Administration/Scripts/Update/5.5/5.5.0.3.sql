
PRINT N'Creating index [IX_DownloadDetail_OrderID] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderID] ON [dbo].[DownloadDetail] ([OrderID]) INCLUDE ([DownloadID], [InitialDownload])
GO