PRINT N'Altering Filter'
GO

IF COL_LENGTH('Filter', 'IsOnDemand') IS NULL
BEGIN
	ALTER TABLE [dbo].[Filter] ADD [IsSavedSearch] [bit] NOT NULL CONSTRAINT [DF_Filter_IsSavedSearch] DEFAULT ((0))
END
GO
