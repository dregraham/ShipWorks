﻿DECLARE @OriginalRecoveryModel nvarchar(50)
DECLARE @SetRecoveryModelSimpleSql nvarchar(255)
DECLARE @SetRecoveryModelOriginalSql nvarchar(255)
DECLARE @RaiseErrorMsg nvarchar(500) = ''

USE [%databaseName%]

SELECT @OriginalRecoveryModel = recovery_model_desc
   FROM sys.databases
   WHERE name = '%databaseName%';

SET @SetRecoveryModelSimpleSql =
	'ALTER DATABASE [%databaseName%] SET RECOVERY SIMPLE WITH NO_WAIT';

SET @SetRecoveryModelOriginalSql =
	'ALTER DATABASE [%databaseName%] SET RECOVERY ' + @OriginalRecoveryModel + ' WITH NO_WAIT';

EXEC(@SetRecoveryModelSimpleSql)

BEGIN TRAN

SET NOCOUNT ON;

DECLARE @DisableAllTriggersSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllTriggersSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllIndexesSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllIndexesSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllChangeTrackingSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllChangeTrackingSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllForeignKeysSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllForeignKeysSql NVARCHAR(MAX) = N'';

/*******************************************************************/
/* Get static list of OrderIDs to delete.                          */
/* We won't delete or modify this list, it is only for joining to. */
/*******************************************************************/
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'OrderIDsToDelete')
BEGIN
	DROP TABLE [OrderIDsToDelete]
END

SELECT DISTINCT o.OrderID as 'EntityID'
INTO    dbo.[OrderIDsToDelete]
FROM    dbo.[Order] o
WHERE   o.OrderDate %orderDateComparer% '%orderDate%'
ORDER BY o.OrderID

