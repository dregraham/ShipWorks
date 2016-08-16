SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Disabling table change tracking'
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] DISABLE CHANGE_TRACKING
GO
PRINT N'Dropping constraints from [dbo].[ShippingDefaultsRule]'
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] DROP CONSTRAINT [PK_ShippingDefaultsRule]
GO
PRINT N'Rebuilding [dbo].[ShippingDefaultsRule]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ShippingDefaultsRule]
(
[ShippingDefaultsRuleID] [bigint] NOT NULL IDENTITY(1057, 1000),
[RowVersion] [timestamp] NOT NULL,
[ShipmentType] [int] NOT NULL,
[FilterNodeID] [bigint] NOT NULL,
[ShippingProfileID] [bigint] NOT NULL,
[Position] [int] NOT NULL
)
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShippingDefaultsRule] ON
GO
INSERT INTO [dbo].[tmp_rg_xx_ShippingDefaultsRule]([ShippingDefaultsRuleID], [ShipmentType], [FilterNodeID], [ShippingProfileID], Position) SELECT [ShippingDefaultsRuleID], [ShipmentType], [FilterNodeID], [ShippingProfileID], 0 FROM [dbo].[ShippingDefaultsRule]
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ShippingDefaultsRule] OFF
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ShippingDefaultsRule]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ShippingDefaultsRule]', RESEED, @idVal)
GO
DROP TABLE [dbo].[ShippingDefaultsRule]
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ShippingDefaultsRule]', N'ShippingDefaultsRule'
GO
PRINT N'Creating primary key [PK_ShippingDefaultsRule] on [dbo].[ShippingDefaultsRule]'
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] ADD CONSTRAINT [PK_ShippingDefaultsRule] PRIMARY KEY CLUSTERED  ([ShippingDefaultsRuleID])
GO
ALTER TABLE [dbo].[ShippingDefaultsRule] ENABLE CHANGE_TRACKING
GO
PRINT N'Altering [dbo].[ShippingDefaultsRule]'
GO
