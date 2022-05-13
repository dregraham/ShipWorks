PRINT N'Dropping table [dbo].[DhlEcommerceScanForm]'
GO
DROP TABLE IF EXISTS [dbo].[DhlEcommerceScanForm]
GO

PRINT N'Creating [dbo].[ShipEngineManifest]'
GO
IF OBJECT_ID(N'[dbo].[ShipEngineManifest]', 'U') IS NULL
	CREATE TABLE [dbo].[ShipEngineManifest](
		[ShipEngineManifestID] [bigint] IDENTITY(1107,1000) NOT NULL,
		[CarrierAccountID] [bigint] NOT NULL,
		[ShipmentTypeCode] [int] NOT NULL,
		[ManifestID] [varchar](50) NOT NULL,
		[FormID] [varchar](50) NOT NULL,
		[CreatedAt] [datetime] NOT NULL,
		[ShipDate] [datetime] NOT NULL,
		[ShipmentCount] [int] NOT NULL,
		[PlatformWarehouseID] [varchar](50) NOT NULL,
		[SubmissionID] [varchar](255) NOT NULL,
		[CarrierID] [varchar](50) NOT NULL,
		[ManifestUrl] [varchar](2048) NOT NULL
	 CONSTRAINT [PK_ShipEngineManifest] PRIMARY KEY CLUSTERED 
	(
		[ShipEngineManifestID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO