SET IDENTITY_INSERT [ShipWorks3xBig]..Customer ON

INSERT INTO [ShipWorks3xBig].[dbo].[Customer]
           (CustomerID,
		    [BillFirstName]
           ,[BillMiddleName]
           ,[BillLastName]
           ,[BillCompany]
           ,[BillStreet1]
           ,[BillStreet2]
           ,[BillStreet3]
           ,[BillCity]
           ,[BillStateProvCode]
           ,[BillPostalCode]
           ,[BillCountryCode]
           ,[BillPhone]
           ,[BillFax]
           ,[BillEmail]
           ,[BillWebsite]
           ,[ShipFirstName]
           ,[ShipMiddleName]
           ,[ShipLastName]
           ,[ShipCompany]
           ,[ShipStreet1]
           ,[ShipStreet2]
           ,[ShipStreet3]
           ,[ShipCity]
           ,[ShipStateProvCode]
           ,[ShipPostalCode]
           ,[ShipCountryCode]
           ,[ShipPhone]
           ,[ShipFax]
           ,[ShipEmail]
           ,[ShipWebsite])
SELECT      (CustomerID * 1000) + 12,
		    [BillFirstName]
           ,''
           ,[BillLastName]
           ,[BillCompany]
           ,[BillAddress1]
           ,[BillAddress2]
           ,[BillAddress3]
           ,[BillCity]
           ,[BillStateProvinceCode]
           ,[BillPostalCode]
           ,[BillCountryCode]
           ,[BillPhone]
           ,[BillFax]
           ,[BillEmail]
           ,''
           ,[ShipFirstName]
           ,''
           ,[ShipLastName]
           ,[ShipCompany]
           ,[ShipAddress1]
           ,[ShipAddress2]
           ,[ShipAddress3]
           ,[ShipCity]
           ,[ShipStateProvinceCode]
           ,[ShipPostalCode]
           ,[ShipCountryCode]
           ,[ShipPhone]
           ,[ShipFax]
           ,[ShipEmail]
           ,''
FROM [ShipWorks2xBig]..Customers

SET IDENTITY_INSERT [ShipWorks3xBig]..Customer OFF

GO

DECLARE @seed bigint
SELECT @seed = MAX(CustomerID) + 1000 FROM [ShipWorks3xBig]..Customer
DBCC CHECKIDENT ('[ShipWorks3xBig]..Customer', RESEED, @seed)
GO

DECLARE @StoreID bigint
SELECT TOP 1 @StoreID = StoreID
  FROM [ShipWorks3xBig]..Store

SET IDENTITY_INSERT [ShipWorks3xBig]..[Order] ON

INSERT INTO [ShipWorks3xBig].[dbo].[Order]
           (OrderID
		   ,[StoreID]
           ,[CustomerID]
           ,[OrderNumber]
		   ,[OrderNumberComplete]
           ,[OrderDate]
           ,[OrderTotal]
           ,[LocalStatus]
           ,[IsManual]
           ,[OnlineLastModified]
           ,[OnlineCustomerID]
           ,[OnlineStatus]
           ,[RequestedShipping]
           ,[CustomerComments]
           ,[BillFirstName]
           ,[BillMiddleName]
           ,[BillLastName]
           ,[BillCompany]
           ,[BillStreet1]
           ,[BillStreet2]
           ,[BillStreet3]
           ,[BillCity]
           ,[BillStateProvCode]
           ,[BillPostalCode]
           ,[BillCountryCode]
           ,[BillPhone]
           ,[BillFax]
           ,[BillEmail]
           ,[BillWebsite]
           ,[ShipFirstName]
           ,[ShipMiddleName]
           ,[ShipLastName]
           ,[ShipCompany]
           ,[ShipStreet1]
           ,[ShipStreet2]
           ,[ShipStreet3]
           ,[ShipCity]
           ,[ShipStateProvCode]
           ,[ShipPostalCode]
           ,[ShipCountryCode]
           ,[ShipPhone]
           ,[ShipFax]
           ,[ShipEmail]
           ,[ShipWebsite]
		   ,[RollupItems])
