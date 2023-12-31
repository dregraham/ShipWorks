﻿-- OrderCharge
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OrderCharge_OrderID] from [dbo].[OrderCharge]'
GO
DROP INDEX [IX_OrderCharge_OrderID] ON [dbo].[OrderCharge]
GO
PRINT N'Creating index [IX_OrderCharge_OrderID] on [dbo].[OrderCharge]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderCharge_OrderID] ON [dbo].[OrderCharge] ([OrderID] ASC, [OrderChargeID] ASC)
GO