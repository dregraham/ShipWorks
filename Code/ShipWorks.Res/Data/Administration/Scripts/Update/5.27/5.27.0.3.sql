PRINT N'Adding dimension fields to OrderItems with constraints'
GO
ALTER TABLE dbo.OrderItem ADD [Length] [decimal] (29, 9) NOT NULL CONSTRAINT DF_OrderItem_Length DEFAULT 0
ALTER TABLE dbo.OrderItem ADD [Width] [decimal] (29, 9) NOT NULL CONSTRAINT DF_OrderItem_Width DEFAULT 0
ALTER TABLE dbo.OrderItem ADD [Height] [decimal] (29, 9) NOT NULL CONSTRAINT DF_OrderItem_Height DEFAULT 0
GO 
PRINT N'Removing constraints'
GO
ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Length
ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Width
ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Height  
GO