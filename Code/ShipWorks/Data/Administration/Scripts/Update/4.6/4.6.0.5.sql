SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[AmazonProfile]'
GO
CREATE TABLE [dbo].[AmazonProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[DimsProfileID] [bigint] NULL,
[DimsLength] [float] NULL,
[DimsWidth] [float] NULL,
[DimsHeight] [float] NULL,
[DimsWeight] [float] NULL,
[DimsAddWeight] [bit] NULL,
[DeliveryExperience] [int] NULL,
[CarrierWillPickUp] [bit] NULL,
[Weight] [float] NULL,
[SendDateMustArriveBy] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_AmazonProfile] on [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [PK_AmazonProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
-- Foreign Keys
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [FK_AmazonProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
