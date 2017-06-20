﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
GO
PRINT N'Creating index [IX_MagentoOrderID] on [dbo].[MagentoOrder]'
GO
CREATE NONCLUSTERED INDEX [IX_MagentoOrderID] ON [dbo].[MagentoOrder] ([MagentoOrderID] ASC)
GO