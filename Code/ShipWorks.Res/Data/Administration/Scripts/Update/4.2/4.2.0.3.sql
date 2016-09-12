﻿SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
PRINT N'Creating [dbo].[ExcludedServiceType]'
GO
CREATE TABLE [dbo].[ExcludedServiceType](
	[ShipmentType] [int] NOT NULL,
	[ServiceType] [int] NOT NULL,
 CONSTRAINT [PK_ExcludedServiceType] PRIMARY KEY CLUSTERED 
(
	[ShipmentType] ASC,
	[ServiceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO