PRINT N'Altering AuditChangeDetail IX_SWDefault_AuditChangeDetail_AuditChangeID to be unique'
GO
IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('[dbo].[AuditChangeDetail]') AND NAME ='IX_SWDefault_AuditChangeDetail_AuditChangeID')
    DROP INDEX IX_SWDefault_AuditChangeDetail_AuditChangeID ON [dbo].[AuditChangeDetail];
GO
CREATE UNIQUE INDEX [IX_SWDefault_AuditChangeDetail_AuditChangeID] ON [dbo].[AuditChangeDetail] ([AuditChangeID], [AuditChangeDetailID] ) 
	INCLUDE ( [AuditID])
GO