PRINT N'Renaming Indexes'
GO
/* Rename indexes that don't match any of the regular formats */
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID', N'IX_SWDefault_EmailOutboundRelation_EmailOutboundIDRelationTypeObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID', N'IX_SWDefault_EmailOutboundRelation_ObjectIDRelationTypeEmailOutboundID', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_RelationTypeObject' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]'))
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_RelationTypeObject', N'IX_SWDefault_EmailOutboundRelation_RelationTypeObject', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterChild_ParentFilterID' AND object_id = OBJECT_ID('[dbo].[FilterSequence]'))
	EXEC sp_rename N'dbo.FilterSequence.IX_FilterChild_ParentFilterID', N'IX_SWDefault_FilterSequence_FilterChild_ParentFilterID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineCustomerID' AND object_id = OBJECT_ID('[dbo].[Order]'))
	EXEC sp_rename N'dbo.Order.IX_OnlineCustomerID', N'IX_SWDefault_Order_OnlineCustomerID', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineLastModified_StoreID_IsManual' AND object_id = OBJECT_ID('[dbo].[Order]'))
	EXEC sp_rename N'dbo.Order.IX_OnlineLastModified_StoreID_IsManual', N'IX_SWDefault_Order_OnlineLastModified_StoreID_IsManual', N'INDEX';
	
IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UK_Computer_Identifier' AND object_id = OBJECT_ID('[dbo].[Computer]'))
	EXEC sp_rename N'dbo.Computer.UK_Computer_Identifier', N'IX_SWDefault_Computer_Identifier', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ActionQueue_ActionQueueType' AND object_id = OBJECT_ID('[dbo].[ActionQueue]')) 
	EXEC sp_rename N'dbo.ActionQueue.IX_ActionQueue_ActionQueueType', N'IX_SWDefault_ActionQueue_ActionQueueType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ActionQueue_ContextLock' AND object_id = OBJECT_ID('[dbo].[ActionQueue]')) 
	EXEC sp_rename N'dbo.ActionQueue.IX_ActionQueue_ContextLock', N'IX_SWDefault_ActionQueue_ContextLock', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ActionQueue_Search' AND object_id = OBJECT_ID('[dbo].[ActionQueue]')) 
	EXEC sp_rename N'dbo.ActionQueue.IX_ActionQueue_Search', N'IX_SWDefault_ActionQueue_Search', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ActionQueueSelection_ActionQueueID' AND object_id = OBJECT_ID('[dbo].[ActionQueueSelection]')) 
	EXEC sp_rename N'dbo.ActionQueueSelection.IX_ActionQueueSelection_ActionQueueID', N'IX_SWDefault_ActionQueueSelection_ActionQueueID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ActionQueueStep_ActionQueue' AND object_id = OBJECT_ID('[dbo].[ActionQueueStep]')) 
	EXEC sp_rename N'dbo.ActionQueueStep.IX_ActionQueueStep_ActionQueue', N'IX_SWDefault_ActionQueueStep_ActionQueue', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_AmazonOrderID' AND object_id = OBJECT_ID('[dbo].[AmazonOrder]')) 
	EXEC sp_rename N'dbo.AmazonOrder.IX_Auto_AmazonOrderID', N'IX_SWDefault_AmazonOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_EarliestExpectedDeliveryDate' AND object_id = OBJECT_ID('[dbo].[AmazonOrder]')) 
	EXEC sp_rename N'dbo.AmazonOrder.IX_Auto_EarliestExpectedDeliveryDate', N'IX_SWDefault_EarliestExpectedDeliveryDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_FulfillmentChannel' AND object_id = OBJECT_ID('[dbo].[AmazonOrder]')) 
	EXEC sp_rename N'dbo.AmazonOrder.IX_Auto_FulfillmentChannel', N'IX_SWDefault_FulfillmentChannel', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_IsPrime' AND object_id = OBJECT_ID('[dbo].[AmazonOrder]')) 
	EXEC sp_rename N'dbo.AmazonOrder.IX_Auto_IsPrime', N'IX_SWDefault_IsPrime', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_LatestExpectedDeliveryDate' AND object_id = OBJECT_ID('[dbo].[AmazonOrder]')) 
	EXEC sp_rename N'dbo.AmazonOrder.IX_Auto_LatestExpectedDeliveryDate', N'IX_SWDefault_LatestExpectedDeliveryDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AmazonOrderSearch_AmazonOrderID_OrderID' AND object_id = OBJECT_ID('[dbo].[AmazonOrderSearch]')) 
	EXEC sp_rename N'dbo.AmazonOrderSearch.IX_AmazonOrderSearch_AmazonOrderID_OrderID', N'IX_SWDefault_AmazonOrderSearch_AmazonOrderID_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AmazonServiceType_ApiValue' AND object_id = OBJECT_ID('[dbo].[AmazonServiceType]')) 
	EXEC sp_rename N'dbo.AmazonServiceType.IX_AmazonServiceType_ApiValue', N'IX_SWDefault_AmazonServiceType_ApiValue', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AsendiaShipment_Service' AND object_id = OBJECT_ID('[dbo].[AsendiaShipment]')) 
	EXEC sp_rename N'dbo.AsendiaShipment.IX_AsendiaShipment_Service', N'IX_SWDefault_AsendiaShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Audit_Action' AND object_id = OBJECT_ID('[dbo].[Audit]')) 
	EXEC sp_rename N'dbo.Audit.IX_Audit_Action', N'IX_SWDefault_Audit_Action', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Audit_Date' AND object_id = OBJECT_ID('[dbo].[Audit]')) 
	EXEC sp_rename N'dbo.Audit.IX_Audit_Date', N'IX_SWDefault_Audit_Date', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Audit_ObjectIDDate' AND object_id = OBJECT_ID('[dbo].[Audit]')) 
	EXEC sp_rename N'dbo.Audit.IX_Audit_ObjectIDDate', N'IX_SWDefault_Audit_ObjectIDDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Audit_TransactionID' AND object_id = OBJECT_ID('[dbo].[Audit]')) 
	EXEC sp_rename N'dbo.Audit.IX_Audit_TransactionID', N'IX_SWDefault_Audit_TransactionID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AuditChange_AuditID' AND object_id = OBJECT_ID('[dbo].[AuditChange]')) 
	EXEC sp_rename N'dbo.AuditChange.IX_AuditChange_AuditID', N'IX_SWDefault_AuditChange_AuditID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AuditChangeDetail_AuditChangeID' AND object_id = OBJECT_ID('[dbo].[AuditChangeDetail]')) 
	EXEC sp_rename N'dbo.AuditChangeDetail.IX_AuditChangeDetail_AuditChangeID', N'IX_SWDefault_AuditChangeDetail_AuditChangeID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AuditChangeDetail_AuditID' AND object_id = OBJECT_ID('[dbo].[AuditChangeDetail]')) 
	EXEC sp_rename N'dbo.AuditChangeDetail.IX_AuditChangeDetail_AuditID', N'IX_SWDefault_AuditChangeDetail_AuditID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_AuditChangeDetail_VariantNew' AND object_id = OBJECT_ID('[dbo].[AuditChangeDetail]')) 
	EXEC sp_rename N'dbo.AuditChangeDetail.IX_AuditChangeDetail_VariantNew', N'IX_SWDefault_AuditChangeDetail_VariantNew', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrder]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrder.IX_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID', N'IX_SWDefault_ChannelAdvisorOrder_CustomerOrderIdentifier_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrder_IsPrime' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrder]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrder.IX_ChannelAdvisorOrder_IsPrime', N'IX_SWDefault_ChannelAdvisorOrder_IsPrime', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrder_OnlineShippingStatus' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrder]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrder.IX_ChannelAdvisorOrder_OnlineShippingStatus', N'IX_SWDefault_ChannelAdvisorOrder_OnlineShippingStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_Classification' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_Classification', N'IX_SWDefault_ChannelAdvisorOrderItem_Classification', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_DistributionCenter' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_DistributionCenter', N'IX_SWDefault_ChannelAdvisorOrderItem_DistributionCenter', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_DistributionCenterID' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_DistributionCenterID', N'IX_SWDefault_ChannelAdvisorOrderItem_DistributionCenterID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_MarketplaceBuyerID' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_MarketplaceBuyerID', N'IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceBuyerID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_MarketPlaceName' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_MarketPlaceName', N'IX_SWDefault_ChannelAdvisorOrderItem_MarketPlaceName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_MarketplaceSalesID' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_MarketplaceSalesID', N'IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceSalesID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderItem_MarketplaceStoreName' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderItem]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderItem.IX_ChannelAdvisorOrderItem_MarketplaceStoreName', N'IX_SWDefault_ChannelAdvisorOrderItem_MarketplaceStoreName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID' AND object_id = OBJECT_ID('[dbo].[ChannelAdvisorOrderSearch]')) 
	EXEC sp_rename N'dbo.ChannelAdvisorOrderSearch.IX_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID', N'IX_SWDefault_ChannelAdvisorOrderSearch_CustomOrderIdentifier_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ClickCartProOrderSearch_ClickCartProOrderID' AND object_id = OBJECT_ID('[dbo].[ClickCartProOrderSearch]')) 
	EXEC sp_rename N'dbo.ClickCartProOrderSearch.IX_ClickCartProOrderSearch_ClickCartProOrderID', N'IX_SWDefault_ClickCartProOrderSearch_ClickCartProOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber' AND object_id = OBJECT_ID('[dbo].[CommerceInterfaceOrderSearch]')) 
	EXEC sp_rename N'dbo.CommerceInterfaceOrderSearch.IX_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber', N'IX_SWDefault_CommerceInterfaceOrderSearch_CommerceInterfaceOrderNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UK_Computer_Identifier' AND object_id = OBJECT_ID('[dbo].[Computer]')) 
	EXEC sp_rename N'dbo.Computer.UK_Computer_Identifier', N'UK_Computer_Identifier', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillCompany' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillCompany', N'IX_SWDefault_BillCompany', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillCountryCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillCountryCode', N'IX_SWDefault_BillCountryCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillEmail' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillEmail', N'IX_SWDefault_BillEmail', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillFirstName' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillFirstName', N'IX_SWDefault_BillFirstName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillLastName' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillLastName', N'IX_SWDefault_BillLastName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillPostalCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillPostalCode', N'IX_SWDefault_BillPostalCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillStateProvCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_BillStateProvCode', N'IX_SWDefault_BillStateProvCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupNoteCount' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_RollupNoteCount', N'IX_SWDefault_RollupNoteCount', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupOrderCount' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_RollupOrderCount', N'IX_SWDefault_RollupOrderCount', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupOrderTotal' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_RollupOrderTotal', N'IX_SWDefault_RollupOrderTotal', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipCompany' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipCompany', N'IX_SWDefault_ShipCompany', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipCountryCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipCountryCode', N'IX_SWDefault_ShipCountryCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipEmail' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipEmail', N'IX_SWDefault_ShipEmail', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipLastName' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipLastName', N'IX_SWDefault_ShipLastName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipPostalCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipPostalCode', N'IX_SWDefault_ShipPostalCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipStateProvCode' AND object_id = OBJECT_ID('[dbo].[Customer]')) 
	EXEC sp_rename N'dbo.Customer.IX_Auto_ShipStateProvCode', N'IX_SWDefault_ShipStateProvCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DhlExpressShipment_Service' AND object_id = OBJECT_ID('[dbo].[DhlExpressShipment]')) 
	EXEC sp_rename N'dbo.DhlExpressShipment.IX_DhlExpressShipment_Service', N'IX_SWDefault_DhlExpressShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DimensionsProfile_Name' AND object_id = OBJECT_ID('[dbo].[DimensionsProfile]')) 
	EXEC sp_rename N'dbo.DimensionsProfile.IX_DimensionsProfile_Name', N'IX_SWDefault_DimensionsProfile_Name', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadLog_StoreID_Ended' AND object_id = OBJECT_ID('[dbo].[Download]')) 
	EXEC sp_rename N'dbo.Download.IX_DownloadLog_StoreID_Ended', N'IX_SWDefault_DownloadLog_StoreID_Ended', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadDetail_BigIntIndex' AND object_id = OBJECT_ID('[dbo].[DownloadDetail]')) 
	EXEC sp_rename N'dbo.DownloadDetail.IX_DownloadDetail_BigIntIndex', N'IX_SWDefault_DownloadDetail_BigIntIndex', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadDetail_DownloadID' AND object_id = OBJECT_ID('[dbo].[DownloadDetail]')) 
	EXEC sp_rename N'dbo.DownloadDetail.IX_DownloadDetail_DownloadID', N'IX_SWDefault_DownloadDetail_DownloadID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadDetail_OrderID' AND object_id = OBJECT_ID('[dbo].[DownloadDetail]')) 
	EXEC sp_rename N'dbo.DownloadDetail.IX_DownloadDetail_OrderID', N'IX_SWDefault_DownloadDetail_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadDetail_OrderNumber' AND object_id = OBJECT_ID('[dbo].[DownloadDetail]')) 
	EXEC sp_rename N'dbo.DownloadDetail.IX_DownloadDetail_OrderNumber', N'IX_SWDefault_DownloadDetail_OrderNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DownloadDetail_String' AND object_id = OBJECT_ID('[dbo].[DownloadDetail]')) 
	EXEC sp_rename N'dbo.DownloadDetail.IX_DownloadDetail_String', N'IX_SWDefault_DownloadDetail_String', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayCombinedOrderRelation' AND object_id = OBJECT_ID('[dbo].[EbayCombinedOrderRelation]')) 
	EXEC sp_rename N'dbo.EbayCombinedOrderRelation.IX_EbayCombinedOrderRelation', N'IX_SWDefault_EbayCombinedOrderRelation', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrder_EbayBuyerID' AND object_id = OBJECT_ID('[dbo].[EbayOrder]')) 
	EXEC sp_rename N'dbo.EbayOrder.IX_EbayOrder_EbayBuyerID', N'IX_SWDefault_EbayOrder_EbayBuyerID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrder_GuaranteedDelivery' AND object_id = OBJECT_ID('[dbo].[EbayOrder]')) 
	EXEC sp_rename N'dbo.EbayOrder.IX_EbayOrder_GuaranteedDelivery', N'IX_SWDefault_EbayOrder_GuaranteedDelivery', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible' AND object_id = OBJECT_ID('[dbo].[EbayOrder]')) 
	EXEC sp_rename N'dbo.EbayOrder.IX_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible', N'IX_SWDefault_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrderItem_OrderID' AND object_id = OBJECT_ID('[dbo].[EbayOrderItem]')) 
	EXEC sp_rename N'dbo.EbayOrderItem.IX_EbayOrderItem_OrderID', N'IX_SWDefault_EbayOrderItem_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrderItem_SellingManagerRecord_OrderID' AND object_id = OBJECT_ID('[dbo].[EbayOrderItem]')) 
	EXEC sp_rename N'dbo.EbayOrderItem.IX_EbayOrderItem_SellingManagerRecord_OrderID', N'IX_SWDefault_EbayOrderItem_SellingManagerRecord_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='SW_EbayOrderItem_EbayItemID_EbayTransactionID' AND object_id = OBJECT_ID('[dbo].[EbayOrderItem]')) 
	EXEC sp_rename N'dbo.EbayOrderItem.SW_EbayOrderItem_EbayItemID_EbayTransactionID', N'IX_SWDefault_EbayOrderItem_EbayItemID_EbayTransactionID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='SW_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID' AND object_id = OBJECT_ID('[dbo].[EbayOrderItem]')) 
	EXEC sp_rename N'dbo.EbayOrderItem.SW_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID', N'IX_SWDefault_EbayOrderItem_EffectiveCheckoutStatus_EbayOrderItemID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EbayOrderSearch_EbayBuyerID_OrderID' AND object_id = OBJECT_ID('[dbo].[EbayOrderSearch]')) 
	EXEC sp_rename N'dbo.EbayOrderSearch.IX_EbayOrderSearch_EbayBuyerID_OrderID', N'IX_SWDefault_EbayOrderSearch_EbayBuyerID_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound' AND object_id = OBJECT_ID('[dbo].[EmailOutbound]')) 
	EXEC sp_rename N'dbo.EmailOutbound.IX_EmailOutbound', N'IX_SWDefault_EmailOutbound', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]')) 
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_EmailOutboundIDRelationTypeObjectID', N'IX_SWDefault_EmailOutbound_EmailOutboundIDRelationTypeObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]')) 
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_ObjectIDRelationTypeEmailOutboundID', N'IX_SWDefault_EmailOutbound_ObjectIDRelationTypeEmailOutboundID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EmailOutbound_RelationTypeObject' AND object_id = OBJECT_ID('[dbo].[EmailOutboundRelation]')) 
	EXEC sp_rename N'dbo.EmailOutboundRelation.IX_EmailOutbound_RelationTypeObject', N'IX_SWDefault_EmailOutbound_RelationTypeObject', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FedExEndOfDayClose_CloseDate' AND object_id = OBJECT_ID('[dbo].[FedExEndOfDayClose]')) 
	EXEC sp_rename N'dbo.FedExEndOfDayClose.IX_FedExEndOfDayClose_CloseDate', N'IX_SWDefault_FedExEndOfDayClose_CloseDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FedExPackage_ShipmentID' AND object_id = OBJECT_ID('[dbo].[FedExPackage]')) 
	EXEC sp_rename N'dbo.FedExPackage.IX_FedExPackage_ShipmentID', N'IX_SWDefault_FedExPackage_ShipmentID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FedExShipment_PackagingType' AND object_id = OBJECT_ID('[dbo].[FedExShipment]')) 
	EXEC sp_rename N'dbo.FedExShipment.IX_FedExShipment_PackagingType', N'IX_SWDefault_FedExShipment_PackagingType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FedExShipment_Service' AND object_id = OBJECT_ID('[dbo].[FedExShipment]')) 
	EXEC sp_rename N'dbo.FedExShipment.IX_FedExShipment_Service', N'IX_SWDefault_FedExShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Filter_IsFolder' AND object_id = OBJECT_ID('[dbo].[Filter]')) 
	EXEC sp_rename N'dbo.Filter.IX_Filter_IsFolder', N'IX_SWDefault_Filter_IsFolder', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Filter_State' AND object_id = OBJECT_ID('[dbo].[Filter]')) 
	EXEC sp_rename N'dbo.Filter.IX_Filter_State', N'IX_SWDefault_Filter_State', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterLayout' AND object_id = OBJECT_ID('[dbo].[FilterLayout]')) 
	EXEC sp_rename N'dbo.FilterLayout.IX_FilterLayout', N'IX_SWDefault_FilterLayout', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose' AND object_id = OBJECT_ID('[dbo].[FilterNode]')) 
	EXEC sp_rename N'dbo.FilterNode.IX_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose', N'IX_SWDefault_FilterNode_FilterSequenceID_FilterNodeContentID_Purpose', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterNode_ParentFilterNodeID' AND object_id = OBJECT_ID('[dbo].[FilterNode]')) 
	EXEC sp_rename N'dbo.FilterNode.IX_FilterNode_ParentFilterNodeID', N'IX_SWDefault_FilterNode_ParentFilterNodeID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterNodeColumnSettings' AND object_id = OBJECT_ID('[dbo].[FilterNodeColumnSettings]')) 
	EXEC sp_rename N'dbo.FilterNodeColumnSettings.IX_FilterNodeColumnSettings', N'IX_SWDefault_FilterNodeColumnSettings', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterNodeContent_Status' AND object_id = OBJECT_ID('[dbo].[FilterNodeContent]')) 
	EXEC sp_rename N'dbo.FilterNodeContent.IX_FilterNodeContent_Status', N'IX_SWDefault_FilterNodeContent_Status', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterNodeContentDirty_FilterNodeContentDirtyID' AND object_id = OBJECT_ID('[dbo].[FilterNodeContentDirty]')) 
	EXEC sp_rename N'dbo.FilterNodeContentDirty.IX_FilterNodeContentDirty_FilterNodeContentDirtyID', N'IX_SWDefault_FilterNodeContentDirty_FilterNodeContentDirtyID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterChild_ParentFilterID' AND object_id = OBJECT_ID('[dbo].[FilterSequence]')) 
	EXEC sp_rename N'dbo.FilterSequence.IX_FilterChild_ParentFilterID', N'IX_SWDefault_FilterChild_ParentFilterID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FilterSequence_FilterID' AND object_id = OBJECT_ID('[dbo].[FilterSequence]')) 
	EXEC sp_rename N'dbo.FilterSequence.IX_FilterSequence_FilterID', N'IX_SWDefault_FilterSequence_FilterID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_GridColumnDisplay' AND object_id = OBJECT_ID('[dbo].[GridColumnFormat]')) 
	EXEC sp_rename N'dbo.GridColumnFormat.IX_GridColumnDisplay', N'IX_SWDefault_GridColumnDisplay', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_GridColumnPosition_GridColumnLayoutIdColumn' AND object_id = OBJECT_ID('[dbo].[GridColumnPosition]')) 
	EXEC sp_rename N'dbo.GridColumnPosition.IX_GridColumnPosition_GridColumnLayoutIdColumn', N'IX_SWDefault_GridColumnPosition_GridColumnLayoutIdColumn', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_GrouponOrderID' AND object_id = OBJECT_ID('[dbo].[GrouponOrder]')) 
	EXEC sp_rename N'dbo.GrouponOrder.IX_Auto_GrouponOrderID', N'IX_SWDefault_GrouponOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_GrouponOrder_ParentOrderID' AND object_id = OBJECT_ID('[dbo].[GrouponOrder]')) 
	EXEC sp_rename N'dbo.GrouponOrder.IX_GrouponOrder_ParentOrderID', N'IX_SWDefault_GrouponOrder_ParentOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_GrouponOrderSearch_GrouponOrderID_OrderID' AND object_id = OBJECT_ID('[dbo].[GrouponOrderSearch]')) 
	EXEC sp_rename N'dbo.GrouponOrderSearch.IX_GrouponOrderSearch_GrouponOrderID_OrderID', N'IX_SWDefault_GrouponOrderSearch_GrouponOrderID_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_GrouponOrderSearch_ParentOrderID_OrderID' AND object_id = OBJECT_ID('[dbo].[GrouponOrderSearch]')) 
	EXEC sp_rename N'dbo.GrouponOrderSearch.IX_GrouponOrderSearch_ParentOrderID_OrderID', N'IX_SWDefault_GrouponOrderSearch_ParentOrderID_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_iParcelPackage_ShipmentID' AND object_id = OBJECT_ID('[dbo].[iParcelPackage]')) 
	EXEC sp_rename N'dbo.iParcelPackage.IX_iParcelPackage_ShipmentID', N'IX_SWDefault_iParcelPackage_ShipmentID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_IParcelShipment_Service' AND object_id = OBJECT_ID('[dbo].[iParcelShipment]')) 
	EXEC sp_rename N'dbo.iParcelShipment.IX_IParcelShipment_Service', N'IX_SWDefault_IParcelShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_JetOrderSearch_JetOrderID' AND object_id = OBJECT_ID('[dbo].[JetOrderSearch]')) 
	EXEC sp_rename N'dbo.JetOrderSearch.IX_JetOrderSearch_JetOrderID', N'IX_SWDefault_JetOrderSearch_JetOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_LabelSheet_Name' AND object_id = OBJECT_ID('[dbo].[LabelSheet]')) 
	EXEC sp_rename N'dbo.LabelSheet.IX_LabelSheet_Name', N'IX_SWDefault_LabelSheet_Name', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_LemonStandOrderID' AND object_id = OBJECT_ID('[dbo].[LemonStandOrder]')) 
	EXEC sp_rename N'dbo.LemonStandOrder.IX_Auto_LemonStandOrderID', N'IX_SWDefault_LemonStandOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_LemonStandOrderSearch_LemonStandOrderID_OrderID' AND object_id = OBJECT_ID('[dbo].[LemonStandOrderSearch]')) 
	EXEC sp_rename N'dbo.LemonStandOrderSearch.IX_LemonStandOrderSearch_LemonStandOrderID_OrderID', N'IX_SWDefault_LemonStandOrderSearch_LemonStandOrderID_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_MagentoOrderSearch_MagentoOrderID' AND object_id = OBJECT_ID('[dbo].[MagentoOrderSearch]')) 
	EXEC sp_rename N'dbo.MagentoOrderSearch.IX_MagentoOrderSearch_MagentoOrderID', N'IX_SWDefault_MagentoOrderSearch_MagentoOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderNote_ObjectID' AND object_id = OBJECT_ID('[dbo].[Note]')) 
	EXEC sp_rename N'dbo.Note.IX_OrderNote_ObjectID', N'IX_SWDefault_OrderNote_ObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ObjectLabel_ObjectTypeIsDeleted' AND object_id = OBJECT_ID('[dbo].[ObjectLabel]')) 
	EXEC sp_rename N'dbo.ObjectLabel.IX_ObjectLabel_ObjectTypeIsDeleted', N'IX_SWDefault_ObjectLabel_ObjectTypeIsDeleted', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ObjectReference_ConsumerIDReferenceKey' AND object_id = OBJECT_ID('[dbo].[ObjectReference]')) 
	EXEC sp_rename N'dbo.ObjectReference.IX_ObjectReference_ConsumerIDReferenceKey', N'IX_SWDefault_ObjectReference_ConsumerIDReferenceKey', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ObjectReference_ObjectID' AND object_id = OBJECT_ID('[dbo].[ObjectReference]')) 
	EXEC sp_rename N'dbo.ObjectReference.IX_ObjectReference_ObjectID', N'IX_SWDefault_ObjectReference_ObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnTracShipment_PackagingType' AND object_id = OBJECT_ID('[dbo].[OnTracShipment]')) 
	EXEC sp_rename N'dbo.OnTracShipment.IX_OnTracShipment_PackagingType', N'IX_SWDefault_OnTracShipment_PackagingType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnTracShipment_Service' AND object_id = OBJECT_ID('[dbo].[OnTracShipment]')) 
	EXEC sp_rename N'dbo.OnTracShipment.IX_OnTracShipment_Service', N'IX_SWDefault_OnTracShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillCompany' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillCompany', N'IX_SWDefault_BillCompany', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillCountryCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillCountryCode', N'IX_SWDefault_BillCountryCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillEmail' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillEmail', N'IX_SWDefault_BillEmail', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillFirstName' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillFirstName', N'IX_SWDefault_BillFirstName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillLastName' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillLastName', N'IX_SWDefault_BillLastName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillPostalCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillPostalCode', N'IX_SWDefault_BillPostalCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_BillStateProvCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_BillStateProvCode', N'IX_SWDefault_BillStateProvCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_CustomerID' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_CustomerID', N'IX_SWDefault_CustomerID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_LocalStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_LocalStatus', N'IX_SWDefault_LocalStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_OnlineStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_OnlineStatus', N'IX_SWDefault_OnlineStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_OrderDate' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_OrderDate', N'IX_SWDefault_OrderDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_OrderNumber' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_OrderNumber', N'IX_SWDefault_OrderNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_OrderNumberComplete' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_OrderNumberComplete', N'IX_SWDefault_OrderNumberComplete', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_OrderTotal' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_OrderTotal', N'IX_SWDefault_OrderTotal', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RequestedShipping' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RequestedShipping', N'IX_SWDefault_RequestedShipping', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupItemCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RollupItemCode', N'IX_SWDefault_RollupItemCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupItemCount' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RollupItemCount', N'IX_SWDefault_RollupItemCount', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupItemName' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RollupItemName', N'IX_SWDefault_RollupItemName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupItemSKU' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RollupItemSKU', N'IX_SWDefault_RollupItemSKU', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RollupNoteCount' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_RollupNoteCount', N'IX_SWDefault_RollupNoteCount', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipCompany' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipCompany', N'IX_SWDefault_ShipCompany', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipCountryCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipCountryCode', N'IX_SWDefault_ShipCountryCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipEmail' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipEmail', N'IX_SWDefault_ShipEmail', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipFirstName' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipFirstName', N'IX_SWDefault_ShipFirstName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipLastName' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipLastName', N'IX_SWDefault_ShipLastName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipPostalCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipPostalCode', N'IX_SWDefault_ShipPostalCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipSenseHashKey' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipSenseHashKey', N'IX_SWDefault_ShipSenseHashKey', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipSenseRecognitionStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipSenseRecognitionStatus', N'IX_SWDefault_ShipSenseRecognitionStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_ShipStateProvCode' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Auto_ShipStateProvCode', N'IX_SWDefault_ShipStateProvCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineCustomerID' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_OnlineCustomerID', N'IX_SWDefault_OnlineCustomerID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OnlineLastModified_StoreID_IsManual' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_OnlineLastModified_StoreID_IsManual', N'IX_SWDefault_OnlineLastModified_StoreID_IsManual', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_BillAddressValidationStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_BillAddressValidationStatus', N'IX_SWDefault_Order_BillAddressValidationStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_BillMilitaryAddress' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_BillMilitaryAddress', N'IX_SWDefault_Order_BillMilitaryAddress', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_BillPOBox' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_BillPOBox', N'IX_SWDefault_Order_BillPOBox', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_BillResidentialStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_BillResidentialStatus', N'IX_SWDefault_Order_BillResidentialStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_BillUSTerritory' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_BillUSTerritory', N'IX_SWDefault_Order_BillUSTerritory', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_CombineSplitStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_CombineSplitStatus', N'IX_SWDefault_Order_CombineSplitStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_ShipAddressValidationStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_ShipAddressValidationStatus', N'IX_SWDefault_Order_ShipAddressValidationStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_ShipMilitaryAddress' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_ShipMilitaryAddress', N'IX_SWDefault_Order_ShipMilitaryAddress', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_ShipPOBox' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_ShipPOBox', N'IX_SWDefault_Order_ShipPOBox', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_ShipResidentialStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_ShipResidentialStatus', N'IX_SWDefault_Order_ShipResidentialStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_ShipUSTerritory' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_ShipUSTerritory', N'IX_SWDefault_Order_ShipUSTerritory', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_StoreIDIsManual' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_StoreIDIsManual', N'IX_SWDefault_Order_StoreIDIsManual', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_StoreIdOnlineStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_StoreIdOnlineStatus', N'IX_SWDefault_Order_StoreIdOnlineStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Order_StoreIDOrderDateLocalStatus' AND object_id = OBJECT_ID('[dbo].[Order]')) 
	EXEC sp_rename N'dbo.Order.IX_Order_StoreIDOrderDateLocalStatus', N'IX_SWDefault_Order_StoreIDOrderDateLocalStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderCharge_OrderID' AND object_id = OBJECT_ID('[dbo].[OrderCharge]')) 
	EXEC sp_rename N'dbo.OrderCharge.IX_OrderCharge_OrderID', N'IX_SWDefault_OrderCharge_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderItem_Code_OrderId' AND object_id = OBJECT_ID('[dbo].[OrderItem]')) 
	EXEC sp_rename N'dbo.OrderItem.IX_OrderItem_Code_OrderId', N'IX_SWDefault_OrderItem_Code_OrderId', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderItem_OrderID' AND object_id = OBJECT_ID('[dbo].[OrderItem]')) 
	EXEC sp_rename N'dbo.OrderItem.IX_OrderItem_OrderID', N'IX_SWDefault_OrderItem_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderItem_OriginalOrderID' AND object_id = OBJECT_ID('[dbo].[OrderItem]')) 
	EXEC sp_rename N'dbo.OrderItem.IX_OrderItem_OriginalOrderID', N'IX_SWDefault_OrderItem_OriginalOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderItemAttribute_OrderItemID' AND object_id = OBJECT_ID('[dbo].[OrderItemAttribute]')) 
	EXEC sp_rename N'dbo.OrderItemAttribute.IX_OrderItemAttribute_OrderItemID', N'IX_SWDefault_OrderItemAttribute_OrderItemID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderPaymentDetail_OrderID' AND object_id = OBJECT_ID('[dbo].[OrderPaymentDetail]')) 
	EXEC sp_rename N'dbo.OrderPaymentDetail.IX_OrderPaymentDetail_OrderID', N'IX_SWDefault_OrderPaymentDetail_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderSearch_OrderNumber' AND object_id = OBJECT_ID('[dbo].[OrderSearch]')) 
	EXEC sp_rename N'dbo.OrderSearch.IX_OrderSearch_OrderNumber', N'IX_SWDefault_OrderSearch_OrderNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderSearch_OrderNumberComplete' AND object_id = OBJECT_ID('[dbo].[OrderSearch]')) 
	EXEC sp_rename N'dbo.OrderSearch.IX_OrderSearch_OrderNumberComplete', N'IX_SWDefault_OrderSearch_OrderNumberComplete', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OrderSearch_StoreIDIsManual' AND object_id = OBJECT_ID('[dbo].[OrderSearch]')) 
	EXEC sp_rename N'dbo.OrderSearch.IX_OrderSearch_StoreIDIsManual', N'IX_SWDefault_OrderSearch_StoreIDIsManual', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OverstockOrder_SalesChannelName' AND object_id = OBJECT_ID('[dbo].[OverstockOrder]')) 
	EXEC sp_rename N'dbo.OverstockOrder.IX_OverstockOrder_SalesChannelName', N'IX_SWDefault_OverstockOrder_SalesChannelName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OverstockOrder_SofsCreatedDate' AND object_id = OBJECT_ID('[dbo].[OverstockOrder]')) 
	EXEC sp_rename N'dbo.OverstockOrder.IX_OverstockOrder_SofsCreatedDate', N'IX_SWDefault_OverstockOrder_SofsCreatedDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_OverstockOrder_WarehouseCode' AND object_id = OBJECT_ID('[dbo].[OverstockOrder]')) 
	EXEC sp_rename N'dbo.OverstockOrder.IX_OverstockOrder_WarehouseCode', N'IX_SWDefault_OverstockOrder_WarehouseCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PayPalOrder_PaymentStatus' AND object_id = OBJECT_ID('[dbo].[PayPalOrder]')) 
	EXEC sp_rename N'dbo.PayPalOrder.IX_PayPalOrder_PaymentStatus', N'IX_SWDefault_PayPalOrder_PaymentStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Permission' AND object_id = OBJECT_ID('[dbo].[Permission]')) 
	EXEC sp_rename N'dbo.Permission.IX_Permission', N'IX_SWDefault_Permission', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PostalShipment_Confirmation' AND object_id = OBJECT_ID('[dbo].[PostalShipment]')) 
	EXEC sp_rename N'dbo.PostalShipment.IX_PostalShipment_Confirmation', N'IX_SWDefault_PostalShipment_Confirmation', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PostalShipment_PackagingType' AND object_id = OBJECT_ID('[dbo].[PostalShipment]')) 
	EXEC sp_rename N'dbo.PostalShipment.IX_PostalShipment_PackagingType', N'IX_SWDefault_PostalShipment_PackagingType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PostalShipment_Service' AND object_id = OBJECT_ID('[dbo].[PostalShipment]')) 
	EXEC sp_rename N'dbo.PostalShipment.IX_PostalShipment_Service', N'IX_SWDefault_PostalShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PrintResult_PrintDateRelatedObjectID' AND object_id = OBJECT_ID('[dbo].[PrintResult]')) 
	EXEC sp_rename N'dbo.PrintResult.IX_PrintResult_PrintDateRelatedObjectID', N'IX_SWDefault_PrintResult_PrintDateRelatedObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PrintResult_RelatedObjectID' AND object_id = OBJECT_ID('[dbo].[PrintResult]')) 
	EXEC sp_rename N'dbo.PrintResult.IX_PrintResult_RelatedObjectID', N'IX_SWDefault_PrintResult_RelatedObjectID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ProStoresOrder_ConfirmationNumber' AND object_id = OBJECT_ID('[dbo].[ProStoresOrder]')) 
	EXEC sp_rename N'dbo.ProStoresOrder.IX_ProStoresOrder_ConfirmationNumber', N'IX_SWDefault_ProStoresOrder_ConfirmationNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ProStoresOrderSearch_ConfirmationNumber' AND object_id = OBJECT_ID('[dbo].[ProStoresOrderSearch]')) 
	EXEC sp_rename N'dbo.ProStoresOrderSearch.IX_ProStoresOrderSearch_ConfirmationNumber', N'IX_SWDefault_ProStoresOrderSearch_ConfirmationNumber', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Resource_Checksum' AND object_id = OBJECT_ID('[dbo].[Resource]')) 
	EXEC sp_rename N'dbo.Resource.IX_Resource_Checksum', N'IX_SWDefault_Resource_Checksum', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Resource_Filename' AND object_id = OBJECT_ID('[dbo].[Resource]')) 
	EXEC sp_rename N'dbo.Resource.IX_Resource_Filename', N'IX_SWDefault_Resource_Filename', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_SearsOrder_PoNumber_OrderID' AND object_id = OBJECT_ID('[dbo].[SearsOrder]')) 
	EXEC sp_rename N'dbo.SearsOrder.IX_SearsOrder_PoNumber_OrderID', N'IX_SWDefault_SearsOrder_PoNumber_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_SearsOrderSearch_PoNumber_OrderID' AND object_id = OBJECT_ID('[dbo].[SearsOrderSearch]')) 
	EXEC sp_rename N'dbo.SearsOrderSearch.IX_SearsOrderSearch_PoNumber_OrderID', N'IX_SWDefault_SearsOrderSearch_PoNumber_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ServerMessage_Expires' AND object_id = OBJECT_ID('[dbo].[ServerMessage]')) 
	EXEC sp_rename N'dbo.ServerMessage.IX_ServerMessage_Expires', N'IX_SWDefault_ServerMessage_Expires', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ServerMessage_Number' AND object_id = OBJECT_ID('[dbo].[ServerMessage]')) 
	EXEC sp_rename N'dbo.ServerMessage.IX_ServerMessage_Number', N'IX_SWDefault_ServerMessage_Number', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ServerMessage_RowVersion' AND object_id = OBJECT_ID('[dbo].[ServerMessage]')) 
	EXEC sp_rename N'dbo.ServerMessage.IX_ServerMessage_RowVersion', N'IX_SWDefault_ServerMessage_RowVersion', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ServerMessageSignoff' AND object_id = OBJECT_ID('[dbo].[ServerMessageSignoff]')) 
	EXEC sp_rename N'dbo.ServerMessageSignoff.IX_ServerMessageSignoff', N'IX_SWDefault_ServerMessageSignoff', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ServiceStatus' AND object_id = OBJECT_ID('[dbo].[ServiceStatus]')) 
	EXEC sp_rename N'dbo.ServiceStatus.IX_ServiceStatus', N'IX_SWDefault_ServiceStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ActualLabelFormat' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ActualLabelFormat', N'IX_SWDefault_Shipment_ActualLabelFormat', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_OrderID' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_OrderID', N'IX_SWDefault_Shipment_OrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_OrderID_ShipSenseStatus' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_OrderID_ShipSenseStatus', N'IX_SWDefault_Shipment_OrderID_ShipSenseStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ProcessedOrderID' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ProcessedOrderID', N'IX_SWDefault_Shipment_ProcessedOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_RequestedLabelFormat' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_RequestedLabelFormat', N'IX_SWDefault_Shipment_RequestedLabelFormat', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ReturnShipment' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ReturnShipment', N'IX_SWDefault_Shipment_ReturnShipment', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipAddressValidationStatus' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipAddressValidationStatus', N'IX_SWDefault_Shipment_ShipAddressValidationStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipDate' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipDate', N'IX_SWDefault_Shipment_ShipDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipmentType' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipmentType', N'IX_SWDefault_Shipment_ShipmentType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipMilitaryAddress' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipMilitaryAddress', N'IX_SWDefault_Shipment_ShipMilitaryAddress', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipPOBox' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipPOBox', N'IX_SWDefault_Shipment_ShipPOBox', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipResidentialStatus' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipResidentialStatus', N'IX_SWDefault_Shipment_ShipResidentialStatus', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipment_ShipUSTerritory' AND object_id = OBJECT_ID('[dbo].[Shipment]')) 
	EXEC sp_rename N'dbo.Shipment.IX_Shipment_ShipUSTerritory', N'IX_SWDefault_Shipment_ShipUSTerritory', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentCustomsItem_ShipmentID' AND object_id = OBJECT_ID('[dbo].[ShipmentCustomsItem]')) 
	EXEC sp_rename N'dbo.ShipmentCustomsItem.IX_ShipmentCustomsItem_ShipmentID', N'IX_SWDefault_ShipmentCustomsItem_ShipmentID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShippingOrigin_Description' AND object_id = OBJECT_ID('[dbo].[ShippingOrigin]')) 
	EXEC sp_rename N'dbo.ShippingOrigin.IX_ShippingOrigin_Description', N'IX_SWDefault_ShippingOrigin_Description', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shortcut_Barcode' AND object_id = OBJECT_ID('[dbo].[Shortcut]')) 
	EXEC sp_rename N'dbo.Shortcut.IX_Shortcut_Barcode', N'IX_SWDefault_Shortcut_Barcode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shortcut_Keys' AND object_id = OBJECT_ID('[dbo].[Shortcut]')) 
	EXEC sp_rename N'dbo.Shortcut.IX_Shortcut_Keys', N'IX_SWDefault_Shortcut_Keys', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Store_StoreName' AND object_id = OBJECT_ID('[dbo].[Store]')) 
	EXEC sp_rename N'dbo.Store.IX_Store_StoreName', N'IX_SWDefault_Store_StoreName', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_TemplateStoreSettings' AND object_id = OBJECT_ID('[dbo].[TemplateStoreSettings]')) 
	EXEC sp_rename N'dbo.TemplateStoreSettings.IX_TemplateStoreSettings', N'IX_SWDefault_TemplateStoreSettings', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip' AND object_id = OBJECT_ID('[dbo].[UpsLocalRatingDeliveryAreaSurcharge]')) 
	EXEC sp_rename N'dbo.UpsLocalRatingDeliveryAreaSurcharge.IX_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip', N'IX_SWDefault_UpsLocalRatingDeliveryAreaSurcharge_DestinationZip', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsPackage_PackagingType' AND object_id = OBJECT_ID('[dbo].[UpsPackage]')) 
	EXEC sp_rename N'dbo.UpsPackage.IX_UpsPackage_PackagingType', N'IX_SWDefault_UpsPackage_PackagingType', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsPackage_ShipmentID' AND object_id = OBJECT_ID('[dbo].[UpsPackage]')) 
	EXEC sp_rename N'dbo.UpsPackage.IX_UpsPackage_ShipmentID', N'IX_SWDefault_UpsPackage_ShipmentID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsPackageRate_WeightInPounds_Zone' AND object_id = OBJECT_ID('[dbo].[UpsPackageRate]')) 
	EXEC sp_rename N'dbo.UpsPackageRate.IX_UpsPackageRate_WeightInPounds_Zone', N'IX_SWDefault_UpsPackageRate_WeightInPounds_Zone', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsShipment_DeliveryConfirmation' AND object_id = OBJECT_ID('[dbo].[UpsShipment]')) 
	EXEC sp_rename N'dbo.UpsShipment.IX_UpsShipment_DeliveryConfirmation', N'IX_SWDefault_UpsShipment_DeliveryConfirmation', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UpsShipment_Service' AND object_id = OBJECT_ID('[dbo].[UpsShipment]')) 
	EXEC sp_rename N'dbo.UpsShipment.IX_UpsShipment_Service', N'IX_SWDefault_UpsShipment_Service', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_User_Username' AND object_id = OBJECT_ID('[dbo].[User]')) 
	EXEC sp_rename N'dbo.User.IX_User_Username', N'IX_SWDefault_User_Username', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_UserColumnSettings' AND object_id = OBJECT_ID('[dbo].[UserColumnSettings]')) 
	EXEC sp_rename N'dbo.UserColumnSettings.IX_UserColumnSettings', N'IX_SWDefault_UserColumnSettings', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ValidatedAddress_ConsumerIDAddressPrefix' AND object_id = OBJECT_ID('[dbo].[ValidatedAddress]')) 
	EXEC sp_rename N'dbo.ValidatedAddress.IX_ValidatedAddress_ConsumerIDAddressPrefix', N'IX_SWDefault_ValidatedAddress_ConsumerIDAddressPrefix', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_VersionSignoff' AND object_id = OBJECT_ID('[dbo].[VersionSignoff]')) 
	EXEC sp_rename N'dbo.VersionSignoff.IX_VersionSignoff', N'IX_SWDefault_VersionSignoff', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_CustomerOrderId' AND object_id = OBJECT_ID('[dbo].[WalmartOrder]')) 
	EXEC sp_rename N'dbo.WalmartOrder.IX_Auto_CustomerOrderId', N'IX_SWDefault_CustomerOrderId', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_EstimatedDeliveryDate' AND object_id = OBJECT_ID('[dbo].[WalmartOrder]')) 
	EXEC sp_rename N'dbo.WalmartOrder.IX_Auto_EstimatedDeliveryDate', N'IX_SWDefault_EstimatedDeliveryDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_EstimatedShipDate' AND object_id = OBJECT_ID('[dbo].[WalmartOrder]')) 
	EXEC sp_rename N'dbo.WalmartOrder.IX_Auto_EstimatedShipDate', N'IX_SWDefault_EstimatedShipDate', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_PurchaseOrderId' AND object_id = OBJECT_ID('[dbo].[WalmartOrder]')) 
	EXEC sp_rename N'dbo.WalmartOrder.IX_Auto_PurchaseOrderId', N'IX_SWDefault_PurchaseOrderId', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Auto_RequestedShippingMethodCode' AND object_id = OBJECT_ID('[dbo].[WalmartOrder]')) 
	EXEC sp_rename N'dbo.WalmartOrder.IX_Auto_RequestedShippingMethodCode', N'IX_SWDefault_RequestedShippingMethodCode', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_WalmartOrderSearch_CustomerOrderID' AND object_id = OBJECT_ID('[dbo].[WalmartOrderSearch]')) 
	EXEC sp_rename N'dbo.WalmartOrderSearch.IX_WalmartOrderSearch_CustomerOrderID', N'IX_SWDefault_WalmartOrderSearch_CustomerOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_WalmartOrderSearch_PurchaseOrderID' AND object_id = OBJECT_ID('[dbo].[WalmartOrderSearch]')) 
	EXEC sp_rename N'dbo.WalmartOrderSearch.IX_WalmartOrderSearch_PurchaseOrderID', N'IX_SWDefault_WalmartOrderSearch_PurchaseOrderID', N'INDEX';

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_WorldShipPackage_ShipmentID' AND object_id = OBJECT_ID('[dbo].[WorldShipPackage]')) 
	EXEC sp_rename N'dbo.WorldShipPackage.IX_WorldShipPackage_ShipmentID', N'IX_SWDefault_WorldShipPackage_ShipmentID', N'INDEX';
GO