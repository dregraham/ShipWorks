﻿
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Add Ebay combine index'
GO
CREATE NONCLUSTERED INDEX [IX_EbayOrder_OrderID_Includes_CheckoutStatus_GspEligible] ON [dbo].[EbayOrder]
(
	[OrderID] ASC
) INCLUDE (
	[RollupEffectiveCheckoutStatus], [GspEligible]
)
ON [PRIMARY]
GO

PRINT N'Creating custom types'
GO
CREATE TYPE dbo.LongList AS TABLE ( item BIGINT );
GO