CREATE TABLE [dbo].[ShipSenseKnowledgebase] (
    [Hash]  NVARCHAR (64)   COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
    [Entry] VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_ShipSenseKnowledgebase] PRIMARY KEY CLUSTERED ([Hash] ASC)
);

