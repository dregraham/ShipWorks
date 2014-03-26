CREATE TABLE [dbo].[WorldShipProcessed] (
    [WorldShipProcessedID] BIGINT        IDENTITY (1068, 1000) NOT NULL,
    [ShipmentID]           NVARCHAR (50) NULL,
    [RowVersion]           ROWVERSION    NOT NULL,
    [PublishedCharges]     FLOAT (53)    NOT NULL,
    [NegotiatedCharges]    FLOAT (53)    NOT NULL,
    [TrackingNumber]       NVARCHAR (50) NULL,
    [UspsTrackingNumber]   NVARCHAR (50) NULL,
    [ServiceType]          NVARCHAR (50) NULL,
    [PackageType]          NVARCHAR (50) NULL,
    [UpsPackageID]         NVARCHAR (20) NULL,
    [DeclaredValueAmount]  FLOAT (53)    NULL,
    [DeclaredValueOption]  NCHAR (2)     NULL,
    [WorldShipShipmentID]  NVARCHAR (50) NULL,
    [VoidIndicator]        NVARCHAR (50) NULL,
    [NumberOfPackages]     NVARCHAR (50) NULL,
    [LeadTrackingNumber]   NVARCHAR (50) NULL,
    [ShipmentIdCalculated] AS            (case when isnumeric([ShipmentID]+'.e0')=(1) then CONVERT([bigint],[ShipmentID],(0))  end) PERSISTED,
    CONSTRAINT [PK_WorldShipProcessed] PRIMARY KEY CLUSTERED ([WorldShipProcessedID] ASC)
);

