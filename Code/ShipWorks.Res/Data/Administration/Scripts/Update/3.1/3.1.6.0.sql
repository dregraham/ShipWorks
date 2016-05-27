SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT N'Altering [dbo].[OrderItemAttribute]'
GO
ALTER TABLE [dbo].[OrderItemAttribute]
	ADD IsManual BIT NOT NULL CONSTRAINT DF_OrderItemAttribute_IsManual DEFAULT 0
GO
ALTER TABLE [dbo].[OrderItemAttribute]
	DROP CONSTRAINT DF_OrderItemAttribute_IsManual
GO

PRINT N'Creating [dbo].[MivaOrderItemAttribute]'
GO
CREATE TABLE [dbo].[MivaOrderItemAttribute]
(
[OrderItemAttributeID] [bigint] NOT NULL,
[MivaOptionCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MivaAttributeID] [int] NOT NULL,
[MivaAttributeCode] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
PRINT N'Creating primary key [PK_MivaOrderItemAttributes] on [dbo].[MivaOrderItemAttribute]'
GO
ALTER TABLE [dbo].[MivaOrderItemAttribute] ADD CONSTRAINT [PK_MivaOrderItemAttributes] PRIMARY KEY CLUSTERED  ([OrderItemAttributeID])
GO
PRINT N'Adding foreign keys to [dbo].[MivaOrderItemAttribute]'
GO
ALTER TABLE [dbo].[MivaOrderItemAttribute] ADD
CONSTRAINT [FK_MivaOrderItemAttribute_OrderItemAttribute] FOREIGN KEY ([OrderItemAttributeID]) REFERENCES [dbo].[OrderItemAttribute] ([OrderItemAttributeID])
GO
-- populate the table
INSERT INTO MivaOrderItemAttribute (OrderItemAttributeID, MivaAttributeID, MivaAttributeCode, MivaOptionCode)
SELECT a.OrderItemAttributeID, 0, '', '' FROM OrderItemAttribute a 
		JOIN OrderItem i on a.OrderItemID = i.OrderItemID
		JOIN [Order] o on o.OrderID = i.OrderID
		JOIN [Store] s on s.StoreID = o.StoreID
WHERE s.TypeCode = 0