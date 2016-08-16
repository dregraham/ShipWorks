SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP
CONSTRAINT [FK_EbayOrder_Order]
GO
PRINT N'Dropping foreign keys from [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] DROP
CONSTRAINT [FK_EbayOrderItem_EbayOrder]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [PK_EbayOrder]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__IsEli__2A2C1B24]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Selec__2B203F5D]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__2C146396]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__2D0887CF]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__2DFCAC08]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__2EF0D041]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__2FE4F47A]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__30D918B3]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__31CD3CEC]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__32C16125]
GO
PRINT N'Dropping constraints from [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] DROP CONSTRAINT [DF__EbayOrder__Globa__33B5855E]
GO
PRINT N'Dropping constraints from [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] DROP CONSTRAINT [DF_ChannelAdvisorOrderItem_IsFBA]
GO
PRINT N'Rebuilding [dbo].[EbayOrder]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_EbayOrder]
(
[OrderID] [bigint] NOT NULL,
[EbayOrderID] [bigint] NOT NULL,
[EbayBuyerID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BuyerFeedbackScore] [int] NOT NULL,
[BuyerFeedbackPrivate] [bit] NOT NULL,
[CombinedLocally] [bit] NOT NULL,
[SelectedShippingMethod] [int] NOT NULL CONSTRAINT [DF__EbayOrder__Selec__2B203F5D] DEFAULT ((0)),
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
[RollupPayPalAddressStatus] [int] NULL,
[RollupSellingManagerRecord] [int] NULL
)
GO
INSERT INTO [dbo].[tmp_rg_xx_EbayOrder](
     [OrderID], [EbayOrderID], [EbayBuyerID], [BuyerFeedbackScore], [BuyerFeedbackPrivate], [CombinedLocally], [SelectedShippingMethod], 
     [GspEligible], [GspFirstName], [GspLastName], [GspStreet1], [GspStreet2], [GspCity], [GspStateProvince], [GspPostalCode], [GspCountryCode], [GspReferenceID], 
	 [RollupEbayItemCount], [RollupEffectiveCheckoutStatus], [RollupEffectivePaymentMethod], [RollupFeedbackLeftType], [RollupFeedbackLeftComments], [RollupFeedbackReceivedType], [RollupFeedbackReceivedComments], [RollupPayPalAddressStatus], [RollupSellingManagerRecord]) 
SELECT 
     [OrderID], [EbayOrderID], [EbayBuyerID], [BuyerFeedbackScore], [BuyerFeedbackPrivate], [CombinedLocally], [SelectedShippingMethod], 
	 [IsEligibleForGlobalShippingProgram], [GlobalShippingProgramFirstName], [GlobalShippingProgramLastName], [GlobalShippingProgramStreet1], [GlobalShippingProgramStreet2], [GlobalShippingProgramCity], [GlobalShippingProgramStateProvince], [GlobalShippingProgramPostalCode], [GlobalShippingProgramCountryCode], [GlobalShippingProgramReferenceID], 
	 [RollupEbayItemCount], [RollupEffectiveCheckoutStatus], [RollupEffectivePaymentMethod], [RollupFeedbackLeftType], [RollupFeedbackLeftComments], [RollupFeedbackReceivedType], [RollupFeedbackReceivedComments], [RollupPayPalAddressStatus], [RollupSellingManagerRecord] FROM [dbo].[EbayOrder]
GO
DROP TABLE [dbo].[EbayOrder]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_EbayOrder]', N'EbayOrder'
GO
PRINT N'Creating primary key [PK_EbayOrder] on [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD CONSTRAINT [PK_EbayOrder] PRIMARY KEY CLUSTERED  ([OrderID])
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ALTER COLUMN [IsFBA] [bit] NOT NULL
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD
CONSTRAINT [FK_EbayOrder_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
GO
PRINT N'Adding foreign keys to [dbo].[EbayOrderItem]'
GO
ALTER TABLE [dbo].[EbayOrderItem] ADD
CONSTRAINT [FK_EbayOrderItem_EbayOrder] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[EbayOrder] ([OrderID])
GO
