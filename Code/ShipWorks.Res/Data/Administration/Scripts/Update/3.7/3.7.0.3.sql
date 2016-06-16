SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'CombinedLocally'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEbayItemCount'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectiveCheckoutStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectivePaymentMethod'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftComments'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedComments'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedType'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupPayPalAddressStatus'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupSellingManagerRecord'
GO
EXEC sp_dropextendedproperty N'AuditFormat', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_dropextendedproperty N'AuditName', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [FK_EbayOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] DROP CONSTRAINT [FK_EbayOrderItem_EbayOrder]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [PK_EbayOrder]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Selec__2B203F5D]
GO
PRINT N'Rebuilding [dbo].[EbayOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EbayOrder]
(
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CombinedLocally] [bit] NOT NULL,
[SelectedShippingMethod] [int] NOT NULL CONSTRAINT [DF__EbayOrder__Selec__2B203F5D] DEFAULT ((0)),
[SellingManagerRecord] [int] NULL,
[GspEligible] [bit] NOT NULL,
[GspFirstName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspLastName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStreet1] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStreet2] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspCity] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspStateProvince] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspPostalCode] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspCountryCode] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[GspReferenceID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RollupEbayItemCount] [int] NOT NULL,
[RollupEffectiveCheckoutStatus] [int] NULL,
[RollupEffectivePaymentMethod] [int] NULL,
[RollupFeedbackLeftType] [int] NULL,
[RollupFeedbackLeftComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupFeedbackReceivedType] [int] NULL,
[RollupFeedbackReceivedComments] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RollupPayPalAddressStatus] [int] NULL
) ON [PRIMARY]
GO
INSERT INTO [dbo].[tmp_rg_xx_EbayOrder]([OrderID], [EbayOrderID], [EbayBuyerID], [CombinedLocally], [SelectedShippingMethod], [SellingManagerRecord], [GspEligible], [GspFirstName], [GspLastName], [GspStreet1], [GspStreet2], [GspCity], [GspStateProvince], [GspPostalCode], [GspCountryCode], [GspReferenceID], [RollupEbayItemCount], [RollupEffectiveCheckoutStatus], [RollupEffectivePaymentMethod], [RollupFeedbackLeftType], [RollupFeedbackLeftComments], [RollupFeedbackReceivedType], [RollupFeedbackReceivedComments], [RollupPayPalAddressStatus]) SELECT [OrderID], [EbayOrderID], [EbayBuyerID], [CombinedLocally], [SelectedShippingMethod], [RollupSellingManagerRecord], [GspEligible], [GspFirstName], [GspLastName], [GspStreet1], [GspStreet2], [GspCity], [GspStateProvince], [GspPostalCode], [GspCountryCode], [GspReferenceID], [RollupEbayItemCount], [RollupEffectiveCheckoutStatus], [RollupEffectivePaymentMethod], [RollupFeedbackLeftType], [RollupFeedbackLeftComments], [RollupFeedbackReceivedType], [RollupFeedbackReceivedComments], [RollupPayPalAddressStatus] FROM [dbo].[EbayOrder]
GO
DROP TABLE [dbo].[EbayOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EbayOrder]', N'EbayOrder'
GO
PRINT N'Creating primary key [PK_EbayOrder] on [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [PK_EbayOrder] PRIMARY KEY CLUSTERED  ([OrderID]) ON [PRIMARY]
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD CONSTRAINT [FK_EbayOrderItem_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID])
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'CombinedLocally'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEbayItemCount'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectiveCheckoutStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupEffectivePaymentMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackLeftType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedComments'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupFeedbackReceivedType'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'RollupPayPalAddressStatus'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'128', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditName', N'Shipping Method', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SelectedShippingMethod'
GO
EXEC sp_addextendedproperty N'AuditFormat', N'1', 'SCHEMA', N'dbo', 'TABLE', N'EbayOrder', 'COLUMN', N'SellingManagerRecord'
GO
