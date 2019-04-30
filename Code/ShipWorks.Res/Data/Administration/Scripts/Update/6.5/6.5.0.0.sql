﻿PRINT N'Altering [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ADD
[IncludeReturn] [bit] NOT NULL CONSTRAINT [DF_ShippingProfile_IncludeReturn] DEFAULT ((0)),
[ApplyReturnProfile] [bit] NOT NULL CONSTRAINT [DF_ShippingProfile_ApplyReturnProfile] DEFAULT ((0)),
[ReturnProfileID] [int] NOT NULL CONSTRAINT [DF_ShippingProfile_ReturnProfileID] DEFAULT ((-1))
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
[IncludeReturns] [bit] NOT NULL CONSTRAINT [DF_Shipment_IncludeReturns] DEFAULT ((0)),
[ApplyReturnProfile] [bit] NOT NULL CONSTRAINT [DF_Shipment_ApplyReturnProfile] DEFAULT ((0)),
[ReturnProfileID] [int] NOT NULL CONSTRAINT [DF_Shipment_ReturnProfileID] DEFAULT ((-1))
GO