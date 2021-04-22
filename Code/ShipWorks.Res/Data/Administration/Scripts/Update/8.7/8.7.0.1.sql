PRINT N'Adding [IX_SWDefault_AuditChange_ObjectID]'
GO
CREATE NONCLUSTERED INDEX [IX_SWDefault_AuditChange_ObjectID]
ON [dbo].[AuditChange] ([ObjectID])
GO