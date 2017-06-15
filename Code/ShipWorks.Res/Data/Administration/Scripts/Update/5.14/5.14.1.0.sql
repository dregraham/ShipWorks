SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

PRINT N'Removing [ChannelAdvisorStore].[AmazonShippingToken]'
IF COL_LENGTH('dbo.ChannelAdvisorStore', 'AmazonShippingToken') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ChannelAdvisorStore DROP COLUMN AmazonShippingToken;
END
GO

PRINT N'Removing [AmazonStore].[DF_AmazonStore_AmazonShippingToken]'
-- AmazonStore.AmazonShippingToken has a default constraint we have to remove it before we can drop the column
IF EXISTS(SELECT * FROM sysconstraints WHERE id=OBJECT_ID('dbo.AmazonStore') AND COL_NAME(id,colid)='AmazonShippingToken' AND OBJECTPROPERTY(constid, 'IsDefaultCnst')=1)
BEGIN
	ALTER TABLE dbo.AmazonStore DROP CONSTRAINT DF_AmazonStore_AmazonShippingToken
END
GO

PRINT N'Removing [AmazonStore].[AmazonShippingToken]'
IF COL_LENGTH('dbo.AmazonStore', 'AmazonShippingToken') IS NOT NULL
BEGIN
    ALTER TABLE dbo.AmazonStore DROP COLUMN AmazonShippingToken;
END
GO