
PRINT N'Creating index [IX_DownloadDetail_OrderID] on [dbo].[DownloadDetail]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DownloadDetail_OrderID' AND object_id = OBJECT_ID(N'[dbo].[DownloadDetail]'))
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderID] ON [dbo].[DownloadDetail] ([OrderID]) INCLUDE ([DownloadID], [InitialDownload])
GO