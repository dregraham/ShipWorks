SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Dropping foreign keys from [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [FK_UserSetting_User]
GO
PRINT N'Dropping constraints from [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] DROP CONSTRAINT [PK_UserSetting_1]
GO
PRINT N'Rebuilding [dbo].[UserSettings]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_UserSettings]
(
[UserID] [bigint] NOT NULL,
[DisplayColorScheme] [int] NOT NULL,
[DisplaySystemTray] [bit] NOT NULL,
[WindowLayout] [varbinary] (max) NOT NULL,
[GridMenuLayout] [xml] NULL,
[FilterInitialUseLastActive] [bit] NOT NULL,
[FilterInitialSpecified] [bigint] NOT NULL,
[FilterInitialSortType] [int] NOT NULL,
[OrderFilterLastActive] [bigint] NOT NULL,
[OrderFilterExpandedFolders] [xml] NULL,
[ShippingWeightFormat] [int] NOT NULL,
[TemplateExpandedFolders] [xml] NULL,
[TemplateLastSelected] [bigint] NOT NULL,
[CustomerFilterLastActive] [bigint] NOT NULL,
[CustomerFilterExpandedFolders] [xml] NULL,
[SingleScanSettings] [int] NOT NULL,
[AutoWeigh] [bit] NOT NULL,
[DialogSettings] [xml] NULL,
[UIMode] [int] NOT NULL
)
GO
INSERT INTO [dbo].[RG_Recovery_1_UserSettings]([UserID], [DisplayColorScheme], [DisplaySystemTray], [WindowLayout], [GridMenuLayout], [FilterInitialUseLastActive], [FilterInitialSpecified], [FilterInitialSortType], [OrderFilterLastActive], [OrderFilterExpandedFolders], [ShippingWeightFormat], [TemplateExpandedFolders], [TemplateLastSelected], [CustomerFilterLastActive], [CustomerFilterExpandedFolders], [SingleScanSettings], [AutoWeigh], [DialogSettings], [UIMode]) 
SELECT [UserID], [DisplayColorScheme], [DisplaySystemTray], [WindowLayout], [GridMenuLayout], [FilterInitialUseLastActive], [FilterInitialSpecified], [FilterInitialSortType], [OrderFilterLastActive], [OrderFilterExpandedFolders], [ShippingWeightFormat], [TemplateExpandedFolders], [TemplateLastSelected], [CustomerFilterLastActive], [CustomerFilterExpandedFolders], [SingleScanSettings], [AutoWeigh], [DialogSettings], 0
FROM [dbo].[UserSettings]
GO
DROP TABLE [dbo].[UserSettings]
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_UserSettings]', N'UserSettings', N'OBJECT'
GO
PRINT N'Creating primary key [PK_UserSetting_1] on [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [PK_UserSetting_1] PRIMARY KEY CLUSTERED  ([UserID])
GO
PRINT N'Adding foreign keys to [dbo].[UserSettings]'
GO
ALTER TABLE [dbo].[UserSettings] ADD CONSTRAINT [FK_UserSetting_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
GO