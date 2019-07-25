PRINT N'Altering [dbo].[Order]'
GO
IF COL_LENGTH(N'[dbo].[Order]', N'Verified') IS NULL
    ALTER TABLE [dbo].[Order] ADD [Verified] [bit] NOT NULL CONSTRAINT [DF_Order_Verified] DEFAULT (0)
GO
IF COL_LENGTH(N'[dbo].[Order]', N'VerifiedBy') IS NULL
    ALTER TABLE [dbo].[Order] ADD [VerifiedBy] [bigint] NULL
GO
IF COL_LENGTH(N'[dbo].[Order]', N'VerifiedDate') IS NULL
    ALTER TABLE [dbo].[Order] ADD [VerifiedDate] [datetime] NULL
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]')
                                 AND name = N'IX_SWDefault_Order_Verified')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_SWDefault_Order_Verified] ON [dbo].[Order] ([Verified])
END
GO

IF NOT EXISTS (SELECT NULL FROM SYS.EXTENDED_PROPERTIES WHERE [major_id] = OBJECT_ID('Order') AND [name] = N'AuditName' AND [minor_id] = (SELECT [column_id] FROM SYS.COLUMNS WHERE [name] = 'Verified' AND [object_id] = OBJECT_ID('Order')))
BEGIN
EXEC sp_addextendedproperty N'AuditName', N'Verified', 'SCHEMA', N'dbo', 'TABLE', N'Order', 'COLUMN', N'Verified'
END
GO
