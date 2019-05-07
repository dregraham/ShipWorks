PRINT N'Altering [dbo].[ShippingProfile]'
GO
ALTER TABLE [dbo].[ShippingProfile] ADD
[IncludeReturn] [bit] NULL,
[ApplyReturnProfile] [bit] NULL,
[ReturnProfileID] [bigint] NULL
GO
PRINT N'Altering [dbo].[Shipment]'
GO
ALTER TABLE [dbo].[Shipment] ADD
[IncludeReturn] [bit] NOT NULL CONSTRAINT [DF_Shipment_IncludeReturns] DEFAULT ((0)),
[ApplyReturnProfile] [bit] NOT NULL CONSTRAINT [DF_Shipment_ApplyReturnProfile] DEFAULT ((0)),
[ReturnProfileID] [bigint] NOT NULL CONSTRAINT [DF_Shipment_ReturnProfileID] DEFAULT ((-1))
GO
