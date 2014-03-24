CREATE TABLE [dbo].[ObjectLabel] (
    [ObjectID]   BIGINT         NOT NULL,
    [RowVersion] ROWVERSION     NOT NULL,
    [ObjectType] INT            NOT NULL,
    [ParentID]   BIGINT         NULL,
    [Label]      NVARCHAR (100) NOT NULL,
    [IsDeleted]  BIT            NOT NULL,
    CONSTRAINT [PK_ObjectLabel] PRIMARY KEY CLUSTERED ([ObjectID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectLabel]
    ON [dbo].[ObjectLabel]([ObjectType] ASC, [IsDeleted] ASC);