SELECT      (OrderID * 1000) + 6
		   ,@StoreID
           ,([CustomerID] * 1000) + 12
           ,[OrderNumber]
		   ,[OrderNumberDisplay]
           ,[OrderDate]
           ,[Total]
           ,[Status]
           ,[IsManual]
           ,[OnlineLastModified]
           ,NULL
           ,'Delivered'
           ,[RequestedShipping]
           ,[CustomerComments]
           ,[BillFirstName]
           ,''
           ,[BillLastName]
           ,[BillCompany]
           ,[BillAddress1]
           ,[BillAddress2]
           ,[BillAddress3]
           ,[BillCity]
           ,[BillStateProvinceCode]
           ,[BillPostalCode]
           ,[BillCountryCode]
           ,[BillPhone]
           ,[BillFax]
           ,[BillEmail]
           ,''
           ,[ShipFirstName]
           ,''
           ,[ShipLastName]
           ,[ShipCompany]
           ,[ShipAddress1]
           ,[ShipAddress2]
           ,[ShipAddress3]
           ,[ShipCity]
           ,[ShipStateProvinceCode]
           ,[ShipPostalCode]
           ,[ShipCountryCode]
           ,[ShipPhone]
           ,[ShipFax]
           ,[ShipEmail]
           ,''
	       ,0
FROM [ShipWorks2xBig]..Orders

SET IDENTITY_INSERT [ShipWorks3xBig]..[Order] OFF

GO

DECLARE @seed bigint
SELECT @seed = MAX(OrderID) + 1000 FROM [ShipWorks3xBig]..[Order]
DBCC CHECKIDENT ('[ShipWorks3xBig]..[Order]', RESEED, @seed)
GO

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderItem] ON

INSERT INTO [ShipWorks3xBig].[dbo].[OrderItem]
           (OrderItemID
		   ,[OrderID]
           ,[SKU]
           ,[Name]
		   ,[Code]
           ,[Description]
           ,[Location]
           ,[Image]
           ,[Thumbnail]
           ,[UnitPrice]
           ,[UnitCost]
           ,[Weight]
           ,[Quantity]
           ,[LocalStatus])
SELECT      
			(OrderItemID * 1000) + 13
		   ,([OrderID] * 1000) + 6
           ,[SKU]
           ,[Name]
           ,[Code]
           ,[Code]
           ,[Location]
           ,[Image]
           ,[Thumbnail]
           ,[UnitPrice]
           ,[UnitCost]
           ,[Weight]
           ,[Quantity]
           ,[Status]
FROM [ShipWorks2xBig]..OrderItems

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderItem] OFF 

GO

DECLARE @seed bigint
SELECT @seed = MAX(OrderItemID) + 1000 FROM [ShipWorks3xBig]..OrderItem
DBCC CHECKIDENT ('[ShipWorks3xBig]..OrderItem', RESEED, @seed)
GO

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderItemAttribute] ON

INSERT INTO [ShipWorks3xBig].[dbo].[OrderItemAttribute]
           (OrderItemAttributeID
		   ,[OrderItemID]
           ,[Name]
           ,[Description]
           ,[UnitPrice])
SELECT     (AttributeID * 1000) + 20
		   ,([OrderItemID] * 1000) + 13
           ,[Name]
           ,[Description]
           ,[UnitPrice]
FROM [ShipWorks2xBig]..OrderItemAttributes

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderItemAttribute] OFF

GO

DECLARE @seed bigint
SELECT @seed = MAX(OrderItemAttributeID) + 1000 FROM [ShipWorks3xBig]..[OrderItemAttribute]
DBCC CHECKIDENT ('[ShipWorks3xBig]..OrderItemAttribute', RESEED, @seed)
GO

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderCharge] ON

INSERT INTO [ShipWorks3xBig].[dbo].[OrderCharge]
           (OrderChargeID
		   ,[OrderID]
           ,[Type]
           ,[Description]
           ,[Amount])
SELECT (ChargeID * 1000) + 21
		   ,([OrderID] * 1000) + 6
           ,[Type]
           ,[Description]
           ,[Amount]
FROM [ShipWorks2xBig]..OrderCharges

SET IDENTITY_INSERT [ShipWorks3xBig]..[OrderCharge] OFF

GO

DECLARE @seed bigint
SELECT @seed = MAX(OrderChargeID) + 1000 FROM [ShipWorks3xBig]..[OrderCharge]
DBCC CHECKIDENT ('[ShipWorks3xBig]..OrderCharge', RESEED, @seed)
GO

INSERT INTO [ShipWorks3xBig].[dbo].[OsCommerceOrder]
           ([OrderID]
           ,[OnlineStatusCode])
SELECT (OrderID * 1000) + 6, 3
  FROM [ShipWorks2xBig]..Orders
GO