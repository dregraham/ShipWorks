/*
DELETE [Order]
DELETE [Customer]
DBCC CHECKIDENT ('Order', RESEED, 6)
DBCC CHECKIDENT ('Customer', RESEED, 12)
*/

DECLARE @Customers int
DECLARE @Orders int
DECLARE @ItemsPerOrder int

SET @Customers = 4
SET @Orders    = 4
SET @ItemsPerOrder = 3

INSERT INTO [Customer]
           (
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
		SELECT
			FirstName,
			COALESCE(MiddleName, ''),
			LastName,
			'',
			AddressLine1,
			COALESCE(AddressLine2, ''),
			'',
			City,
			state.Name,
			PostalCode,
			'US',
			ABS(CHECKSUM(NEWID())) % 1234567,
			ABS(CHECKSUM(NEWID())) % 1234567,
			EmailAddress,
			'',
			FirstName,
			COALESCE(MiddleName, ''),
			LastName,
			'',
			AddressLine1,
			COALESCE(AddressLine2, ''),
			'',
			City,
			state.Name,
			PostalCode,
			'US',
			ABS(CHECKSUM(NEWID())) % 1234567,
			ABS(CHECKSUM(NEWID())) % 1234567,
			EmailAddress,
			''
		FROM (SELECT TOP(CAST(SQRT(@Customers) as int)) * FROM AdventureWorks.Person.Address) as t INNER JOIN AdventureWorks.Person.StateProvince state ON t.StateProvinceID = state.StateProvinceID, 
             (SELECT TOP(CAST(SQRT(@Customers) as int)) * FROM AdventureWorks.Person.Contact) AS c 

DECLARE @topCustomer int
SELECT @topCustomer = MAX(CustomerID) FROM Customer
SET @topCustomer = (@topCustomer - 12) / 1000

INSERT INTO [Order]
           (
			StoreID,
			CustomerID,	
            OrderNumber,
            OrderDate,
            OrderTotal,
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
		SELECT
			5, 
			((ABS(CHECKSUM(NEWID())) % @topCustomer) * 1000) + 1012,
			ABS(CHECKSUM(NEWID())) % 100000000, DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 700), GETDATE()),
		    ABS(CHECKSUM(NEWID())) % 500,
			FirstName,
			COALESCE(MiddleName, ''),
			LastName,
			'',
			AddressLine1,
			COALESCE(AddressLine2, ''),
			'',
			City,
			state.Name,
			PostalCode,
			'US',
			ABS(CHECKSUM(NEWID())) % 1234567,
			ABS(CHECKSUM(NEWID())) % 1234567,
			EmailAddress,
			'',
			FirstName,
			COALESCE(MiddleName, ''),
			LastName,
			'',
			AddressLine1,
			COALESCE(AddressLine2, ''),
			'',
			City,
			state.Name,
			PostalCode,
			'US',
			ABS(CHECKSUM(NEWID())) % 1234567,
			ABS(CHECKSUM(NEWID())) % 1234567,
			EmailAddress,
			''
		FROM (SELECT TOP(CAST(SQRT(@Orders) as int)) * FROM AdventureWorks.Person.Address) as t INNER JOIN AdventureWorks.Person.StateProvince state ON t.StateProvinceID = state.StateProvinceID, 
             (SELECT TOP(CAST(SQRT(@Orders) as int)) * FROM AdventureWorks.Person.Contact) AS c 

DECLARE @topOrder int
SELECT @topOrder = MAX(OrderID) FROM [Order]
SET @topOrder = (@topOrder - 6) / 1000

INSERT INTO [OrderItem]
  (
	 OrderID,
     [Name]
  )
   SELECT
     ((ABS(CHECKSUM(NEWID())) % @topOrder) * 1000) + 1006,
     p.[Name]
    FROM (SELECT x.* FROM AdventureWorks.Production.Product x CROSS JOIN (SELECT TOP((@Orders * @ItemsPerOrder) / 504) * FROM [Order] o) as y) as p
