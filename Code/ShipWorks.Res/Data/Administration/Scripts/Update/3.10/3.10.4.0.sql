SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[FilterSequence]'
GO
ALTER TABLE [dbo].[FilterSequence] DROP CONSTRAINT [FK_FilterSequence_Filter]
ALTER TABLE [dbo].[FilterSequence] DROP CONSTRAINT [FK_FilterSequence_Folder]
GO
PRINT N'Dropping constraints from [dbo].[Filter]'
GO
ALTER TABLE [dbo].[Filter] DROP CONSTRAINT [PK_Filter]
GO
PRINT N'Rebuilding [dbo].[Filter]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Filter]
(
[FilterID] [bigint] NOT NULL IDENTITY(1010, 1000),
[RowVersion] [timestamp] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FilterTarget] [int] NOT NULL,
[IsFolder] [bit] NOT NULL,
[Definition] [xml] NULL,
[State] [tinyint] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Filter] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_Filter]([FilterID], [Name], [FilterTarget], [IsFolder], [Definition], [State]) SELECT [FilterID], [Name], [FilterTarget], [IsFolder], [Definition], 1 FROM [dbo].[Filter]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Filter] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Filter]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Filter]', RESEED, @idVal)
GO
DROP TABLE [dbo].[Filter]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Filter]', N'Filter'
GO
PRINT N'Creating primary key [PK_Filter] on [dbo].[Filter]'
GO
ALTER TABLE [dbo].[Filter] ADD CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED  ([FilterID])
GO
PRINT N'Creating index [IX_Filter_State] on [dbo].[Filter]'
GO
CREATE NONCLUSTERED INDEX [IX_Filter_State] ON [dbo].[Filter] ([State])
GO
PRINT N'Adding foreign keys to [dbo].[FilterSequence]'
GO
ALTER TABLE [dbo].[FilterSequence] ADD CONSTRAINT [FK_FilterSequence_Filter] FOREIGN KEY ([FilterID]) REFERENCES [dbo].[Filter] ([FilterID])
ALTER TABLE [dbo].[FilterSequence] ADD CONSTRAINT [FK_FilterSequence_Folder] FOREIGN KEY ([ParentFilterID]) REFERENCES [dbo].[Filter] ([FilterID])
GO