PRINT N'Altering [dbo].[Filter]'
GO
IF COL_LENGTH(N'[dbo].[Filter]', N'HubFilterID') IS NULL
ALTER TABLE [dbo].[Filter] ADD
    [HubFilterID] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Filter_HubFilterID] DEFAULT ('')
GO