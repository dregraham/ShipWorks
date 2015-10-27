SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[AmazonProfile]'
GO
CREATE TABLE [dbo].[AmazonProfile]
(
[ShippingProfileID] [bigint] NOT NULL,
[InsuranceValue] [money] NOT NULL CONSTRAINT [DF_AmazonProfile_InsuranceValue] DEFAULT ((0)),
[DimsShippingProfileID] [bigint] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsShippingProfileID] DEFAULT ((0)),
[DimsLength] [float] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsLength] DEFAULT ((0)),
[DimsWidth] [float] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsWidth] DEFAULT ((0)),
[DimsHeight] [float] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsHeight] DEFAULT ((0)),
[DimsWeight] [float] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsWeight] DEFAULT ((0)),
[DimsAddWeight] [bit] NOT NULL CONSTRAINT [DF_AmazonProfile_DimsAddWeight] DEFAULT ((0)),
[DeliveryExperience] [int] NOT NULL CONSTRAINT [DF_AmazonProfile_DeliveryExperience] DEFAULT ((0)),
[CarrierWillPickUp] [bit] NOT NULL CONSTRAINT [DF_AmazonProfile_CarrierWillPickUp] DEFAULT ((0)),
[DeclaredValue] [money] NULL
)
GO
PRINT N'Creating primary key [PK_AmazonProfile] on [dbo].[AmazonProfile]'
GO
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [PK_AmazonProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
-- Foreign Keys
ALTER TABLE [dbo].[AmazonProfile] ADD CONSTRAINT [FK_AmazonProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
GO
