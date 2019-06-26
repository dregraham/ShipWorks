PRINT N'Altering [dbo].[Store]'
GO
ALTER TABLE [dbo].[Store] ALTER COLUMN [Phone] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
PRINT N'Altering [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ALTER COLUMN [ShipPhone] [nvarchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
GO
