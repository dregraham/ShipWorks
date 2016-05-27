-- script to only drop indexes that are detrimental to the data move

-- slows down the customer deletes
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[v2m_Customers]') AND name = N'IX_Customers_AddressHash')
DROP INDEX v2m_Customers.IX_Customers_AddressHash

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[v2m_Orders]') AND name = N'IX_OrdersStores')
ALTER TABLE [dbo].[v2m_Orders] DROP CONSTRAINT [IX_OrdersStores]
