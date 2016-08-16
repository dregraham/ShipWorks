SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_ActionQueue_ContextLock] from [dbo].[ActionQueue]'
GO
DROP INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue]
GO
PRINT N'Dropping index [IX_ActionQueue_Search] from [dbo].[ActionQueue]'
GO
DROP INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue]
GO
PRINT N'Dropping index [IX_Audit_TransactionID] from [dbo].[Audit]'
GO
DROP INDEX [IX_Audit_TransactionID] ON [dbo].[Audit]
GO
PRINT N'Dropping index [IX_AuditChange] from [dbo].[AuditChange]'
GO
DROP INDEX [IX_AuditChange] ON [dbo].[AuditChange]
GO
PRINT N'Dropping index [IX_AuditChangeDetail] from [dbo].[AuditChangeDetail]'
GO
DROP INDEX [IX_AuditChangeDetail] ON [dbo].[AuditChangeDetail]
GO
PRINT N'Dropping index [IX_EmailOutbound] from [dbo].[EmailOutbound]'
GO
DROP INDEX [IX_EmailOutbound] ON [dbo].[EmailOutbound]
GO
PRINT N'Dropping index [IX_EmailOutbound_Email] from [dbo].[EmailOutboundRelation]'
GO
DROP INDEX [IX_EmailOutbound_Email] ON [dbo].[EmailOutboundRelation]
GO
PRINT N'Dropping index [IX_EmailOutbound_Object] from [dbo].[EmailOutboundRelation]'
GO
DROP INDEX [IX_EmailOutbound_Object] ON [dbo].[EmailOutboundRelation]
GO
PRINT N'Dropping index [IX_FilterNodeCountDirty] from [dbo].[FilterNodeContentDirty]'
GO
DROP INDEX [IX_FilterNodeCountDirty] ON [dbo].[FilterNodeContentDirty]
GO
PRINT N'Dropping index [IX_ObjectLabel] from [dbo].[ObjectLabel]'
GO
DROP INDEX [IX_ObjectLabel] ON [dbo].[ObjectLabel]
GO
PRINT N'Dropping index [IX_ObjectReference] from [dbo].[ObjectReference]'
GO
DROP INDEX [IX_ObjectReference] ON [dbo].[ObjectReference]
GO
PRINT N'Creating index [IX_ActionQueue_ActionQueueType] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_ActionQueueType] ON [dbo].[ActionQueue] ([ActionQueueType] DESC) INCLUDE ([ComputerLimitedList], [Status])
GO
PRINT N'Creating index [IX_ActionQueue_ContextLock] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue] ([ContextLock]) INCLUDE ([Status])
GO
PRINT N'Creating index [IX_ActionQueue_Search] on [dbo].[ActionQueue]'
GO
CREATE NONCLUSTERED INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue] ([Status], [ActionQueueType], [ActionQueueID])
GO
PRINT N'Creating index [IX_Audit_Date] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_Audit_Date] ON [dbo].[Audit] ([Date])
GO
PRINT N'Creating index [IX_Audit_ObjectIDDate] on [dbo].[Audit]'
GO
CREATE NONCLUSTERED INDEX [IX_Audit_ObjectIDDate] ON [dbo].[Audit] ([ObjectID]) INCLUDE ([Date])
GO
PRINT N'Creating index [IX_Audit_TransactionID] on [dbo].[Audit]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Audit_TransactionID] ON [dbo].[Audit] ([TransactionID]) INCLUDE ([Action])
GO
PRINT N'Creating index [IX_AuditChange_AuditID] on [dbo].[AuditChange]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChange_AuditID] ON [dbo].[AuditChange] ([AuditID])
GO
PRINT N'Creating index [IX_AuditChangeDetail_AuditChangeID] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail_AuditChangeID] ON [dbo].[AuditChangeDetail] ([AuditChangeID])
GO
PRINT N'Creating index [IX_AuditChangeDetail_AuditID] on [dbo].[AuditChangeDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail_AuditID] ON [dbo].[AuditChangeDetail] ([AuditID])
GO
PRINT N'Creating index [IX_DownloadDetail_DownloadID] on [dbo].[DownloadDetail]'
GO
CREATE NONCLUSTERED INDEX [IX_DownloadDetail_DownloadID] ON [dbo].[DownloadDetail] ([DownloadID])
GO
PRINT N'Creating index [IX_EbayOrderItem_OrderID] on [dbo].[EbayOrderItem]'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrderItem_OrderID] ON [dbo].[EbayOrderItem] ([OrderID])
GO
PRINT N'Creating index [IX_EmailOutbound] on [dbo].[EmailOutbound]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound] ON [dbo].[EmailOutbound] ([SendStatus], [AccountID], [DontSendBefore], [SentDate], [ComposedDate]) INCLUDE ([Visibility])
GO
PRINT N'Creating index [IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID] ON [dbo].[EmailOutboundRelation] ([EmailOutboundID], [RelationType]) INCLUDE ([ObjectID])
GO
PRINT N'Creating index [IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID] ON [dbo].[EmailOutboundRelation] ([ObjectID], [RelationType]) INCLUDE ([EmailOutboundID])
GO
PRINT N'Creating index [IX_EmailOutbound_RelationTypeObject] on [dbo].[EmailOutboundRelation]'
GO
CREATE NONCLUSTERED INDEX [IX_EmailOutbound_RelationTypeObject] ON [dbo].[EmailOutboundRelation] ([RelationType], [ObjectID]) INCLUDE ([EmailOutboundID])
GO
PRINT N'Creating index [IX_FedExPackage_ShipmentID] on [dbo].[FedExPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_FedExPackage_ShipmentID] ON [dbo].[FedExPackage] ([ShipmentID])
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_ColumnsUpdated] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_ColumnsUpdated] ON [dbo].[FilterNodeContentDirty] ([ColumnsUpdated])
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_ParentIDObjectType] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_ParentIDObjectType] ON [dbo].[FilterNodeContentDirty] ([ParentID], [ObjectType]) INCLUDE ([ColumnsUpdated], [ComputerID])
GO
PRINT N'Creating index [IX_FilterNodeContentDirty_RowVersion] on [dbo].[FilterNodeContentDirty]'
GO
CREATE NONCLUSTERED INDEX [IX_FilterNodeContentDirty_RowVersion] ON [dbo].[FilterNodeContentDirty] ([RowVersion])
GO
PRINT N'Creating index [IX_iParcelPackage_ShipmentID] on [dbo].[iParcelPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_iParcelPackage_ShipmentID] ON [dbo].[iParcelPackage] ([ShipmentID])
GO
PRINT N'Creating index [IX_ObjectLabel_ObjectTypeIsDeleted] on [dbo].[ObjectLabel]'
GO
CREATE NONCLUSTERED INDEX [IX_ObjectLabel_ObjectTypeIsDeleted] ON [dbo].[ObjectLabel] ([ObjectType], [IsDeleted])
GO
PRINT N'Creating index [IX_ObjectReference_ConsumerIDReferenceKey] on [dbo].[ObjectReference]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectReference_ConsumerIDReferenceKey] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey])
GO
PRINT N'Creating index [IX_ObjectReference_ObjectID] on [dbo].[ObjectReference]'
GO
CREATE NONCLUSTERED INDEX [IX_ObjectReference_ObjectID] ON [dbo].[ObjectReference] ([ObjectID])
GO
PRINT N'Creating index [IX_ShipmentCustomsItem_ShipmentID] on [dbo].[ShipmentCustomsItem]'
GO
CREATE NONCLUSTERED INDEX [IX_ShipmentCustomsItem_ShipmentID] ON [dbo].[ShipmentCustomsItem] ([ShipmentID])
GO
PRINT N'Creating index [IX_UpsPackage_ShipmentID] on [dbo].[UpsPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_UpsPackage_ShipmentID] ON [dbo].[UpsPackage] ([ShipmentID])
GO
PRINT N'Creating index [IX_WorldShipPackage_ShipmentID] on [dbo].[WorldShipPackage]'
GO
CREATE NONCLUSTERED INDEX [IX_WorldShipPackage_ShipmentID] ON [dbo].[WorldShipPackage] ([ShipmentID])
GO
