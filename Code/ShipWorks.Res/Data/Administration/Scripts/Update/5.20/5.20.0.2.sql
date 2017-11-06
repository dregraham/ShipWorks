PRINT N'Creating [dbo].[AsendiaProfile]'
GO
CREATE TABLE [dbo].[AsendiaProfile](
	[ShippingProfileID] [bigint] NOT NULL,
	[AsendiaAccountID] [bigint] NULL,
	[Service] [int] NULL,
	[NonMachinable] [bit] NULL,
	[Contents] [int] NULL,
	[NonDelivery] [int] NULL,
	[Weight] [float] NULL,
	[DimsProfileID] [bigint] NULL,
	[DimsLength] [float] NULL,
	[DimsWidth] [float] NULL,
	[DimsHeight] [float] NULL,
	[DimsWeight] [float] NULL,
	[DimsAddWeight] [bit] NULL,
)
GO
PRINT N'Creating primary key [PK_AsendiaProfile] on [dbo].[AsendiaProfile]'
GO
ALTER TABLE [dbo].[AsendiaProfile] ADD CONSTRAINT [PK_AsendiaProfile] PRIMARY KEY CLUSTERED  ([ShippingProfileID])
GO
PRINT N'Adding foreign keys to [dbo].[AsemdoaProfile]'
GO
ALTER TABLE [dbo].[AsendiaProfile] ADD CONSTRAINT [FK_AsendiaProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID])
GO