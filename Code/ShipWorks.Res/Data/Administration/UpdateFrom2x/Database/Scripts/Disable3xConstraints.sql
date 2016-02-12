ALTER TABLE dbo.UpsPackage NOCHECK CONSTRAINT ALL
GO
ALTER TABLE dbo.FedexPackage NOCHECK CONSTRAINT ALL
GO

-- This is needed b\c the SuperUser isn't created yet at the time we create notes
ALTER TABLE dbo.Note
	DROP CONSTRAINT FK_Note_User
GO

-- A null value in this column is how "UpdateOrderRollups.sql" knows it still has work to do
ALTER TABLE [Order]
 ALTER COLUMN [RollupItemCount] int NULL
GO

-- A null value in this column is how "UpdateeBayOrderRollups.sql" knows it still has work to do
ALTER TABLE [EbayOrder]
 ALTER COLUMN [RollupEbayItemCount] int NULL
GO

-- A null value in this column is how "UpdateCustomerRollups.sql" knows it still has work to do
ALTER TABLE [Customer]
  ALTER COLUMN [RollupOrderCount] int NULL
GO