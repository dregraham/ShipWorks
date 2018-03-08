SET NOCOUNT ON;
GO

DECLARE @DisableAllTriggersSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllTriggersSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllIndexesSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllIndexesSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllChangeTrackingSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllChangeTrackingSql NVARCHAR(MAX) = N'';
DECLARE @DisableAllForeignKeysSql NVARCHAR(MAX) = N'';
DECLARE @EnableAllForeignKeysSql NVARCHAR(MAX) = N'';

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
/* Get static list of OrderIDs to delete.                          */
/* We won't delete or modify this list, it is only for joining to. */
/*******************************************************************/
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'OrderIDsToDelete')
BEGIN
	DROP TABLE [OrderIDsToDelete]
END

	SELECT o.OrderID as 'EntityID'
	INTO    dbo.[OrderIDsToDelete]
	FROM    dbo.[Order] o
	WHERE   o.OrderDate <= '{0}'
	ORDER BY o.OrderID

	/*******************************************************************/
	/* Populate the list of ShipmentIDs to delete.                     */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT s.Shipmentid as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[Shipment] s
			inner join [OrderIDsToDelete] o on s.OrderID = o.EntityID
		ORDER BY s.Shipmentid
	
BEGIN TRAN

	/*******************************************************************/
	/* Delete based on ShipmentIDs.                                    */
	/*******************************************************************/
	exec PurgeEntities 'WorldShipGoods', 'ShipmentID'
	exec PurgeEntities 'WorldShipPackage', 'ShipmentID'
	exec PurgeEntities 'DhlExpressPackage', 'ShipmentID'
	exec PurgeEntities 'EndiciaShipment', 'ShipmentID'
	exec PurgeEntities 'FedExPackage', 'ShipmentID'
	exec PurgeEntities 'iParcelPackage', 'ShipmentID'
	exec PurgeEntities 'UpsPackage', 'ShipmentID'
	exec PurgeEntities 'UspsShipment', 'ShipmentID'
	exec PurgeEntities 'WorldShipShipment', 'ShipmentID'
	exec PurgeEntities 'AmazonShipment', 'ShipmentID'
	exec PurgeEntities 'AsendiaShipment', 'ShipmentID'
	exec PurgeEntities 'BestRateShipment', 'ShipmentID'
	exec PurgeEntities 'DhlExpressShipment', 'ShipmentID'
	exec PurgeEntities 'FedExShipment', 'ShipmentID'
	exec PurgeEntities 'InsurancePolicy', 'ShipmentID'
	exec PurgeEntities 'iParcelShipment', 'ShipmentID'
	exec PurgeEntities 'OnTracShipment', 'ShipmentID'
	exec PurgeEntities 'OtherShipment', 'ShipmentID'
	exec PurgeEntities 'PostalShipment', 'ShipmentID'
	exec PurgeEntities 'UpsShipment', 'ShipmentID'
	exec PurgeEntities 'ShipmentCustomsItem', 'ShipmentID'
	exec PurgeEntities 'ShipmentReturnItem', 'ShipmentID'
	exec PurgeEntities 'WorldShipProcessed', 'ShipmentID'
	exec PurgeEntities 'Shipment', 'ShipmentID'
	exec PurgeEntities 'ValidatedAddress', 'ConsumerID'
	exec PurgeEntities 'PrintResult', 'RelatedObjectID'
	exec PurgeEntities 'PrintResult', 'ContextObjectID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID'
	exec PurgeEntities 'ObjectReference', 'ObjectID'
	exec PurgeEntities 'EmailOutboundRelation', 'ObjectID'
	exec PurgeEntities 'EmailOutbound', 'ContextID'
	
	
	/*******************************************************************/
	/* Populate the list of OrderItemAttributeIDs to delete.           */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT oia.OrderItemAttributeID as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderItem] oi inner join [OrderIDsToDelete] o on oi.OrderID = o.EntityID
			    inner join [OrderItemAttribute] oia on oia.OrderItemID = oi.OrderItemID
	ORDER BY oia.OrderItemAttributeID
	
	/*******************************************************************/
	/* Delete based on OrderItemAttributeIDs.                          */
	/*******************************************************************/
	exec PurgeEntities 'MivaOrderItemAttribute', 'OrderItemAttributeID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID'
	exec PurgeEntities 'ObjectReference', 'ObjectID'
	
	
	/*******************************************************************/
	/* Populate the list of OrderItemIDs to delete.                    */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT oi.OrderItemID as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderItem] oi
			    inner join [OrderIDsToDelete] o on oi.OrderID = o.EntityID
	ORDER BY oi.OrderItemID
	
	/*******************************************************************/
	/* Delete based on OrderItemIDs.                                   */
	/*******************************************************************/
	exec PurgeEntities 'AmazonOrderItem', 'OrderItemID'
	exec PurgeEntities 'BigCommerceOrderItem', 'OrderItemID'
	exec PurgeEntities 'BuyDotComOrderItem', 'OrderItemID'
	exec PurgeEntities 'ChannelAdvisorOrderItem', 'OrderItemID'
	exec PurgeEntities 'EbayOrderItem', 'OrderItemID'
	exec PurgeEntities 'EtsyOrderItem', 'OrderItemID'
	exec PurgeEntities 'GenericModuleOrderItem', 'OrderItemID'
	exec PurgeEntities 'GrouponOrderItem', 'OrderItemID'
	exec PurgeEntities 'InfopiaOrderItem', 'OrderItemID'
	exec PurgeEntities 'JetOrderItem', 'OrderItemID'
	exec PurgeEntities 'LemonStandOrderItem', 'OrderItemID'
	exec PurgeEntities 'ShopifyOrderItem', 'OrderItemID'
	exec PurgeEntities 'NeweggOrderItem', 'OrderItemID'
	exec PurgeEntities 'OrderItemAttribute', 'OrderItemID'
	exec PurgeEntities 'SearsOrderItem', 'OrderItemID'
	exec PurgeEntities 'ThreeDCartOrderItem', 'OrderItemID'
	exec PurgeEntities 'WalmartOrderItem', 'OrderItemID'
	exec PurgeEntities 'YahooOrderItem', 'OrderItemID'
	exec PurgeEntities 'OrderItem', 'OrderItemID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID'
	exec PurgeEntities 'ObjectReference', 'ObjectID'

	
	/*******************************************************************/
	/* Populate the list of OrderIDs to delete.                        */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	SELECT o.EntityID as 'EntityID'
		INTO    dbo.[EntityIDsToDelete]
		FROM    dbo.[OrderIDsToDelete] o
	ORDER BY o.EntityID
	
	/*******************************************************************/
	/* Delete based on OrderIDs.                                       */
	/*******************************************************************/
	exec PurgeEntities 'AmazonOrderSearch', 'OrderID'
	exec PurgeEntities 'ChannelAdvisorOrderSearch', 'OrderID'
	exec PurgeEntities 'ClickCartProOrderSearch', 'OrderID'
	exec PurgeEntities 'CommerceInterfaceOrderSearch', 'OrderID'
	exec PurgeEntities 'EbayCombinedOrderRelation', 'OrderID'
	exec PurgeEntities 'EbayOrderSearch', 'OrderID'
	exec PurgeEntities 'GrouponOrderSearch', 'OrderID'
	exec PurgeEntities 'JetOrderSearch', 'OrderID'
	exec PurgeEntities 'LemonStandOrderSearch', 'OrderID'
	exec PurgeEntities 'MagentoOrderSearch', 'OrderID'
	exec PurgeEntities 'MarketplaceAdvisorOrderSearch', 'OrderID'
	exec PurgeEntities 'NetworkSolutionsOrderSearch', 'OrderID'
	exec PurgeEntities 'OrderMotionOrderSearch', 'OrderID'
	exec PurgeEntities 'PayPalOrderSearch', 'OrderID'
	exec PurgeEntities 'SearsOrderSearch', 'OrderID'
	exec PurgeEntities 'ShopifyOrderSearch', 'OrderID'
	exec PurgeEntities 'ThreeDCartOrderSearch', 'OrderID'
	exec PurgeEntities 'WalmartOrderSearch', 'OrderID'
	exec PurgeEntities 'YahooOrderSearch', 'OrderID'
	exec PurgeEntities 'AmazonOrder', 'OrderID'
	exec PurgeEntities 'ChannelAdvisorOrder', 'OrderID'
	exec PurgeEntities 'ClickCartProOrder', 'OrderID'
	exec PurgeEntities 'CommerceInterfaceOrder', 'OrderID'
	exec PurgeEntities 'EbayOrder', 'OrderID'
	exec PurgeEntities 'EtsyOrder', 'OrderID'
	exec PurgeEntities 'GenericModuleOrder', 'OrderID'
	exec PurgeEntities 'GrouponOrder', 'OrderID'
	exec PurgeEntities 'JetOrder', 'OrderID'
	exec PurgeEntities 'LemonStandOrder', 'OrderID'
	exec PurgeEntities 'MagentoOrder', 'OrderID'
	exec PurgeEntities 'MarketplaceAdvisorOrder', 'OrderID'
	exec PurgeEntities 'NetworkSolutionsOrder', 'OrderID'
	exec PurgeEntities 'NeweggOrder', 'OrderID'
	exec PurgeEntities 'OrderCharge', 'OrderID'
	exec PurgeEntities 'OrderMotionOrder', 'OrderID'
	exec PurgeEntities 'OrderPaymentDetail', 'OrderID'
	exec PurgeEntities 'OrderSearch', 'OrderID'
	exec PurgeEntities 'PayPalOrder', 'OrderID'
	exec PurgeEntities 'SearsOrder', 'OrderID'
	exec PurgeEntities 'Shipment', 'ShipmentID'
	exec PurgeEntities 'ShopifyOrder', 'OrderID'
	exec PurgeEntities 'ThreeDCartOrder', 'OrderID'
	exec PurgeEntities 'WalmartOrder', 'OrderID'
	exec PurgeEntities 'YahooOrder', 'OrderID'
	exec PurgeEntities 'Note', 'ObjectID'
	exec PurgeEntities 'Order', 'OrderID'
	exec PurgeEntities 'ValidatedAddress', 'ConsumerID'
	exec PurgeEntities 'PrintResult', 'RelatedObjectID'
	exec PurgeEntities 'PrintResult', 'ContextObjectID'
	exec PurgeEntities 'ObjectLabel', 'ObjectID'
	exec PurgeEntities 'ObjectLabel', 'ParentID'
	exec PurgeEntities 'ObjectReference', 'ConsumerID'
	exec PurgeEntities 'ObjectReference', 'ObjectID'
	exec PurgeEntities 'EmailOutboundRelation', 'ObjectID'
	exec PurgeEntities 'EmailOutbound', 'ContextID'
	exec PurgeEntities 'DownloadDetail', 'DownloadedDetailID'


	/*******************************************************************/
	/*						Purge Audits		                       */
	/* Populate the list of AuditIDs to delete.                        */
	/*******************************************************************/
	IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
	BEGIN
		DROP TABLE [EntityIDsToDelete]
	END

	;WITH AuditsToDelete AS
	(
		SELECT a.AuditID as 'EntityID'
			FROM    dbo.[OrderIDsToDelete] o, [Audit] a
			WHERE   o.EntityID = a.ObjectID
		UNION ALL
		SELECT a.AuditID as 'EntityID'
			FROM    dbo.[OrderIDsToDelete] o, [Audit] a, Shipment s
			WHERE   s.ShipmentID = a.ObjectID
			  AND   o.EntityID = s.OrderID
	)
	SELECT EntityID 
		INTO    dbo.[EntityIDsToDelete]
		FROM AuditsToDelete
		ORDER BY EntityID
	
	exec PurgeEntities 'AuditChangeDetail', 'AuditID'
	exec PurgeEntities 'AuditChange', 'AuditID'
	exec PurgeEntities 'Audit', 'AuditID'


	/*************************************/
	/*   Purge Abandoned Resources       */
	/*************************************/

	exec PurgeAbandonedResources @runUntil = null, @olderThan = null


/************************/
/*   COMMIT     */
/************************/
COMMIT

/* Cleanup */

/* Drop id holding tables */
IF EXISTS(SELECT * FROM sys.tables WHERE name = 'OrderIDsToDelete')
BEGIN
	DROP TABLE [OrderIDsToDelete]
END

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'EntityIDsToDelete')
BEGIN
	DROP TABLE [EntityIDsToDelete]
END

/* Enable all triggers */
exec (@EnableAllTriggersSql);

/* Enable all indexes */
exec (@EnableAllIndexesSql);

/* Enable all change tracking */
exec (@EnableAllChangeTrackingSql);

/* Enable all foreign keys */
exec (@EnableAllForeignKeysSql);
