PRINT N'Altering [dbo].[OrderItem]'
GO
	alter table [OrderItem] alter column [HarmonizedCode] nvarchar(20)
	alter table [ShipmentCustomsItem] alter column [HarmonizedCode] nvarchar(20)
GO