--IF NOT EXISTS(SELECT TOP 1 * FROM dbo.[OrderIDsToDelete])
IF EXISTS(SELECT TOP 1 * FROM dbo.[OrderIDsToDelete])
BEGIN
--	RAISERROR('There are no orders older than selected date to archive.', 16, 1)
--END
--ELSE
--BEGIN
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'ShipmentIDsToDelete')
	BEGIN
		DROP TABLE [ShipmentIDsToDelete]
	END

	/*******************************************************************/
	/* Get static list of ShipmentIDs to delete.                       */
	/* We won't delete or modify this list, it is only for joining to. */
	/*******************************************************************/
	SELECT DISTINCT s.ShipmentID as 'EntityID'
	INTO    dbo.[ShipmentIDsToDelete]
	FROM    dbo.[Shipment] s
		inner join [OrderIDsToDelete] o on s.OrderID = o.EntityID
	ORDER BY s.Shipmentid

	/* Get Disable/Enable all table change tracking SQL. */
		SELECT @DisableAllChangeTrackingSql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(object_id)) + N' DISABLE CHANGE_TRACKING; ' + NCHAR(13),
			   @EnableAllChangeTrackingSql  += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(object_id)) + N' ENABLE CHANGE_TRACKING; ' + NCHAR(13)
		FROM sys.change_tracking_tables t
		ORDER BY OBJECT_NAME(object_id);

	/* Get Disable/Enable all triggers SQL. */
		SELECT @DisableAllTriggersSql += N'DISABLE TRIGGER ALL ON  ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(t.name) + N'; ' + NCHAR(13),
			   @EnableAllTriggersSql  += N'ENABLE TRIGGER ALL ON  ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(t.name) + N'; ' + NCHAR(13)
		FROM sys.tables AS t
		WHERE t.is_ms_shipped = 0
		ORDER BY t.name;

	/* Get Disable/Enable all indexes SQL. */
		SELECT @DisableAllIndexesSql += N'ALTER INDEX ' + QUOTENAME(I.name) + ' ON ' +  QUOTENAME(SCHEMA_NAME(T.schema_id))+'.'+ QUOTENAME(T.name) + ' DISABLE;' + NCHAR(13),
			   @EnableAllIndexesSql  += N'ALTER INDEX ' + QUOTENAME(I.name) + ' ON ' +  QUOTENAME(SCHEMA_NAME(T.schema_id))+'.'+ QUOTENAME(T.name) + ' REBUILD;' + NCHAR(13)
		FROM sys.indexes I
			INNER JOIN sys.tables T ON I.object_id = T.object_id
		WHERE I.type_desc = 'NONCLUSTERED'
			AND I.name IS NOT NULL
		ORDER BY t.name, I.name

	/* Get Disable/Enable all foreign keys SQL. */
		SELECT @DisableAllForeignKeysSql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(t.name) + N' NOCHECK CONSTRAINT ALL; ' + NCHAR(13),
			   @EnableAllForeignKeysSql  += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(t.name) + N' WITH CHECK CHECK CONSTRAINT  ALL; ' + NCHAR(13)
		FROM sys.tables AS t
		WHERE t.is_ms_shipped = 0
		ORDER BY t.name;

	/* Disable all triggers */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Disable all triggers'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@DisableAllTriggersSql);

	/* Disable all indexes */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Disable all indexes'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@DisableAllIndexesSql);

	/* Disable all change tracking */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Disable change tracking'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@DisableAllChangeTrackingSql);

	/* Disable all foreign keys */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Disable all foreign keys'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@DisableAllForeignKeysSql);

	/*******************************************************************/
	/* Populate the list of ShipmentIDs to delete.                     */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT DISTINCT EntityID
		INTO dbo.[EntityIDsToDelete]
		FROM dbo.[ShipmentIDsToDelete]
		ORDER BY EntityID

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 10 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	/*******************************************************************/
	/* Delete based on ShipmentIDs.                                    */
	/*******************************************************************/
	exec PurgeEntities 'WorldShipGoods', 'ShipmentID', 'WorldShipGoodsID'
	exec PurgeEntities 'WorldShipPackage', 'ShipmentID', 'UpsPackageID'
	exec PurgeEntities 'DhlExpressPackage', 'ShipmentID', 'DhlExpressPackageID'
	exec PurgeEntities 'EndiciaShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'FedExPackage', 'ShipmentID', 'FedExPackageID'
	exec PurgeEntities 'iParcelPackage', 'ShipmentID', 'iParcelPackageID'
	exec PurgeEntities 'UpsPackage', 'ShipmentID', 'UpsPackageID'
	exec PurgeEntities 'UspsShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'WorldShipShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'AmazonSFPShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'AmazonSWAShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'AsendiaShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'BestRateShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'DhlExpressShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'FedExShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'InsurancePolicy', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'iParcelShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'OnTracShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'OtherShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'PostalShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'UpsShipment', 'ShipmentID', 'ShipmentID'
	exec PurgeEntities 'ShipmentCustomsItem', 'ShipmentID', 'ShipmentCustomsItemID'
	exec PurgeEntities 'ShipmentReturnItem', 'ShipmentID', 'ShipmentReturnItemID'
	exec PurgeEntities 'WorldShipProcessed', 'ShipmentID', 'WorldShipProcessedID'
	exec PurgeEntities 'ValidatedAddress', 'ConsumerID', 'ValidatedAddressID'
	exec PurgeEntities 'Note', 'ObjectID', 'NoteID'
	exec PurgeEntities 'PrintResult', 'RelatedObjectID', 'PrintResultID'
	exec PurgeEntities 'PrintResult', 'ContextObjectID', 'PrintResultID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID', 'ObjectID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID', 'ObjectReferenceID'
	exec PurgeEntities 'ObjectReference', 'ObjectID', 'ObjectReferenceID'
	exec PurgeEntities 'EmailOutboundRelation', 'ObjectID', 'EmailOutboundRelationID'
	exec PurgeEntities 'EmailOutbound', 'ContextID', 'EmailOutboundID'
	exec PurgeEntities 'Shipment', 'ShipmentID', 'ShipmentID'
	DELETE FROM FedExEndOfDayClose WHERE CloseDate %orderDateComparer% '%orderDate%'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 20 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/*******************************************************************/
	/* Delete ScanForms based on deleted Shipments.                    */
	/*******************************************************************/
	DECLARE @ScanFormBatchesToKeep TABLE([ScanFormBatchID] bigint);

	INSERT INTO @ScanFormBatchesToKeep(ScanFormBatchID)
		SELECT DISTINCT ScanFormBatchID FROM EndiciaShipment WHERE ScanFormBatchID IS NOT NULL
		UNION ALL
		SELECT DISTINCT ScanFormBatchID FROM UspsShipment WHERE ScanFormBatchID IS NOT NULL

	DELETE FROM EndiciaScanForm
	WHERE ScanFormBatchID NOT IN
	(
		SELECT DISTINCT ScanFormBatchID FROM @ScanFormBatchesToKeep
	)

	DELETE FROM UspsScanForm
	WHERE ScanFormBatchID NOT IN
	(
		SELECT DISTINCT ScanFormBatchID FROM @ScanFormBatchesToKeep
	)

	DELETE FROM ScanFormBatch
	WHERE ScanFormBatchID NOT IN
	(
		SELECT DISTINCT ScanFormBatchID FROM @ScanFormBatchesToKeep
	)

	/* Now do matching Audit entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    [Audit] a, [ShipmentIDsToDelete] s
		WHERE   s.EntityID = a.ObjectID

	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'

	/* Now do matching AuditChange entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    [AuditChange] a, [ShipmentIDsToDelete] s
		WHERE   s.EntityID = a.ObjectID

	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 30 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/*******************************************************************/
	/* Populate the list of OrderItemAttributeIDs to delete.           */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT DISTINCT oia.OrderItemAttributeID as 'EntityID'
	INTO    dbo.[EntityIDsToDelete]
	FROM    dbo.[OrderItem] oi inner join [OrderIDsToDelete] o on oi.OrderID = o.EntityID
			inner join [OrderItemAttribute] oia on oia.OrderItemID = oi.OrderItemID
	ORDER BY oia.OrderItemAttributeID

	/*******************************************************************/
	/* Delete based on OrderItemAttributeIDs.                          */
	/*******************************************************************/
	exec PurgeEntities 'ObjectLabel', 'ObjectID', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID', 'ObjectID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID', 'ObjectReferenceID'
	exec PurgeEntities 'ObjectReference', 'ObjectID', 'ObjectReferenceID'
	exec PurgeEntities 'MivaOrderItemAttribute', 'OrderItemAttributeID', 'OrderItemAttributeID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 40 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/*******************************************************************/
	/* Populate the list of OrderItemIDs to delete.                    */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT DISTINCT oi.OrderItemID as 'EntityID'
	INTO    dbo.[EntityIDsToDelete]
	FROM    dbo.[OrderItem] oi
			inner join [OrderIDsToDelete] o on oi.OrderID = o.EntityID
	ORDER BY oi.OrderItemID

	/*******************************************************************/
	/* Delete based on OrderItemIDs.                                   */
	/*******************************************************************/
	exec PurgeEntities 'ObjectLabel', 'ObjectID', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID', 'ObjectID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID', 'ObjectReferenceID'
	exec PurgeEntities 'ObjectReference', 'ObjectID', 'ObjectReferenceID'
	exec PurgeEntities 'AmazonOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'BigCommerceOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'BuyDotComOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'ChannelAdvisorOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'EbayOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'EtsyOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'GenericModuleOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'GrouponOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'InfopiaOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'JetOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'LemonStandOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'ShopifyOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'NeweggOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'OverstockOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'OrderItemAttribute', 'OrderItemID', 'OrderItemAttributeID'
	exec PurgeEntities 'SearsOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'ThreeDCartOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'WalmartOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'YahooOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'OrderItem', 'OrderItemID', 'OrderItemID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 50 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/*******************************************************************/
	/* Populate the list of OrderIDs to delete.                        */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT DISTINCT o.EntityID as 'EntityID'
	INTO    dbo.[EntityIDsToDelete]
	FROM    dbo.[OrderIDsToDelete] o
	ORDER BY o.EntityID

	/*******************************************************************/
	/* Delete based on OrderIDs.                                       */
	/*******************************************************************/
	exec PurgeEntities 'AmazonOrderSearch', 'OrderID', 'AmazonOrderSearchID'
	exec PurgeEntities 'ChannelAdvisorOrderSearch', 'OrderID', 'ChannelAdvisorOrderSearchID'
	exec PurgeEntities 'OverstockOrderSearch', 'OrderID', 'OverstockOrderSearchID'
	exec PurgeEntities 'ClickCartProOrderSearch', 'OrderID', 'ClickCartProOrderSearchID'
	exec PurgeEntities 'CommerceInterfaceOrderSearch', 'OrderID', 'CommerceInterfaceOrderSearchID'
	exec PurgeEntities 'EbayCombinedOrderRelation', 'OrderID', 'EbayCombinedOrderRelationID'
	exec PurgeEntities 'EbayOrderSearch', 'OrderID', 'EbayOrderSearchID'
	exec PurgeEntities 'GrouponOrderSearch', 'OrderID', 'GrouponOrderSearchID'
	exec PurgeEntities 'JetOrderSearch', 'OrderID', 'JetOrderSearchID'
	exec PurgeEntities 'LemonStandOrderSearch', 'OrderID', 'LemonStandOrderSearchID'
	exec PurgeEntities 'MagentoOrderSearch', 'OrderID', 'MagentoOrderSearchID'
	exec PurgeEntities 'MarketplaceAdvisorOrderSearch', 'OrderID', 'MarketplaceAdvisorOrderSearchID'
	exec PurgeEntities 'NetworkSolutionsOrderSearch', 'OrderID', 'NetworkSolutionsOrderSearchID'
	exec PurgeEntities 'OrderMotionOrderSearch', 'OrderID', 'OrderMotionOrderSearchID'
	exec PurgeEntities 'PayPalOrderSearch', 'OrderID', 'PayPalOrderSearchID'
	exec PurgeEntities 'ProStoresOrderSearch', 'OrderID', 'ProStoresOrderSearchID'
	exec PurgeEntities 'RakutenOrderSearch', 'OrderID', 'RakutenOrderSearchID'
	exec PurgeEntities 'SearsOrderSearch', 'OrderID', 'SearsOrderSearchID'
	exec PurgeEntities 'ShopifyOrderSearch', 'OrderID', 'ShopifyOrderSearchID'
	exec PurgeEntities 'ThreeDCartOrderSearch', 'OrderID', 'ThreeDCartOrderSearchID'
	exec PurgeEntities 'WalmartOrderSearch', 'OrderID', 'WalmartOrderSearchID'
	exec PurgeEntities 'YahooOrderSearch', 'OrderID', 'YahooOrderSearchID'
	exec PurgeEntities 'AmazonOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ProStoresOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ChannelAdvisorOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'OverstockOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ClickCartProOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'CommerceInterfaceOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'EbayOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'EtsyOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'GenericModuleOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'GrouponOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'JetOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'LemonStandOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'MagentoOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'MarketplaceAdvisorOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'NetworkSolutionsOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'NeweggOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'OrderCharge', 'OrderID', 'OrderChargeID'
	exec PurgeEntities 'OrderMotionOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'OrderPaymentDetail', 'OrderID', 'OrderPaymentDetailID'
	exec PurgeEntities 'OrderSearch', 'OrderID', 'OrderSearchID'
	exec PurgeEntities 'PayPalOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'RakutenOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'SearsOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ShopifyOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ThreeDCartOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'WalmartOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'YahooOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'Note', 'ObjectID', 'NoteID'
	exec PurgeEntities 'ValidatedAddress', 'ConsumerID', 'ValidatedAddressID'
	exec PurgeEntities 'PrintResult', 'RelatedObjectID', 'PrintResultID'
	exec PurgeEntities 'PrintResult', 'ContextObjectID', 'PrintResultID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID', 'ObjectID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID', 'ObjectReferenceID'
	exec PurgeEntities 'ObjectReference', 'ObjectID', 'ObjectReferenceID'
	exec PurgeEntities 'EmailOutboundRelation', 'ObjectID', 'EmailOutboundRelationID'
	exec PurgeEntities 'EmailOutbound', 'ContextID', 'EmailOutboundID'
	exec PurgeEntities 'DownloadDetail', 'OrderID', 'DownloadedDetailID'
	exec PurgeEntities 'Order', 'OrderID', 'OrderID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 70 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/* Now do matching Audit entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID  as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderIDsToDelete] o, [Audit] a
		WHERE   o.EntityID = a.ObjectID

	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 80 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/* Now do matching AuditChange entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID  as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderIDsToDelete] o, [AuditChange] a
		WHERE   o.EntityID = a.ObjectID

	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 90 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/*************************************/
	/*   Purge Abandoned Resources       */
	/*************************************/
	exec PurgeAbandonedResources @runUntil = null, @olderThan = NULL, @softDelete = 1

	/* The "All" Order filter does not get updated by filter regen, so force it's count to be correct. */
	UPDATE FilterNodeContent SET [Count] = (SELECT COUNT(*) FROM [Order]) WHERE FilterNodeContentID = -26

	/* Cleanup */
	/* Enable all triggers */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Enable all triggers.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@EnableAllTriggersSql);

	/* Enable all indexes */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Rebuild all indexes.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@EnableAllIndexesSql);

	/* Enable all change tracking */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Enable change tracking.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@EnableAllChangeTrackingSql);

	/* Enable all foreign keys */
	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - Enable all foreign keys.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT
	exec (@EnableAllForeignKeysSql);

	SET @RaiseErrorMsg = 'OrderArchiveInfo: (' + db_name() + ') - 100 percent processed.'
	RAISERROR (@RaiseErrorMsg, 0, 1) WITH NOWAIT

	/* Drop id holding tables */
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'OrderIDsToDelete')
	BEGIN
		DROP TABLE [OrderIDsToDelete]
	END

	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'ShipmentIDsToDelete')
	BEGIN
		DROP TABLE [ShipmentIDsToDelete]
	END
END

COMMIT TRAN

/* THIS MUST BE DONE OUTSIDE THE TRAN!!! */
/* Put the database back in its original recovery model */
EXEC(@SetRecoveryModelOriginalSql)

EXEC('ALTER DATABASE %databaseName% SET MULTI_USER;')
