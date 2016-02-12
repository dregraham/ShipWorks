SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Altering [dbo].[ChannelAdvisorOrderItem]'
GO
ALTER TABLE [dbo].[ChannelAdvisorOrderItem] ADD
[IsFBA] [bit] NOT NULL CONSTRAINT [DF_ChannelAdvisorOrderItem_IsFBA] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[EbayOrder]'
GO
ALTER TABLE [dbo].[EbayOrder] ADD
[IsEligibleForGlobalShippingProgram] [bit] NOT NULL CONSTRAINT [DF__EbayOrder__IsEli__2A2C1B24] DEFAULT ((0)),
[SelectedShippingMethod] [int] NOT NULL CONSTRAINT [DF__EbayOrder__Selec__2B203F5D] DEFAULT ((0)),
[GlobalShippingProgramFirstName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__2C146396] DEFAULT (''),
[GlobalShippingProgramLastName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__2D0887CF] DEFAULT (''),
[GlobalShippingProgramStreet1] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__2DFCAC08] DEFAULT (''),
[GlobalShippingProgramStreet2] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__2EF0D041] DEFAULT (''),
[GlobalShippingProgramCity] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__2FE4F47A] DEFAULT (''),
[GlobalShippingProgramStateProvince] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__30D918B3] DEFAULT (''),
[GlobalShippingProgramPostalCode] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__31CD3CEC] DEFAULT (''),
[GlobalShippingProgramCountryCode] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__32C16125] DEFAULT (''),
[GlobalShippingProgramReferenceID] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__EbayOrder__Globa__33B5855E] DEFAULT ('')
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO