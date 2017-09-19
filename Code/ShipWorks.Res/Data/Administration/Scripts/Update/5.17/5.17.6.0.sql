
PRINT N'Dropping index [IX_DownloadDetail_BigIntIndex] from [dbo].[DownloadDetail]'
GO
DROP INDEX [IX_DownloadDetail_BigIntIndex] ON [dbo].[DownloadDetail]
GO
PRINT N'Dropping index [IX_DownloadDetail_String] from [dbo].[DownloadDetail]'
GO
DROP INDEX [IX_DownloadDetail_String] ON [dbo].[DownloadDetail]
GO
PRINT N'Dropping index [IX_DownloadDetail_OrderNumber] from [dbo].[DownloadDetail]'
GO
DROP INDEX [IX_DownloadDetail_OrderNumber] ON [dbo].[DownloadDetail]
GO
PRINT N'Creating index [IX_DownloadDetail_BigIntIndex] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_BigIntIndex] ON [dbo].[DownloadDetail] ([ExtraBigIntData1], [ExtraBigIntData2], [ExtraBigIntData3]) INCLUDE ([DownloadID])
GO
PRINT N'Creating index [IX_DownloadDetail_String] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_String] ON [dbo].[DownloadDetail] ([ExtraStringData1]) INCLUDE ([DownloadID])
GO
PRINT N'Creating index [IX_DownloadDetail_OrderNumber] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderNumber] ON [dbo].[DownloadDetail] ([OrderNumber], [ExtraStringData1]) INCLUDE ([DownloadID])
GO


