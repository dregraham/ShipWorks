CREATE TABLE [dbo].[DownloadDetail] (
    [DownloadedDetailID] BIGINT        IDENTITY (1019, 1000) NOT NULL,
    [DownloadID]         BIGINT        NOT NULL,
    [OrderID]            BIGINT        NOT NULL,
    [InitialDownload]    BIT           NOT NULL,
    [OrderNumber]        BIGINT        NULL,
    [ExtraBigIntData1]   BIGINT        NULL,
    [ExtraBigIntData2]   BIGINT        NULL,
    [ExtraBigIntData3]   BIGINT        NULL,
    [ExtraStringData1]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_DownloadDetail] PRIMARY KEY CLUSTERED ([DownloadedDetailID] ASC),
    CONSTRAINT [FK_DownloadDetail_Download] FOREIGN KEY ([DownloadID]) REFERENCES [dbo].[Download] ([DownloadID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderNumber]
    ON [dbo].[DownloadDetail]([OrderNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_BigIntIndex]
    ON [dbo].[DownloadDetail]([ExtraBigIntData1] ASC, [ExtraBigIntData2] ASC, [ExtraBigIntData3] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_String]
    ON [dbo].[DownloadDetail]([ExtraStringData1] ASC);


GO
CREATE TRIGGER [dbo].[FilterDirtyDownloadDetail]
    ON [dbo].[DownloadDetail]
    AFTER INSERT
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyDownloadDetail]

