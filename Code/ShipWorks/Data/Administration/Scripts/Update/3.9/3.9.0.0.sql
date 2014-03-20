SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ShipSenseKnowledgebase]'
GO
CREATE TABLE [dbo].[ShipSenseKnowledgebase]
(
[Hash] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Entry] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_ShipSenseKnowledgebase] on [dbo].[ShipSenseKnowledgebase]'
GO
ALTER TABLE [dbo].[ShipSenseKnowledgebase] ADD CONSTRAINT [PK_ShipSenseKnowledgebase] PRIMARY KEY CLUSTERED  ([Hash])
GO