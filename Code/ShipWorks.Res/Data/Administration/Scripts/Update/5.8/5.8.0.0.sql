SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OnlineLastModified_StoreID_IsManual] from [dbo].[Order]'
GO
DROP INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order]
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [OnlineLastModified] [datetime2] NOT NULL
GO
PRINT N'Creating index [IX_OnlineLastModified_StoreID_IsManual] on [dbo].[Order]'
GO
CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
GO