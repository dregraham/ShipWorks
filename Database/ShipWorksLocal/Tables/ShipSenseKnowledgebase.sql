CREATE TABLE [dbo].[ShipSenseKnowledgebase]
(
	[Hash] NVARCHAR(64) NOT NULL, 
    [Entry] VARBINARY(MAX) NOT NULL,
	CONSTRAINT [PK_ShipSenseKnowledgeBase] PRIMARY KEY CLUSTERED 
	(
		[Hash] ASC
	)
)
