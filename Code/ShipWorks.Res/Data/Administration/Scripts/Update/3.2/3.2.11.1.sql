GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[BigCommerceOrderItem]'
GO
CREATE TABLE [dbo].[BigCommerceOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[OrderAddressID] [bigint] NOT NULL,
[OrderProductID] [bigint] NOT NULL,
[IsDigitalItem] [bit] NOT NULL CONSTRAINT [DF_BigCommerceOrderItem_IsDigitalItem] DEFAULT ((0)),
[EventDate] [datetime] NULL,
[EventName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
PRINT N'Creating primary key [PK_BigCommerceOrderItem] on [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] ADD CONSTRAINT [PK_BigCommerceOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[BigCommerceStore]'
GO
CREATE TABLE [dbo].[BigCommerceStore]
(
[StoreID] [bigint] NOT NULL,
[ApiUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserName] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiToken] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_BigCommerceStore] on [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD CONSTRAINT [PK_BigCommerceStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[BigCommerceOrderItem]'
GO
ALTER TABLE [dbo].[BigCommerceOrderItem] ADD CONSTRAINT [FK_BigCommerceOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[BigCommerceStore]'
GO
ALTER TABLE [dbo].[BigCommerceStore] ADD CONSTRAINT [FK_BigCommerceStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiToken'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api Token', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiToken'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUrl'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api Url', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUrl'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUserName'
GO
EXEC sp_addextendedproperty N'AuditName', N'Api User Name', 'SCHEMA', N'dbo', 'TABLE', N'BigCommerceStore', 'COLUMN', N'ApiUserName'
GO

	
-- Convert any BigCommerce GenericModuleStores and OrderItems to the new BigCommerce Schema
begin tran

	insert into BigCommerceStore (StoreID, ApiUrl, ApiUserName, ApiToken, StatusCodes)
		select s.StoreID, REPLACE(gms.ModuleUrl, 'admin/shippingmanager.php?manager=shipworks', 'api/v2/'), '', '', gms.ModuleStatusCodes
		from Store s, GenericModuleStore gms where s.StoreID = gms.StoreID and s.TypeCode = 30 
		 and s.StoreID not in (select StoreID from BigCommerceStore)

	delete from GenericModuleStore where StoreID in (select StoreID from Store where TypeCode = 30)

	insert into BigCommerceOrderItem (OrderItemID, OrderAddressID, OrderProductID, IsDigitalItem, EventDate, EventName)
		select oi.OrderItemID, -1, -1, 0, null, null
		from Store s, [Order] o, OrderItem oi where s.TypeCode = 30 and s.StoreID = o.StoreID and o.OrderID = oi.OrderID
		 and oi.OrderItemID not in (select OrderItemID from BigCommerceOrderItem)

commit tran

-- Convert BigCommerce GenericStoreOrderUpdate action tasks to BigCommerceOrderUpdateTask
begin tran

	declare @storeId nvarchar(20)
	declare @storeTypeCode int
	
	set @storeTypeCode = 30

	DECLARE updateStores CURSOR
		FOR 
			SELECT distinct s.StoreID FROM Store s
			where s.TypeCode = @storeTypeCode
			
	OPEN updateStores
	FETCH NEXT FROM updateStores into @storeId

	WHILE @@FETCH_STATUS = 0
	BEGIN

		update [Action] set TaskSummary = 'BigCommerceOrderUpdate' where ActionID in 
		(
			select ActionID from ActionTask 
			where TaskIdentifier = 'GenericStoreOrderUpdate' and TaskSettings.exist('//Settings/StoreID[@value=sql:variable("@storeId")]') = 1
		)

		update ActionTask set TaskIdentifier='BigCommerceOrderUpdate', TaskSettings.modify('delete //Settings/Comment')
		where TaskIdentifier = 'GenericStoreOrderUpdate' and TaskSettings.exist('//Settings/StoreID[@value=sql:variable("@storeId")]') = 1

		update ActionTask set TaskIdentifier='BigCommerceShipmentUpload'
		where TaskIdentifier = 'GenericStoreShipmentUpload' and TaskSettings.exist('//Settings/StoreTypeCode[@value=sql:variable("@storeTypeCode")]') = 1

		FETCH NEXT FROM updateStores into @storeId
	END

	CLOSE updateStores;
	DEALLOCATE updateStores;
commit tran
