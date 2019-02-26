PRINT N'Altering Filter'
GO

IF COL_LENGTH('Filter', 'IsOnDemand') IS NULL
BEGIN
	ALTER TABLE [dbo].[Filter] ADD [IsOnDemand] [bit] NOT NULL CONSTRAINT [DF_Filter_IsOnDemand] DEFAULT ((0))
END
GO
