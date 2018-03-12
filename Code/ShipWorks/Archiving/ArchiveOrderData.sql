DECLARE @OriginalRecoveryModel nvarchar(50)
DECLARE @SetRecoveryModelSimpleSql nvarchar(255)
DECLARE @SetRecoveryModelOriginalSql nvarchar(255)

SELECT @OriginalRecoveryModel = recovery_model_desc  
   FROM sys.databases  
   WHERE name = DB_NAME(); 

SET @SetRecoveryModelSimpleSql = 
	'USE [master]
	
	ALTER DATABASE [' + DB_NAME() + '] SET RECOVERY SIMPLE WITH NO_WAIT
	
	use ' + DB_NAME() + '';

SET @SetRecoveryModelOriginalSql = 
	'USE [master]
	
	ALTER DATABASE [' + DB_NAME() + '] SET RECOVERY ' + @OriginalRecoveryModel + ' WITH NO_WAIT
	
	use ' + DB_NAME() + '';
	
EXEC(@SetRecoveryModelSimpleSql)

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
WHERE   o.OrderDate <= '{0}'
ORDER BY o.OrderID

IF NOT EXISTS(SELECT TOP 1 * FROM dbo.[OrderIDsToDelete])
BEGIN
	RAISERROR('There are no orders older than selected date to archive.', 16, 1)
END
ELSE
BEGIN

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
	exec (@DisableAllTriggersSql);

	/* Disable all indexes */
	exec (@DisableAllIndexesSql);

	/* Disable all change tracking */
	exec (@DisableAllChangeTrackingSql);

	/* Disable all change tracking */
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
	exec PurgeEntities 'AmazonShipment', 'ShipmentID', 'ShipmentID'
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
	exec PurgeEntities 'PrintResult', 'RelatedObjectID', 'PrintResultID'
	exec PurgeEntities 'PrintResult', 'ContextObjectID', 'PrintResultID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID', 'ObjectID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID', 'ObjectReferenceID'
	exec PurgeEntities 'ObjectReference', 'ObjectID', 'ObjectReferenceID'
	exec PurgeEntities 'EmailOutboundRelation', 'ObjectID', 'EmailOutboundRelationID'
	exec PurgeEntities 'EmailOutbound', 'ContextID', 'EmailOutboundID'
	exec PurgeEntities 'Shipment', 'ShipmentID', 'ShipmentID'
		
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
	exec PurgeEntities 'OrderItemAttribute', 'OrderItemID', 'OrderItemAttributeID'
	exec PurgeEntities 'SearsOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'ThreeDCartOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'WalmartOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'YahooOrderItem', 'OrderItemID', 'OrderItemID'
	exec PurgeEntities 'OrderItem', 'OrderItemID', 'OrderItemID'
	
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
	exec PurgeEntities 'SearsOrderSearch', 'OrderID', 'SearsOrderSearchID'
	exec PurgeEntities 'ShopifyOrderSearch', 'OrderID', 'ShopifyOrderSearchID'
	exec PurgeEntities 'ThreeDCartOrderSearch', 'OrderID', 'ThreeDCartOrderSearchID'
	exec PurgeEntities 'WalmartOrderSearch', 'OrderID', 'WalmartOrderSearchID'
	exec PurgeEntities 'YahooOrderSearch', 'OrderID', 'YahooOrderSearchID'
	exec PurgeEntities 'AmazonOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ProStoresOrder', 'OrderID', 'OrderID'
	exec PurgeEntities 'ChannelAdvisorOrder', 'OrderID', 'OrderID'
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
	exec PurgeEntities 'DownloadDetail', 'DownloadedDetailID', 'DownloadedDetailID'
	exec PurgeEntities 'Order', 'OrderID', 'OrderID'

	/* Now do matching Audit entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID  as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderIDsToDelete] o, [Audit] a
		WHERE   o.EntityID = a.ObjectID
	
	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'
		
	/* Now do matching AuditChange entries */
	DROP TABLE  dbo.[EntityIDsToDelete]
	SELECT DISTINCT a.AuditID  as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderIDsToDelete] o, [AuditChange] a
		WHERE   o.EntityID = a.ObjectID
	
	exec PurgeEntities 'AuditChangeDetail', 'AuditID', 'AuditChangeDetailID'
	exec PurgeEntities 'AuditChange', 'AuditID', 'AuditChangeID'
	exec PurgeEntities 'Audit', 'AuditID', 'AuditID'

	/*************************************/
	/*   Purge Abandoned Resources       */
	/*************************************/
	exec PurgeAbandonedResources @runUntil = null, @olderThan = null

	/* Cleanup */
	/* Enable all triggers */
	print 'Enable all triggers'
	exec (@EnableAllTriggersSql);

	/* Enable all indexes */
	print 'Enable all indexes'
	exec (@EnableAllIndexesSql);

	/* Enable all change tracking */
	print 'Enable all change tracking'
	exec (@EnableAllChangeTrackingSql);

	/* Enable all foreign keys */
	print 'Enable all foreign keys';
	exec (@EnableAllForeignKeysSql);

	/* Put the database back in its original recovery model */
	print 'Setting original recovery model';
	EXEC(@SetRecoveryModelOriginalSql)

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