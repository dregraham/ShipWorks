
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

PRINT N'Altering [dbo].[AmazonStore]'
GO
ALTER TABLE [dbo].[AmazonStore] ALTER COLUMN [MerchantID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[AmazonStore] ALTER COLUMN [MarketplaceID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO

PRINT N'Creating [dbo].[ThreeDCartOrderItem]'
GO
CREATE TABLE [dbo].[ThreeDCartOrderItem]
(
[OrderItemID] [bigint] NOT NULL,
[ThreeDCartShipmentID] [bigint] NOT NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartOrderItem] on [dbo].[ThreeDCartOrderItem]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderItem] ADD CONSTRAINT [PK_ThreeDCartOrderItem] PRIMARY KEY CLUSTERED  ([OrderItemID])
GO
PRINT N'Creating [dbo].[ThreeDCartStore]'
GO
CREATE TABLE [dbo].[ThreeDCartStore]
(
[StoreID] [bigint] NOT NULL,
[StoreUrl] [nvarchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApiUserKey] [nvarchar] (65) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeZoneID] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StatusCodes] [xml] NULL
)
GO
PRINT N'Creating primary key [PK_ThreeDCartStore] on [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [PK_ThreeDCartStore] PRIMARY KEY CLUSTERED  ([StoreID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartOrderItem]'
GO
ALTER TABLE [dbo].[ThreeDCartOrderItem] ADD CONSTRAINT [FK_ThreeDCartOrderItem_OrderItem] FOREIGN KEY ([OrderItemID]) REFERENCES [dbo].[OrderItem] ([OrderItemID])
GO
PRINT N'Adding foreign keys to [dbo].[ThreeDCartStore]'
GO
ALTER TABLE [dbo].[ThreeDCartStore] ADD CONSTRAINT [FK_ThreeDCartStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
EXEC sp_addextendedproperty N'AuditName', N'User Key', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'ApiUserKey'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'0', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO
EXEC sp_addextendedproperty N'AuditName', N'Store URL', 'SCHEMA', N'dbo', 'TABLE', N'ThreeDCartStore', 'COLUMN', N'StoreUrl'
GO

-- 3d Cart store/order conversion
begin tran

declare @3dCartTypeCode as int;
set @3dCartTypeCode = 29

-- Find the 3d cart generic module stores that do NOT already have a 3d cart store entry
;with GSM3DCartStores as
(
	select gms.* 
	from GenericModuleStore gms, Store s
	where s.StoreID = gms.StoreID
	  and s.TypeCode = @3dCartTypeCode
	  and gms.StoreID not in (select StoreID from ThreeDCartStore)
)

-- Insert a 3d cart store entry for each found
-- Default the user key to blank, so that upon first download the user will get an error
-- letting them know they need to enter the api user key
insert into ThreeDCartStore (StoreID, StoreUrl, ApiUserKey, TimeZoneId, StatusCodes)
	select gsm.StoreID, gsm.ModuleUrl, '', null, ModuleStatusCodes
	from GSM3DCartStores gsm
	
-- Now delete the generid module stores that made it into the 3d cart store table
delete from GenericModuleStore where StoreID in
(
	select StoreID from ThreeDCartStore
)

-- Now create 3d cart order entries for existing 3d cart orders
;with ThreeDCartOrderItems as
(
	select distinct oi.OrderItemID 
	from [Order] o, Store s, OrderItem oi
	where s.StoreID = o.StoreID
	  and s.TypeCode = @3dCartTypeCode
	  and oi.OrderID = o.OrderID
)
insert into ThreeDCartOrderItem (OrderItemID, ThreeDCartShipmentID)
	select o.OrderItemID, 0
	from  ThreeDCartOrderItems o

commit