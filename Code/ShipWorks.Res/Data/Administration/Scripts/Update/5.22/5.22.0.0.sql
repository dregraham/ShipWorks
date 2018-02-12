PRINT N'Creating [dbo].[PackageProfile]'
GO
CREATE TABLE [dbo].[PackageProfile]
(
[PackageProfileID] [bigint] NOT NULL IDENTITY(1104, 1000),
[ShippingProfileID] [bigint] NOT NULL,
[Weight] [float] NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_PackageProfile] on [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [PK_PackageProfile] PRIMARY KEY CLUSTERED  ([PackageProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[PackageProfile]'
GO
ALTER TABLE [dbo].[PackageProfile] ADD CONSTRAINT [FK_PackageProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO