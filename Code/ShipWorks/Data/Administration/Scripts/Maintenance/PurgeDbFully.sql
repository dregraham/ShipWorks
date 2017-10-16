/***********

This script will truncate virtually ALL data!  

Use at your own risk!

This should NOT be run on an active production database.  It is only to be used
on a secondary database that will eventually be used in production after testing.

This script depends on a stored proc and view; be sure to run those scripts first.
- TruncateNonEmptyTable_SupportsDeleteCascade
- EntitiesToDeleteView

You should look at the results of EntitiesToDeleteView to verify that no new tables
have been added that could accidently be truncated.  Ask dev for help if you have
questions.

************/

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ObjectType' AND Object_ID = Object_ID(N'dbo.ObjectReference'))
BEGIN
	ALTER TABLE ObjectReference ADD
		ObjectType  AS ObjectID % 1000
END
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ConsumerType' AND Object_ID = Object_ID(N'dbo.ObjectReference'))
BEGIN
	ALTER TABLE ObjectReference ADD
		ConsumerType  AS ConsumerID % 1000
END
GO

declare @DatabaseName nvarchar(50) = DB_NAME()

IF OBJECT_ID('TruncateNonEmptyTable_SupportsDeleteCascade') IS NULL
	THROW 50000, 'You must create the TruncateNonEmptyTable_SupportsDeleteCascade first!', 1;
IF LEN(@DatabaseName) = 0
	THROW 50000, 'You must set the @DatabaseName variable first!', 1;
IF OBJECT_ID('EntitiesToDeleteView') IS NULL
	THROW 50000, 'You must create the EntitiesToDeleteView first!', 1;
ELSE
BEGIN

	
	exec sp_MSforeachtable 'ALTER TABLE ? DISABLE TRIGGER ALL';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OtherShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EndiciaShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'AmazonShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShipmentCustomsItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'UspsShipment';
	IF OBJECT_ID('ShipmentReturnItem') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShipmentReturnItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'BestRateShipment';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'InsurancePolicy';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'WorldShipGoods';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'WorldShipPackage';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'UpsPackage';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FedExPackage';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'iParcelPackage';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FedExShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'iParcelShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OnTracShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'WorldShipShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'PostalShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'UpsShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Shipment';

	IF OBJECT_ID('OrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderSearch';
	IF OBJECT_ID('AmazonOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'AmazonOrderSearch';
	IF OBJECT_ID('ChannelAdvisorOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ChannelAdvisorOrderSearch';
	IF OBJECT_ID('ClickCartProOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ClickCartProOrderSearch';
	IF OBJECT_ID('CommerceInterfaceOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'CommerceInterfaceOrderSearch';
	IF OBJECT_ID('EbayOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'EbayOrderSearch';
	IF OBJECT_ID('GrouponOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'GrouponOrderSearch';
	IF OBJECT_ID('JetOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'JetOrderSearch';
	IF OBJECT_ID('LemonStandOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'LemonStandOrderSearch';
	IF OBJECT_ID('MagentoOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'MagentoOrderSearch';
	IF OBJECT_ID('MarketplaceAdvisorOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'MarketplaceAdvisorOrderSearch';
	IF OBJECT_ID('NetworkSolutionsOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'NetworkSolutionsOrderSearch';
	IF OBJECT_ID('OrderMotionOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderMotionOrderSearch';
	IF OBJECT_ID('PayPalOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'PayPalOrderSearch';
	IF OBJECT_ID('ProStoresOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ProStoresOrderSearch';
	IF OBJECT_ID('SearsOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'SearsOrderSearch';
	IF OBJECT_ID('ShopifyOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShopifyOrderSearch';
	IF OBJECT_ID('ThreeDCartOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'ThreeDCartOrderSearch';
	IF OBJECT_ID('WalmartOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'WalmartOrderSearch';
	IF OBJECT_ID('YahooOrderSearch') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'YahooOrderSearch';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'AmazonOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'BigCommerceOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'BuyDotComOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ChannelAdvisorOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EbayCombinedOrderRelation';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EbayOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EtsyOrder';
	IF OBJECT_ID('EtsyOrderItem') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'EtsyOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'GrouponOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'InfopiaOrderItem';
	IF OBJECT_ID('JetOrderItem') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'JetOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'LemonStandOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'MivaOrderItemAttribute';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'NeweggOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'NeweggOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderCharge';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderMotionStore';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderPaymentDetail';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'SearsOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShopifyOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ThreeDCartOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'WalmartOrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'YahooOrderItem';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'AmazonOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ChannelAdvisorOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ClickCartProOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'CommerceInterfaceOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EbayOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'GrouponOrder';
	IF OBJECT_ID('JetOrder') IS NOT NULL exec TruncateNonEmptyTable_SupportsDeleteCascade 'JetOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'LemonStandOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'MagentoOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'MarketplaceAdvisorOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'NetworkSolutionsOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderItemAttribute';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderMotionOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'PayPalOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ProStoresOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'SearsOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShopifyOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ThreeDCartOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'WalmartOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'YahooOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'YahooProduct';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'OrderItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Order';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'AuditChangeDetail';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'AuditChange';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Audit';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeContentDetail';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeContentDirty';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeRootDirty';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateCheckpoint';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateCustomer';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdatePending';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeUpdateShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeContentDirty';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateCheckpoint';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateItem';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateCustomer';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateOrder';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdatePending';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'QuickFilterNodeUpdateShipment';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FilterNodeContentDetail';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Dirty';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ActionQueueSelection';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ActionQueueStep';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ActionQueue';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'DownloadDetail';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Download';

    exec TruncateNonEmptyTable_SupportsDeleteCascade 'EmailOutboundRelation';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EmailOutbound';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'EndiciaScanForm';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'FedExEndOfDayClose';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Note';

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ShipSenseKnowledgeBase';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'UspsScanForm';
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ValidatedAddress';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ScanFormBatch';
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Customer';
	
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'PrintResult';


/************* START Delete from ObjectLabel table ******************************************/
/* Since this table contains rows that need to be kept, we can't blindly do a truncate.  So */
/* instead we will copy the rows to keep into a temp table, truncate the actual table, then */
/* copy the temp table rows back into the actual table.                                     */

	declare @ObjectLabelIdentitySeed bigint
	select @ObjectLabelIdentitySeed = max(ObjectID) + 1000 from [ObjectLabel]

	IF OBJECT_ID('ObjectLabel_Temp') IS NOT NULL
	BEGIN
		DROP TABLE ObjectLabel_Temp
	END

	select [ObjectID], [RowVersion], [ObjectType], [ParentID], [Label], [IsDeleted] -- 73
		into ObjectLabel_Temp
		from [ObjectLabel] 
		where ObjectType not in (SELECT EntitySeed from EntitiesToDeleteView)

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ObjectLabel';

	insert into [ObjectLabel] ([ObjectID], [ObjectType], [ParentID], [Label], [IsDeleted])
		select [ObjectID], [ObjectType], [ParentID], [Label], [IsDeleted]
		from ObjectLabel_Temp
	

/************* END Delete from ObjectLabel table ********************************************/



/************* START Delete from Resource table *********************************************/
/* Since this table contains rows that need to be kept, we can't blindly do a truncate.  So */
/* instead we will copy the rows to keep into a temp table, truncate the actual table, then */
/* copy the temp table rows back into the actual table.                                     */

	declare @ResourceIdentitySeed bigint
	select @ResourceIdentitySeed = max(ResourceID)+ 1000 from [Resource]

	IF OBJECT_ID('Resource_Temp') IS NOT NULL
	BEGIN
		DROP TABLE Resource_Temp
	END

	select [ResourceID], [Data], [Checksum], [Compressed], [Filename]
		into Resource_Temp
		from [Resource] 
		where ResourceID in 
			(
				select distinct ObjectID from ObjectReference 
				where  ConsumerType not in (SELECT EntitySeed from EntitiesToDeleteView)
			)
	
	exec TruncateNonEmptyTable_SupportsDeleteCascade 'Resource';

	SET IDENTITY_INSERT [Resource] ON
	
	insert into [Resource] ([ResourceID], [Data], [Checksum], [Compressed], [Filename])
		select [ResourceID], [Data], [Checksum], [Compressed], [Filename]
		from Resource_Temp

	DBCC CHECKIDENT ('Resource', RESEED, @ResourceIdentitySeed);
	
	SET IDENTITY_INSERT [Resource] OFF

/************* END Delete from Resource table ***********************************************/

	

/************* START Delete from ObjectReference table **************************************/
/* Since this table contains rows that need to be kept, we can't blindly do a truncate.  So */
/* instead we will copy the rows to keep into a temp table, truncate the actual table, then */
/* copy the temp table rows back into the actual table.                                     */

	declare @ObjectReferenceIdentitySeed bigint
	select @ObjectReferenceIdentitySeed = max(ObjectReferenceID)+ 1000 from [ObjectReference]

	IF OBJECT_ID('ObjectReference_Temp') IS NOT NULL
	BEGIN
		DROP TABLE ObjectReference_Temp
	END

	select [ObjectReferenceID], [ConsumerID], [ReferenceKey], [ObjectID], [Reason], [ObjectType], [ConsumerType]
		into ObjectReference_Temp
		from [ObjectReference] 
		where ConsumerType not in (SELECT EntitySeed from EntitiesToDeleteView)

	exec TruncateNonEmptyTable_SupportsDeleteCascade 'ObjectReference';

	SET IDENTITY_INSERT [ObjectReference] ON
	
	insert into [ObjectReference] ([ObjectReferenceID], [ConsumerID], [ReferenceKey], [ObjectID], [Reason])
		select [ObjectReferenceID], [ConsumerID], [ReferenceKey], [ObjectID], [Reason]
		from ObjectReference_Temp

	DBCC CHECKIDENT ('ObjectReference', RESEED, @ObjectReferenceIdentitySeed);
	
	SET IDENTITY_INSERT [ObjectReference] OFF
	  
/************* END Delete from ObjectReference table ******************************************/

/************* We need to reset the root filter counts to 0 ***********************************/
	update FilterNodeContent set Count = 0 where FilterNodeContentID = -26
	update FilterNodeContent set Count = 0 where FilterNodeContentID = -28

	exec sp_MSforeachtable 'ALTER TABLE ? ENABLE TRIGGER ALL';

	DBCC SHRINKFILE (N'ShipWorks_Data' , 0, TRUNCATEONLY)

	DBCC SHRINKDATABASE(@DatabaseName)

END

