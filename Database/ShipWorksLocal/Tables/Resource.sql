CREATE TABLE [dbo].[Resource] (
    [ResourceID] BIGINT          IDENTITY (1026, 1000) NOT NULL,
    [Data]       VARBINARY (MAX) NOT NULL,
    [Checksum]   BINARY (32)     NOT NULL,
    [Compressed] BIT             NOT NULL,
    [Filename]   NVARCHAR (30)   NOT NULL,
    CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED ([ResourceID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Checksum]
    ON [dbo].[Resource]([Checksum] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Filename]
    ON [dbo].[Resource]([Filename] ASC);

