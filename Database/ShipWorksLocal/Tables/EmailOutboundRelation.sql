CREATE TABLE [dbo].[EmailOutboundRelation] (
    [EmailOutboundRelationID] BIGINT IDENTITY (1046, 1000) NOT NULL,
    [EmailOutboundID]         BIGINT NOT NULL,
    [ObjectID]                BIGINT NOT NULL,
    [RelationType]            INT    NOT NULL,
    CONSTRAINT [PK_EmailOutboundObject] PRIMARY KEY CLUSTERED ([EmailOutboundRelationID] ASC),
    CONSTRAINT [FK_EmailOutboundObject_EmailOutbound] FOREIGN KEY ([EmailOutboundID]) REFERENCES [dbo].[EmailOutbound] ([EmailOutboundID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Email]
    ON [dbo].[EmailOutboundRelation]([EmailOutboundID] ASC, [RelationType] ASC)
    INCLUDE([ObjectID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Object]
    ON [dbo].[EmailOutboundRelation]([ObjectID] ASC, [RelationType] ASC)
    INCLUDE([EmailOutboundID]);


GO
CREATE TRIGGER [dbo].[FilterDirtyEmailOutboundRelation]
    ON [dbo].[EmailOutboundRelation]
    AFTER INSERT, DELETE, UPDATE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FilterDirtyEmailOutboundRelation]

