CREATE TABLE [dbo].[Download] (
    [DownloadID]    BIGINT         IDENTITY (1018, 1000) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    [StoreID]       BIGINT         NOT NULL,
    [ComputerID]    BIGINT         NOT NULL,
    [UserID]        BIGINT         NOT NULL,
    [InitiatedBy]   INT            NOT NULL,
    [Started]       DATETIME       NOT NULL,
    [Ended]         DATETIME       NULL,
    [Duration]      AS             (datediff(second,[Started],[Ended])) PERSISTED,
    [QuantityTotal] INT            NULL,
    [QuantityNew]   INT            NULL,
    [Result]        INT            NOT NULL,
    [ErrorMessage]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Download] PRIMARY KEY CLUSTERED ([DownloadID] ASC),
    CONSTRAINT [FK_Download_Computer] FOREIGN KEY ([ComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
    CONSTRAINT [FK_Download_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Download_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
ALTER TABLE [dbo].[Download] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_DownloadLog_StoreID_Ended]
    ON [dbo].[Download]([StoreID] ASC, [Ended] ASC);

