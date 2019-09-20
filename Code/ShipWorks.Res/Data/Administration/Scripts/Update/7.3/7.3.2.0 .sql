PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [Order] ALTER COLUMN BillPhone NVARCHAR (35) NOT Null;
GO