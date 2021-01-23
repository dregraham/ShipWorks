PRINT N'Altering [dbo].[Order]'
GO
IF COL_LENGTH(N'[dbo].[Order]', N'DeliverByDate') IS NULL
ALTER TABLE [dbo].[Order] ADD
    [DeliverByDate] [datetime] NULL
GO