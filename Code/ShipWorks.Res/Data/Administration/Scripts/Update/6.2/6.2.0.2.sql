PRINT N'Altering Filter'
GO

IF COL_LENGTH('Filter', 'IsSavedSearch') IS NULL
BEGIN
	ALTER TABLE [dbo].[Filter] ADD [IsSavedSearch] [bit] NOT NULL CONSTRAINT [DF_Filter_IsSavedSearch] DEFAULT ((0))
END
GO

PRINT 'Altering FilterInfo view.'
GO
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[FilterInfo]'))
	DROP VIEW [dbo].[FilterInfo]
GO
CREATE VIEW FilterInfo WITH ENCRYPTION AS
	SELECT f.Name, f.FilterID, f.IsFolder, f.State, f.IsSavedSearch, f.[Definition], n.FilterNodeID, c.*
		FROM FilterNode n INNER JOIN FilterSequence s ON n.FilterSequenceID = s.FilterSequenceID 
						  INNER JOIN Filter f ON s.FilterID = f.FilterID 
						  INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
GO
