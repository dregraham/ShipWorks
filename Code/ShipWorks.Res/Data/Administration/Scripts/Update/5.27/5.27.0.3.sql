ALTER TABLE dbo.OrderItem ADD [Length] [float] NOT NULL CONSTRAINT DF_OrderItem_Length DEFAULT 0
ALTER TABLE dbo.OrderItem ADD [Width] [float] NOT NULL CONSTRAINT DF_OrderItem_Width DEFAULT 0
ALTER TABLE dbo.OrderItem ADD [Height] [float] NOT NULL CONSTRAINT DF_OrderItem_Height DEFAULT 0
GO 

ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Length
ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Width
ALTER TABLE dbo.OrderItem DROP CONSTRAINT DF_OrderItem_Height  
GO