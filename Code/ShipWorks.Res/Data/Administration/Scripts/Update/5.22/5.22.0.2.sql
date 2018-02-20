CREATE TABLE [dbo].[Shortcut](
	[ShortcutID] [bigint] NOT NULL IDENTITY(1105, 1000),
	[Barcode] [nvarchar](50) NOT NULL,
	[Hotkey] [int] NULL,
	[Action] [int] NOT NULL,
	[RelatedObjectID] [bigint] NULL,
 CONSTRAINT [PK_Shortcut] PRIMARY KEY CLUSTERED 
(
	[ShortcutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE dbo.Tmp_ShippingProfile
	(
	ShippingProfileID bigint NOT NULL IDENTITY (1053, 1000),
	RowVersion timestamp NOT NULL,
	Name nvarchar(50) NOT NULL,
	ShipmentType int NULL,
	ShipmentTypePrimary bit NOT NULL,
	OriginID bigint NULL,
	Insurance bit NULL,
	InsuranceInitialValueSource int NULL,
	InsuranceInitialValueAmount money NULL,
	ReturnShipment bit NULL,
	RequestedLabelFormat int NULL
	)  ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_ShippingProfile ON
GO
IF EXISTS(SELECT * FROM dbo.ShippingProfile)
	 EXEC('INSERT INTO dbo.Tmp_ShippingProfile (ShippingProfileID, Name, ShipmentType, ShipmentTypePrimary, OriginID, Insurance, InsuranceInitialValueSource, InsuranceInitialValueAmount, ReturnShipment, RequestedLabelFormat)
		SELECT ShippingProfileID, Name, ShipmentType, ShipmentTypePrimary, OriginID, Insurance, InsuranceInitialValueSource, InsuranceInitialValueAmount, ReturnShipment, RequestedLabelFormat FROM dbo.ShippingProfile WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ShippingProfile OFF
GO
ALTER TABLE dbo.iParcelProfile
	DROP CONSTRAINT FK_iParcelProfile_ShippingProfile
GO
ALTER TABLE dbo.OnTracProfile
	DROP CONSTRAINT FK_OnTracProfile_ShippingProfile
GO
ALTER TABLE dbo.OtherProfile
	DROP CONSTRAINT FK_OtherProfile_ShippingProfile
GO
ALTER TABLE dbo.PostalProfile
	DROP CONSTRAINT FK_PostalProfile_ShippingProfile
GO
ALTER TABLE dbo.UpsProfile
	DROP CONSTRAINT FK_UpsProfile_ShippingProfile
GO
ALTER TABLE dbo.PackageProfile
	DROP CONSTRAINT FK_PackageProfile_ShippingProfile
GO
ALTER TABLE dbo.BestRateProfile
	DROP CONSTRAINT FK_BestRateProfile_ShippingProfile
GO
ALTER TABLE dbo.FedExProfile
	DROP CONSTRAINT FK_FedExProfile_ShippingProfile
GO
ALTER TABLE dbo.AmazonProfile
	DROP CONSTRAINT FK_AmazonProfile_ShippingProfile
GO
ALTER TABLE dbo.DhlExpressProfile
	DROP CONSTRAINT FK_DhlExpressProfile_ShippingProfile
GO
ALTER TABLE dbo.AsendiaProfile
	DROP CONSTRAINT FK_AsendiaProfile_ShippingProfile
GO
DROP TABLE dbo.ShippingProfile
GO
EXECUTE sp_rename N'dbo.Tmp_ShippingProfile', N'ShippingProfile', 'OBJECT' 
GO
ALTER TABLE dbo.ShippingProfile ADD CONSTRAINT
	PK_ShippingProfile PRIMARY KEY CLUSTERED 
	(
	ShippingProfileID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ShippingProfile ENABLE CHANGE_TRACKING
GO
ALTER TABLE dbo.AsendiaProfile ADD CONSTRAINT
	FK_AsendiaProfile_ShippingProfile FOREIGN KEY
	(
	ShippingProfileID
	) REFERENCES dbo.ShippingProfile
	(
	ShippingProfileID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
GO