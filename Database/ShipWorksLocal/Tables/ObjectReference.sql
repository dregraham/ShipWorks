CREATE TABLE [dbo].[ObjectReference] (
    [ObjectReferenceID] BIGINT         IDENTITY (1030, 1000) NOT NULL,
    [ConsumerID]        BIGINT         NOT NULL,
    [ReferenceKey]      VARCHAR (250)  CONSTRAINT [DF_ObjectReference_ReferenceKey] DEFAULT ('') NOT NULL,
    [ObjectID]          BIGINT         NOT NULL,
    [Reason]            NVARCHAR (250) NULL,
    CONSTRAINT [PK_ObjectReference] PRIMARY KEY CLUSTERED ([ObjectReferenceID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectReference]
    ON [dbo].[ObjectReference]([ConsumerID] ASC, [ReferenceKey] ASC);

