-- OrderPaymentDetail
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping index [IX_OrderPaymentDetail_OrderID] from [dbo].[OrderPaymentDetail]'
GO
DROP INDEX [IX_OrderPaymentDetail_OrderID] ON [dbo].[OrderPaymentDetail]
GO
PRINT N'Creating index [IX_OrderPaymentDetail_OrderID] on [dbo].[OrderPaymentDetail]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OrderPaymentDetail_OrderID] ON [dbo].[OrderPaymentDetail] ([OrderID] ASC, [OrderPaymentDetailID] ASC)
GO