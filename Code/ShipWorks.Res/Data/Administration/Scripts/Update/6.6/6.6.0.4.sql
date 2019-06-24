PRINT N'Altering [dbo].[Order]'
GO
IF COL_LENGTH(N'[dbo].[Order]', N'HubSequence') IS NULL
    ALTER TABLE [dbo].[Order]
        ADD [HubOrderID] [uniqueidentifier] NULL,
            [HubSequence] [bigint] NULL

GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]')
                                 AND name = N'[IX_SWDefault_Order_HubSequence]')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_HubSequence] ON [dbo].[Order] ([HubSequence])
END
GO