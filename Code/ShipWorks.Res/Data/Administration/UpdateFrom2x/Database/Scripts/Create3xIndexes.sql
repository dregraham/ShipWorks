IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EmailOutbound' AND object_id = OBJECT_ID('[EmailOutbound]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_EmailOutbound] ON [dbo].[EmailOutbound] ([SendStatus], [AccountID], [DontSendBefore], [SentDate], [ComposedDate])
	SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Audit_Action' AND object_id = OBJECT_ID('[Audit]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Audit_Action] ON [dbo].[Audit] ([Action])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Audit_TransactionID' AND object_id = OBJECT_ID('[Audit]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Audit_TransactionID] ON [dbo].[Audit] ([TransactionID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_AuditChange' AND object_id = OBJECT_ID('[AuditChange]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AuditChange] ON [dbo].[AuditChange] ([AuditID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_AuditChangeDetail' AND object_id = OBJECT_ID('[AuditChangeDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail] ON [dbo].[AuditChangeDetail] ([AuditChangeID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_AuditChangeDetail_VariantNew' AND object_id = OBJECT_ID('[AuditChangeDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AuditChangeDetail_VariantNew] ON [dbo].[AuditChangeDetail] ([VariantNew]) INCLUDE ([AuditID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCity' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCity] ON [dbo].[Customer] ([BillCity]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCompany' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Customer] ([BillCompany]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCountryCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Customer] ([BillCountryCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillEmail' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Customer] ([BillEmail]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillFax' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillFax] ON [dbo].[Customer] ([BillFax]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillFirstName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Customer] ([BillFirstName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillLastName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Customer] ([BillLastName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillMiddleName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillMiddleName] ON [dbo].[Customer] ([BillMiddleName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillPhone' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillPhone] ON [dbo].[Customer] ([BillPhone]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillPostalCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Customer] ([BillPostalCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStateProvCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Customer] ([BillStateProvCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet1' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet1] ON [dbo].[Customer] ([BillStreet1]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet2' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet2] ON [dbo].[Customer] ([BillStreet2]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet3' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet3] ON [dbo].[Customer] ([BillStreet3]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillWebsite' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillWebsite] ON [dbo].[Customer] ([BillWebsite]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupOrderCount' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderCount] ON [dbo].[Customer] ([RollupOrderCount]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupNoteCount' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Customer] ([RollupNoteCount]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END


IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupOrderTotal' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupOrderTotal] ON [dbo].[Customer] ([RollupOrderTotal]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCity' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCity] ON [dbo].[Customer] ([ShipCity]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCompany' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Customer] ([ShipCompany]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCountryCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Customer] ([ShipCountryCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipEmail' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Customer] ([ShipEmail]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipFax' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipFax] ON [dbo].[Customer] ([ShipFax]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipFirstName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Customer] ([ShipFirstName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipLastName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Customer] ([ShipLastName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipMiddleName' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Customer] ([ShipMiddleName]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipPhone' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipPhone] ON [dbo].[Customer] ([ShipPhone]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipPostalCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Customer] ([ShipPostalCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStateProvCode' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Customer] ([ShipStateProvCode]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet1' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet1] ON [dbo].[Customer] ([ShipStreet1]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet2' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet2] ON [dbo].[Customer] ([ShipStreet2]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet3' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet3] ON [dbo].[Customer] ([ShipStreet3]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipWebsite' AND object_id = OBJECT_ID('[Customer]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipWebsite] ON [dbo].[Customer] ([ShipWebsite]) INCLUDE ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DimensionsProfile_Name' AND object_id = OBJECT_ID('[DimensionsProfile]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_DimensionsProfile_Name] ON [dbo].[DimensionsProfile] ([Name])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DownloadLog_StoreID_Ended' AND object_id = OBJECT_ID('[Download]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DownloadLog_StoreID_Ended] ON [dbo].[Download] ([StoreID], [Ended])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DownloadDetail_AmazonOrderID' AND object_id = OBJECT_ID('[DownloadDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DownloadDetail_AmazonOrderID] ON [dbo].[DownloadDetail] ([AmazonOrderID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DownloadDetail_Ebay' AND object_id = OBJECT_ID('[DownloadDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DownloadDetail_Ebay] ON [dbo].[DownloadDetail] ([EbayItemID], [EbayOrderID], [EbayTransactionID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DownloadDetail_OrderNumber' AND object_id = OBJECT_ID('[DownloadDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DownloadDetail_OrderNumber] ON [dbo].[DownloadDetail] ([OrderNumber])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EmailOutbound_Email' AND object_id = OBJECT_ID('[EmailOutboundRelation]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Email] ON [dbo].[EmailOutboundRelation] ([EmailOutboundID], [RelationType]) INCLUDE ([ObjectID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EmailOutbound_Object' AND object_id = OBJECT_ID('[EmailOutboundRelation]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_EmailOutbound_Object] ON [dbo].[EmailOutboundRelation] ([ObjectID], [RelationType]) INCLUDE ([EmailOutboundID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FedExEndOfDayClose_CloseDate' AND object_id = OBJECT_ID('[FedExEndOfDayClose]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FedExEndOfDayClose_CloseDate] ON [dbo].[FedExEndOfDayClose] ([CloseDate]) INCLUDE ([FedExAccountID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterLayout' AND object_id = OBJECT_ID('[FilterLayout]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterLayout] ON [dbo].[FilterLayout] ([UserID], [FilterTarget])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNode_ParentFilterNodeID' AND object_id = OBJECT_ID('[FilterNode]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FilterNode_ParentFilterNodeID] ON [dbo].[FilterNode] ([ParentFilterNodeID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeColumnSettings' AND object_id = OBJECT_ID('[FilterNodeColumnSettings]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeColumnSettings] ON [dbo].[FilterNodeColumnSettings] ([UserID], [FilterNodeID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeCountDetail' AND object_id = OBJECT_ID('[FilterNodeContentDetail]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeCountDetail] ON [dbo].[FilterNodeContentDetail] ([FilterNodeContentID], [ObjectID]) WITH (IGNORE_DUP_KEY=ON)
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeCountDirty' AND object_id = OBJECT_ID('[FilterNodeContentDirty]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FilterNodeCountDirty] ON [dbo].[FilterNodeContentDirty] ([RowVersion]) INCLUDE ([ComputerID], [ObjectID], [ObjectType])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ObjectDeletion_Date' AND object_id = OBJECT_ID('[FilterNodeContentRemoved]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ObjectDeletion_Date] ON [dbo].[FilterNodeContentRemoved] ([DeletionDate])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ObjectDeletion_RowVersion' AND object_id = OBJECT_ID('[FilterNodeContentRemoved]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectDeletion_RowVersion] ON [dbo].[FilterNodeContentRemoved] ([ObjectType], [RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeUpdateCustomer' AND object_id = OBJECT_ID('[FilterNodeUpdateCustomer]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateCustomer] ON [dbo].[FilterNodeUpdateCustomer] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeUpdateItem' AND object_id = OBJECT_ID('[FilterNodeUpdateItem]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateItem] ON [dbo].[FilterNodeUpdateItem] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeUpdateOrder' AND object_id = OBJECT_ID('[FilterNodeUpdateOrder]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateOrder] ON [dbo].[FilterNodeUpdateOrder] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterNodeUpdateShipment' AND object_id = OBJECT_ID('[FilterNodeUpdateShipment]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FilterNodeUpdateShipment] ON [dbo].[FilterNodeUpdateShipment] ([ObjectID]) WITH (IGNORE_DUP_KEY=ON)
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FilterChild_ParentFilterID' AND object_id = OBJECT_ID('[FilterSequence]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FilterChild_ParentFilterID] ON [dbo].[FilterSequence] ([ParentFilterID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_GridColumnDisplay' AND object_id = OBJECT_ID('[GridColumnFormat]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_GridColumnDisplay] ON [dbo].[GridColumnFormat] ([UserID], [ColumnGuid])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_GridLayoutColumn' AND object_id = OBJECT_ID('[GridColumnPosition]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_GridLayoutColumn] ON [dbo].[GridColumnPosition] ([GridColumnLayoutID], [ColumnGuid])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_LabelSheet_Name' AND object_id = OBJECT_ID('[LabelSheet]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_LabelSheet_Name] ON [dbo].[LabelSheet] ([Name])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderNote_ObjectID' AND object_id = OBJECT_ID('[Note]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderNote_ObjectID] ON [dbo].[Note] ([ObjectID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ObjectLabel' AND object_id = OBJECT_ID('[ObjectLabel]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ObjectLabel] ON [dbo].[ObjectLabel] ([ObjectType], [IsDeleted])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ObjectReference' AND object_id = OBJECT_ID('[ObjectReference]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectReference] ON [dbo].[ObjectReference] ([ConsumerID], [ReferenceKey])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCity' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCity] ON [dbo].[Order] ([BillCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCompany' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCompany] ON [dbo].[Order] ([BillCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillCountryCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillCountryCode] ON [dbo].[Order] ([BillCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillEmail' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillEmail] ON [dbo].[Order] ([BillEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillFax' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillFax] ON [dbo].[Order] ([BillFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillFirstName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillFirstName] ON [dbo].[Order] ([BillFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillLastName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillLastName] ON [dbo].[Order] ([BillLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillMiddleName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillMiddleName] ON [dbo].[Order] ([BillMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillPhone' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillPhone] ON [dbo].[Order] ([BillPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillPostalCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillPostalCode] ON [dbo].[Order] ([BillPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStateProvCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStateProvCode] ON [dbo].[Order] ([BillStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet1' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet1] ON [dbo].[Order] ([BillStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet2' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet2] ON [dbo].[Order] ([BillStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillStreet3' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillStreet3] ON [dbo].[Order] ([BillStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_BillWebsite' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_BillWebsite] ON [dbo].[Order] ([BillWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_CustomerID' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_CustomerID] ON [dbo].[Order] ([CustomerID]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_IsManual' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_IsManual] ON [dbo].[Order] ([IsManual]) INCLUDE ([RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_LocalStatus' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_LocalStatus] ON [dbo].[Order] ([LocalStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OnlineCustomerID' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OnlineCustomerID] ON [dbo].[Order] ([OnlineCustomerID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OnlineLastModified' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OnlineLastModified] ON [dbo].[Order] ([OnlineLastModified]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OnlineLastModified_StoreID_IsManual' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OnlineLastModified_StoreID_IsManual] ON [dbo].[Order] ([OnlineLastModified] DESC, [StoreID], [IsManual])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OnlineStatus' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OnlineStatus] ON [dbo].[Order] ([OnlineStatus]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OrderDate' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OrderDate] ON [dbo].[Order] ([OrderDate]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OrderNumber' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumber] ON [dbo].[Order] ([OrderNumber]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OrderNumberComplete' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OrderNumberComplete] ON [dbo].[Order] ([OrderNumberComplete]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_OrderTotal' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_OrderTotal] ON [dbo].[Order] ([OrderTotal]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RequestedShipping' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RequestedShipping] ON [dbo].[Order] ([RequestedShipping]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemCount' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCount] ON [dbo].[Order] ([RollupItemCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemCode] ON [dbo].[Order] ([RollupItemCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemLocation' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemLocation] ON [dbo].[Order] ([RollupItemLocation]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemName] ON [dbo].[Order] ([RollupItemName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemQuantity' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemQuantity] ON [dbo].[Order] ([RollupItemQuantity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemSKU' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemSKU] ON [dbo].[Order] ([RollupItemSKU]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupNoteCount' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupNoteCount] ON [dbo].[Order] ([RollupNoteCount]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCity' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCity] ON [dbo].[Order] ([ShipCity]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCompany' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCompany] ON [dbo].[Order] ([ShipCompany]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipCountryCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipCountryCode] ON [dbo].[Order] ([ShipCountryCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipEmail' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipEmail] ON [dbo].[Order] ([ShipEmail]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipFax' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipFax] ON [dbo].[Order] ([ShipFax]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipFirstName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipFirstName] ON [dbo].[Order] ([ShipFirstName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipLastName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipLastName] ON [dbo].[Order] ([ShipLastName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipMiddleName' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipMiddleName] ON [dbo].[Order] ([ShipMiddleName]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipPhone' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipPhone] ON [dbo].[Order] ([ShipPhone]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipPostalCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipPostalCode] ON [dbo].[Order] ([ShipPostalCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStateProvCode' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStateProvCode] ON [dbo].[Order] ([ShipStateProvCode]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet1' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet1] ON [dbo].[Order] ([ShipStreet1]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet2' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet2] ON [dbo].[Order] ([ShipStreet2]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipStreet3' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipStreet3] ON [dbo].[Order] ([ShipStreet3]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_ShipWebsite' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_ShipWebsite] ON [dbo].[Order] ([ShipWebsite]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_StoreID' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_StoreID] ON [dbo].[Order] ([StoreID]) INCLUDE ([IsManual], [RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderCharge_OrderID' AND object_id = OBJECT_ID('[OrderCharge]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderCharge_OrderID] ON [dbo].[OrderCharge] ([OrderID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderItem_OrderID' AND object_id = OBJECT_ID('[OrderItem]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderID] ON [dbo].[OrderItem] ([OrderID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderItemAttribute_OrderItemID' AND object_id = OBJECT_ID('[OrderItemAttribute]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderItemAttribute_OrderItemID] ON [dbo].[OrderItemAttribute] ([OrderItemID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderPaymentDetail_OrderID' AND object_id = OBJECT_ID('[OrderPaymentDetail]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_OrderPaymentDetail_OrderID] ON [dbo].[OrderPaymentDetail] ([OrderID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Permission' AND object_id = OBJECT_ID('[Permission]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Permission] ON [dbo].[Permission] ([UserID], [PermissionType], [ObjectID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Resource_Filename' AND object_id = OBJECT_ID('[Resource]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Filename] ON [dbo].[Resource] ([Filename])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ServerMessage_Expires' AND object_id = OBJECT_ID('[ServerMessage]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ServerMessage_Expires] ON [dbo].[ServerMessage] ([Expires])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ServerMessage_Number' AND object_id = OBJECT_ID('[ServerMessage]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_Number] ON [dbo].[ServerMessage] ([Number])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ServerMessage_RowVersion' AND object_id = OBJECT_ID('[ServerMessage]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessage_RowVersion] ON [dbo].[ServerMessage] ([RowVersion])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ServerMessageSignoff' AND object_id = OBJECT_ID('[ServerMessageSignoff]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerMessageSignoff] ON [dbo].[ServerMessageSignoff] ([UserID], [ComputerID], [ServerMessageID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Shipment_OrderID' AND object_id = OBJECT_ID('[Shipment]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID] ON [dbo].[Shipment] ([OrderID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Store_StoreName' AND object_id = OBJECT_ID('[Store]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Store_StoreName] ON [dbo].[Store] ([StoreName])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_TemplateStoreSettings' AND object_id = OBJECT_ID('[TemplateStoreSettings]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_TemplateStoreSettings] ON [dbo].[TemplateStoreSettings] ([TemplateID], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_User_Username' AND object_id = OBJECT_ID('[User]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Username] ON [dbo].[User] ([Username])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_UserColumnSettings' AND object_id = OBJECT_ID('[UserColumnSettings]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_UserColumnSettings] ON [dbo].[UserColumnSettings] ([UserID], [SettingsKey])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_VersionSignoff' AND object_id = OBJECT_ID('[VersionSignoff]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_VersionSignoff] ON [dbo].[VersionSignoff] ([ComputerID], [UserID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Auto_RollupItemTotalWeight' AND object_id = OBJECT_ID('[Order]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Auto_RollupItemTotalWeight] ON [dbo].[Order] ([RollupItemTotalWeight]) INCLUDE ([IsManual], [RowVersion], [StoreID])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ActionQueue_Search' AND object_id = OBJECT_ID('[ActionQueue]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ActionQueue_Search] ON [dbo].[ActionQueue] ([ActionQueueID], [RunComputerID], [Status])
    SELECT 1 as WorkCompleted
    RETURN
END

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ActionQueue_ContextLock' AND object_id = OBJECT_ID('[ActionQueue]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ActionQueue_ContextLock] ON [dbo].[ActionQueue] ([ContextLock])
    SELECT 1 as WorkCompleted
    RETURN
END

CREATE NONCLUSTERED INDEX [IX_PrintResult_RelatedObjectID] ON [dbo].[PrintResult] ([RelatedObjectID])
CREATE UNIQUE NONCLUSTERED INDEX [IX_Resource_Checksum] ON [dbo].[Resource] ([Checksum])
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShippingOrigin_Description] ON [dbo].[ShippingOrigin] ([Description])

SELECT 0 as WorkCompleted